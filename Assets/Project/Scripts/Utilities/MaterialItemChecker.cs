using Bonjoura.UI;
using Bonjoura.Player;
using UnityEngine;

namespace Bonjoura.Craft
{
    public class MaterialItemChecker
    {
        private static MaterialItemChecker instance;
        public static MaterialItemChecker Instance => instance ??= new MaterialItemChecker();

        private MaterialItemChecker() { }

        public bool IsExist(MaterialItem material)
        {
            var inventory = PlayerController.Instance.ItemInventory;
            var quantity = inventory.GetItemQuantity(material.item);

            if (material.quantity > quantity)
                return false;

            return true;
        }
    }
}