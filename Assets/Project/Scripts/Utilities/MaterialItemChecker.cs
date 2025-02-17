using Bonjoura.Player;
using Bonjoura.Craft;
using SGS29.Utilities;

namespace Bonjoura.Utilities
{
    public class MaterialItemChecker
    {
        private static MaterialItemChecker instance;
        public static MaterialItemChecker Instance => instance ??= new MaterialItemChecker();

        private MaterialItemChecker() { }

        public bool IsExist(MaterialItem material)
        {
            var inventory = SM.Instance<PlayerController>().ItemInventory;
            var quantity = inventory.GetItemQuantity(material.item);

            if (material.quantity > quantity)
                return false;

            return true;
        }
    }
}