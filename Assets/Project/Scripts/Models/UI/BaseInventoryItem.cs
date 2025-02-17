using System.Collections.Generic;
using UnityEngine;

namespace Bonjoura.Inventory
{
    [CreateAssetMenu(fileName = "BaseInventoryItem", menuName = "Scriptable Objects/BaseInventoryItem")]
    public sealed class BaseInventoryItem : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField, Min(0)] private int itemIndex;
        [SerializeField] private bool canStacking;
        [SerializeField] private int maxStack = 64;

        [Header("Icon")]
        [SerializeField] private Sprite itemIcon;
        [SerializeField] private Vector3 iconScale = new Vector3(0.5f, 0.5f, 0.5f);

        [Header("Type")]
        [SerializeField] private DefaultType defaultType;
        [SerializeField] private List<OtherType> otherTypes;
        [SerializeField] private BaseInventoryItem dropWhenCoocked;
        [SerializeField] private float repairHunger;


        public string ItemName => itemName;
        public int ItemIndex => itemIndex;
        public Sprite ItemIcon => itemIcon;
        public Vector3 IconScale => iconScale;
        public bool CanStacking => canStacking;
        public int MaxStack => maxStack;
        public float RepairHunger => repairHunger;
        public BaseInventoryItem DropWhenCoocked => dropWhenCoocked;

        public DefaultType DefaultType => defaultType;
        public List<OtherType> OtherTypes => otherTypes;

        public BaseInventoryItem GetCookedVersion()
        {

            if (otherTypes.Contains(OtherType.Cooked))
            {
                return this;
            }


            BaseInventoryItem cookedFood = Instantiate(this);


            if (cookedFood.OtherTypes.Contains(OtherType.Raw))
            {
                cookedFood.OtherTypes.Remove(OtherType.Raw);
                cookedFood.OtherTypes.Add(OtherType.Cooked);
            }

            return cookedFood;
        }
    }
}