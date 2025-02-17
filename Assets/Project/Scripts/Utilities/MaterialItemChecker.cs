using Bonjoura.Player;
using Bonjoura.Craft;
using SGS29.Utilities;
using Bonjoura.UI;
using System;
using System.Collections.Generic;

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

    public interface IEnumCondition
    {
        bool Condition<T1, T2>(T1 single, T2 flags) where T1 : Enum where T2 : Enum;
    }

    public interface IFlagsToArrayConverter
    {
        int[] GetEnumValues<T>(T value) where T : Enum;
    }

    public class EnumFlagsToArrayConverter : IFlagsToArrayConverter
    {
        public int[] GetEnumValues<T>(T enumValue) where T : Enum
        {
            List<int> values = new List<int>();

            foreach (T value in Enum.GetValues(typeof(T)))
            {
                int intValue = Convert.ToInt32(value);
                if (intValue != 0 && (Convert.ToInt32(enumValue) & intValue) == intValue)
                {
                    values.Add(intValue);
                }
            }

            return values.ToArray();
        }
    }

    public class EnumConditions : IEnumCondition
    {
        private readonly IFlagsToArrayConverter converter;

        public EnumConditions(IFlagsToArrayConverter converter)
        {
            this.converter = converter;
        }

        public bool Condition<T1, T2>(T1 single, T2 flags) where T1 : Enum where T2 : Enum
        {
            int[] cond = converter.GetEnumValues(flags);
            int state = Convert.ToInt32(single);

            for (int i = 0; i < cond.Length; i++)
            {
                if (state == cond[i])
                {
                    return true;
                }
            }

            return false;
        }
    }
}