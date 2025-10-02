using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : InteractableObject
{
    [Header("���� ����")]
    public int coinValue = 10;
    public string questTag = "Coin";                                        //����Ʈ���� ����� �±�

    protected override void Start()
    {
        base.Start();
        objectName = "����";
        interactionText = "[E] ���� ȹ��";
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
