using Bonjoura.Player;
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
                if (!MaterialItemChecker.Instance.IsExist(material)) return;
            }

            var inventory = PlayerController.Instance.ItemInventory;
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