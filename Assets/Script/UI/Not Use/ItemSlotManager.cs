using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlotManager : MonoBehaviour
{
    public Transform slotGrid;
    public ItemSlot itemSlotPrefab;

    private List<ItemSlot> itemSlots;


    // 디버그용
    private void Start()
    {
        itemSlots = new List<ItemSlot>();
        for (int i = 0; i < 3; i++)
        {
            ItemSlot slot = Instantiate(itemSlotPrefab, slotGrid);
            slot.SetItemInfo(ref i);
            itemSlots.Add(slot);
        }
    }

    public void CreateItemSlots(int amount)
    {
        itemSlots.Clear();
        for (int i = 0; i < amount; i++)
        {
            itemSlots.Add(Instantiate(itemSlotPrefab, slotGrid));
        }
    }

    public void SetItemSlots(Sprite sprite, ref int itemInfoIndex)
    {
        itemSlots[itemInfoIndex].SetItemInfo(ref itemInfoIndex, sprite);
    }
}
