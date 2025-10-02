using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
public class QuestData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string questTitle = "���ο� ����Ʈ";                                               //����Ʈ ����

    [TextArea(2, 4)]
    public string description = "����Ʈ ������ �Է��ϼ���.";                                  //����Ʈ ����
    public Sprite questIcon;                                                                  //����Ʈ ������

    [Header("����Ʈ ����")]
    public QuestType questType;
    public int targetAmount = 1;

    [Header("��� ����Ʈ�� (Delivery")]
    public Vector3 deliveryPosition;                                                          //��� ������
    public float deliveryRedius = 3f;                                                         //���� ���� ����

    [Header("����/��ȣ�ۿ� ����Ʈ��")]
    public string targetTag = "";                                                             //��� ������Ʈ �±� ����

    [Header("����")]
    public int experienceReward = 100;
    public string rewardMessage = "����Ʈ �Ϸ�";

    [Header("����Ʈ ����")]
    public QuestData nextQuest;

    //��Ÿ�� ������ (������� ����)
    [System.NonSerialized] public int currentProgress = 0;
    [System.NonSerialized] public bool isActive = false;
    [System.NonSerialized] public bool isCompleted = false;

    //����Ʈ �ʱ�ȭ
    public void Initalize()
    {
        currentProgress = 0;
        isActive = false;
        isCompleted = false;
    }

    //����Ʈ �Ϸ� üũ
    public bool IsComplete()
    {
        switch (questType)
        {
            case QuestType.Delivery:
                return currentProgress >= 1;

            case QuestType.Collect:
            case QuestType.Interect:
                return currentProgress >= targetAmount;

            default:
                return false;
        }
    }

    //����� ���(0.0 ~1.0)
    public float GetProgressPrecentage()
    {
        if (targetAmount <= 0) return 0;
        return Mathf.Clamp01((float)currentProgress / targetAmount);
    }


    //���� ��Ȳ �ؽ�Ʈ
    public string GetProgerssText()
    {
        switch (questType)
        {
            case QuestType.Delivery:
                return isCompleted ? "��� �Ϸ�" : "�������� �̵��ϼ���";

            case QuestType.Collect:
            case QuestType.Interect:
                return $"{currentProgress} / {targetAmount}";
            
            default:
                return "";
        }
    }
}

