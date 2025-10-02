using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : InteractableObject
{
    [Header("동전 설정")]
    public int coinValue = 10;
    public string questTag = "Coin";                                        //퀘스트에서 사용할 태그

    protected override void Start()
    {
        base.Start();
        objectName = "동전";
        interactionText = "[E] 동전 획득";
        interactionType = InteractionType.Item;
    }

    protected override void CollectItem()
    {
        if (QuestManager.instance != null)
        {
            QuestManager.instance.AddCollectProgerss(questTag);
        }
        Destroy(gameObject);
    }

}
