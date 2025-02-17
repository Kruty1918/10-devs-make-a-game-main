using Bonjoura.Craft;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bonjoura.UI.Craft
{
    public class CraftOptionUI : MonoBehaviour
    {
        [SerializeField] private List<MaterialItemUI> materialUI;
        [SerializeField] private TMP_Text craftName;
        [SerializeField] private Image craftIcon;

        private CraftItem craft;

        public void InitializateOption(CraftOption craftOption)
        {
            craft = new CraftItem(craftOption);
            craftIcon.sprite = craftOption.CraftIcon;
            craftName.text = craftOption.CraftName;
            for (int i = 0; i < craftOption.MaterialList.Count; i++)
            {
                materialUI[i].SetMaterialInformation(craftOption.MaterialList[i]);
            }
            for (int i = 0; i < materialUI.Count - craftOption.MaterialList.Count; i++)
            {
                materialUI[i + craftOption.MaterialList.Count].SetNull();
            }
        }

        public void OnButtonClick()
        {
            craft.ToCraft();
        }
    }
}