using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    [Header("UI 요소들")]
    public GameObject questUI;                                  //퀘스트 패널 UI
    public Text questTiltleText;                                //퀘스트 타이틀 텍스트
    public Text questDescriptionText;                           //퀘스트 내용
    public Text questProgressText;                              //진행 상태
    public Button completeButton;                               //완료 버튼

    [Header("퀘스트 목록")]
    public QuestData[] availableQuests;                         //가지고 있는 퀘스트 목록

    private QuestData currentQuest;                              //진행중인 퀘스트 데이터
    private int currentQuestIndex = 0;                          //퀘스트 목록중에 진행 중인 번호

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (availableQuests.Length > 0)
        {
            StartQuest(availableQuests[0]);                                                                     //시작시 가지고 있는 첫번째 배열의 퀘스트를 진행
        }
        if (completeButton != null)
        {
            completeButton.onClick.AddListener(ComplateCurrentQuest);                                           //완료 버튼을 완료 함수와 연결
        }
    }

    private void Update()
    {
        if (currentQuest != null && currentQuest.isActive)                                                      //퀘스트 진행중인지 체크 후
        {
            CheckQuestProgress();                                                                               //퀘스트 진행상태 함수 호출
            UpdateQuestUI();                                                                                    //퀘스트 UI 함수 호출
        }
    }

    //UI 업데이트 (퀘스트 진행 상황 UI로 표시)
    void UpdateQuestUI()
    {
        if (currentQuest == null) return;

        if (questTiltleText != null)
        {
            questTiltleText.text = currentQuest.questTitle;
        }

        if (questDescriptionText != null)
        {
            questDescriptionText.text = currentQuest.description;
        }

        if (questProgressText != null)
        {
            questProgressText.text = currentQuest.GetProgerssText();
        }
    }

    //퀘스트 시작
    public void StartQuest(QuestData quest)
    {
        if (quest == null) return;

        currentQuest = quest;                                    //퀘스트를 받아와서 CurrentQuest에 셋팅한다.
        currentQuest.Initalize();
        currentQuest.isActive = true;

        Debug.Log("퀘스트 시작 : " + questTiltleText);
        UpdateQuestUI();

        if (questUI != null)
        {
            questUI.SetActive(true);
        }
    }

    void CheckDeliveryProgress()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;               //플레이어 위치를 찾는다
        if (player == null) return;

        float distance = Vector3.Distance(player.position, currentQuest.deliveryPosition);       //플레이와 도착지 거리를 계산한다.

        if (distance <= currentQuest.deliveryRedius)                                             //플레이어의 거리가 도착 범위 안쪽인지 검사
        {
            if (currentQuest.currentProgress == 0)
            {
                currentQuest.currentProgress = 1;                                                //퀘스트 완료
            }
        }
        else
        {
            currentQuest.currentProgress = 0;                                                    //도착하지 못함
        }
    }

    //수집 퀘스트 진행 (외부에서 호출)
    public void AddCollectProgerss(string itemTag)
    {
        if (currentQuest == null || !currentQuest.isActive) return;

        if (currentQuest.questType == QuestType.Collect && currentQuest.targetTag == itemTag)
        {
            currentQuest.currentProgress++;
            Debug.Log("아이템 수집 :" + itemTag);
        }
    }

    //상호작용 퀘스트 진행 (외부에서 호출)
    public void AddinteractProgress(string objectTag)
    {
        if (currentQuest == null || !currentQuest.isActive) return;

        if (currentQuest.questType == QuestType.Interect && currentQuest.targetTag == objectTag)
        {
            currentQuest.currentProgress++;
            Debug.Log("상호 작용 완료:" + objectTag);
        }
    }

    //현재 퀘스트 완료
    public void ComplateCurrentQuest()
    {
        if (currentQuest == null || !currentQuest.isCompleted) return;

        Debug.Log("퀘스트 완료 ! " + currentQuest.rewardMessage);

        //완료 버튼 비활성화
        if (completeButton != null)
        {
            completeButton.gameObject.SetActive(false);
        }

        //다음 퀘스트가 있으면 시작
        currentQuestIndex++;
        if (currentQuestIndex < availableQuests.Length)
        {
            StartQuest(availableQuests[currentQuestIndex]);
        }
        else
        {
            currentQuest = null;

            if (questUI != null)
            {
                questUI.gameObject.SetActive(false);
            }
        }
    }

    //퀘스트 진행 체크
    void CheckQuestProgress()
    {
        if (currentQuest.questType == QuestType.Delivery)
        {
            CheckDeliveryProgress();
        }

        //퀘스트 완료 체크
        if (currentQuest.IsComplete() && !currentQuest.isCompleted)
        {
            currentQuest.isCompleted = true;

            //완료 버튼 활성화
            if (completeButton != null)
            {
                completeButton.gameObject.SetActive(true);
            }
        }
    }
}
