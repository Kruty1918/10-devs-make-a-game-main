using System;
using System.Collections.Generic;
using Bonjoura.Player;
using SGS29.Utilities;
using UnityEngine;

namespace Bonjoura.UI
{
    public sealed class ItemInventory : MonoBehaviour
    {
        [SerializeField] private int maxSlots = 20;

        [Header("Dropped item")]
        [SerializeField] private GameObject droppedItemReference;
        [SerializeField] private float forceDrop;

        public readonly List<InventorySlot> InventorySlots = new();

        public event Action OnRefreshItemEvent;

        private int _selectedSlotIndex = -1;

        private void Awake()
        {
            for (int i = 0; i < maxSlots; i++)
            {
                InventorySlots.Add(new InventorySlot());
            }
        }

        public bool RemoveItem(BaseInventoryItem itemToRemove)
        {
            foreach (var slot in InventorySlots)
            {
                if (slot.item == itemToRemove)
                {
                    slot.quantity -= 1;
                    if (slot.quantity <= 0)
                        slot.ClearSlot();
                    OnRefreshItemEvent?.Invoke();
                    return true;
                }
            }

            return false;
        }

        public int GetItemQuantity(BaseInventoryItem item)
        {
            int quantity = 0;

            foreach (var slot in InventorySlots)
            {
                if (slot.item == item)
                {
                    quantity += slot.quantity;
                }
            }

            return quantity;
        }

        public void RemoveItemWithDroppedItem(BaseInventoryItem itemToRemove)
        {
            if (RemoveItem(itemToRemove)) CreateDroppedItem(itemToRemove);
        }

        public void RemoveSelectedItem()
        {
            CreateDroppedItem(InventorySlots[_selectedSlotIndex].item);
            InventorySlots[_selectedSlotIndex].quantity -= 1;
            if (InventorySlots[_selectedSlotIndex].quantity <= 0)
                InventorySlots[_selectedSlotIndex].ClearSlot();
            OnRefreshItemEvent?.Invoke();
        }

        public void RemoveSelectedItemWithDropped()
        {
            CreateDroppedItem(InventorySlots[_selectedSlotIndex].item);
            InventorySlots[_selectedSlotIndex].quantity -= 1;
            if (InventorySlots[_selectedSlotIndex].quantity <= 0)
                InventorySlots[_selectedSlotIndex].ClearSlot();
            OnRefreshItemEvent?.Invoke();
        }

        // Вибір слота
        public void SelectSlot(int slotIndex)
        {
            _selectedSlotIndex = slotIndex;
        }

        private void CreateDroppedItem(BaseInventoryItem item)
        {
            GameObject droppedItemObject = Instantiate(droppedItemReference, SM.Instance<PlayerController>().FPSCamera.transform.position, Quaternion.identity);
            DroppedItem droppedItem = droppedItemObject.GetComponent<DroppedItem>();

            droppedItem.SetSprite(item.ItemIcon);
            droppedItem.SetSpriteScale(item.IconScale);
            droppedItem.Drop(SM.Instance<PlayerController>().FPSCamera.transform.forward, forceDrop);
            droppedItem.SetItem(item);
        }

        public bool AddItem(BaseInventoryItem newItem, int amount = 1)
        {
            foreach (var slot in InventorySlots)
            {
                if (!slot.IsEmpty && slot.CanStack(newItem))
                {
                    slot.AddAmount(amount);
                    OnRefreshItemEvent?.Invoke();
                    return true;
                }
            }

            foreach (var slot in InventorySlots)
            {
                if (slot.IsEmpty)
                {
                    slot.item = newItem;
                    slot.quantity = amount;
                    OnRefreshItemEvent?.Invoke();
                    return true;
                }
            }

            return false;
        }

        public void SwapSlots(int indexA, int indexB)
        {
            if (indexA == indexB) return;

            (InventorySlots[indexA], InventorySlots[indexB]) = (InventorySlots[indexB], InventorySlots[indexA]);
        }

    }
}
