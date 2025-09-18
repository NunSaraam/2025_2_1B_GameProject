using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("게임 상태")]
    public int playerScore = 0;
    public int itemsColledted = 0;

    [Header("UI참조")]
    public Text scoreText;
    public Text itemCountText;
    public Text gameStatusText;

    public static GameManager Instance;             //싱글턴 패턴

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectItem()
    {
        itemsColledted++;
        Debug.Log($"아이템 수집! (총 : {itemsColledted} 개");
    }

    public void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "점수 :" + playerScore;
        }

        if (itemCountText != null)
        {
            itemCountText.text = "아이템 :" + itemsColledted + "개";
        }
    }
}
