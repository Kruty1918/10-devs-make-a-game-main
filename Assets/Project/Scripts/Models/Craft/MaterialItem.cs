using Bonjoura.UI;
using UnityEngine;

namespace Bonjoura.Craft
{
    [System.Serializable]
    public class MaterialItem
    {
        public BaseInventoryItem item;
        public int quantity;

        public int ItemIndex => item.ItemIndex;
        public Sprite ItemIcon => item.ItemIcon;
    }
}