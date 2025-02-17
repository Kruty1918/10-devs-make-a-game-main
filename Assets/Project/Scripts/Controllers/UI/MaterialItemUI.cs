using Bonjoura.Craft;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bonjoura.UI.Craft
{
    public class MaterialItemUI : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private TMP_Text quantityText;

        private string quantityFormat;
        private MaterialItem materialItem;
        
        private void Awake()
        {
            quantityFormat = quantityText.text;
        }

        public void SetMaterialInformation(MaterialItem material)
        {
            materialItem = material;
            itemIcon.sprite = material.ItemIcon;
            quantityText.text = String.Format(quantityText.text, material.quantity);
            ApplyTextColor();
        }

        public void SetNull()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            ApplyTextColor();
        }

        private void ApplyTextColor()
        {
            if (MaterialItemChecker.Instance.IsExist(materialItem))
            {
                quantityText.color = Color.white;
            }
            else
            {
                quantityText.color = Color.red;
            }
        }
    }
}