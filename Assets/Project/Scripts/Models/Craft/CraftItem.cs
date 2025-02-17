using Bonjoura.Player;
using Bonjoura.Utilities;
using SGS29.Utilities;
using System.Collections.Generic;

namespace Bonjoura.Craft
{
    public class CraftItem
    {
        private readonly CraftOption _craftOption;

        public CraftItem(CraftOption craftOption)
        {
            _craftOption = craftOption;
        }

        public void ToCraft()
        {
            foreach (var material in _craftOption.MaterialList)
            {
                if (!MaterialItemChecker.IsExist(material)) return;
            }

            var inventory = SM.Instance<PlayerController>().ItemInventory;
            foreach (var material in _craftOption.MaterialList)
            {
                for (int i = 0; i < material.quantity; i++)
                {
                    inventory.RemoveItem(material.item);
                }
            }

            for (int i = 0; i < _craftOption.Quantity; i++)
            {
                inventory.AddItem(_craftOption.CraftedItem);
            }
        }
    }
}