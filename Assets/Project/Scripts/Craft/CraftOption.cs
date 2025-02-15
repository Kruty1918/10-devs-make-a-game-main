using Bonjoura.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace Bonjoura.Craft
{
    [CreateAssetMenu(fileName = "CraftOption", menuName = "Scriptable Objects/CraftOption")]
    public class CraftOption : ScriptableObject
    {
        [SerializeField] private int quantity = 1;
        [SerializeField] private BaseInventoryItem craftedItem;
        [SerializeField] private List<MaterialItem> materialList = new List<MaterialItem>();

        private const int MaxMaterial = 3;

        public int Quantity => quantity;
        public string CraftName => craftedItem.ItemName;
        public Sprite CraftIcon => craftedItem.ItemIcon;
        public BaseInventoryItem CraftedItem => craftedItem;
        public List<MaterialItem> MaterialList => materialList;

        private void OnValidate()
        {
            if(materialList.Count > MaxMaterial) 
            {
                materialList.RemoveRange(MaxMaterial, materialList.Count - MaxMaterial);
            }
        }
    }
}