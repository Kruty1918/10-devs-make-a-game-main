using Bonjoura.Player;
using Bonjoura.Craft;
using SGS29.Utilities;
using Bonjoura.UI;

namespace Bonjoura.Utilities
{
    public static class MaterialItemChecker
    {
        public static bool IsExist(MaterialItem material)
        {
            ItemInventory inventory = SM.Instance<PlayerController>().ItemInventory;
            int quantity = inventory.GetItemQuantity(material.item);

            if (material.quantity > quantity)
                return false;

            return true;
        }
    }
}