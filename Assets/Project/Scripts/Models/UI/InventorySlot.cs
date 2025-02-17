using UnityEngine;

namespace Bonjoura.Inventory
{
    [System.Serializable]
    public class InventorySlot
    {
        public BaseInventoryItem item;
        public int quantity;
        
        public bool IsEmpty => !item; 

        public void ClearSlot()
        {
            item = null;
            quantity = 0;
        }
        
        public bool CanStack(BaseInventoryItem newItem)
        {
            return item == newItem && item.CanStacking && quantity < item.MaxStack;
        }

        public void AddAmount(int amount)
        {
            quantity = Mathf.Min(quantity + amount, item.MaxStack);
        }
    }
}