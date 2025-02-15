using Bonjoura.Inventory;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bonjoura.UI.Inventory
{
    public abstract class BaseSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("References")] 
        [SerializeField] private GameObject groupUI;
        [SerializeField] private Image itemIcon;
        [SerializeField] private TMP_Text itemCountText;

        private bool _isMouseEnter;
        
        private InventorySlot _itemInSlot;

        private int _slotIndex;

        public bool IsMouseEnter => _isMouseEnter;
        public GameObject GroupUI => groupUI;

        public InventorySlot ItemInSlot => _itemInSlot;
        public int SlotIndex => _slotIndex;
        
        private void Awake()
        {
            groupUI.SetActive(false);
            Init();
        }

        private void OnEnable()
        {
            if (_itemInSlot != null) groupUI.SetActive(true);
        }

        protected abstract void Init();

        public void SetIndex(int index) => _slotIndex = index;
        
        public void ClearSlot()
        {
            _itemInSlot = null;
            groupUI.SetActive(false);
        }

        public void UpdateValue()
        {
            if (_itemInSlot == null) return;
            itemCountText.text = $"{_itemInSlot.quantity}";
        }

        public void SetItem(InventorySlot slot)
        {
            groupUI.SetActive(true);
            itemIcon.sprite = slot.item.ItemIcon;
            if (slot.item.CanStacking)
                itemCountText.gameObject.SetActive(true);
            else itemCountText.gameObject.SetActive(false);
            _itemInSlot = slot;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isMouseEnter = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isMouseEnter = false;
        }

        public static void ReplaceItem(BaseSlot fromSlot, BaseSlot toSlot)
        {
            InventorySlot item1 = fromSlot.ItemInSlot;
            InventorySlot item2 = toSlot.ItemInSlot;
            

            if (item1 != null)
            {
                toSlot.SetItem(item1);
                toSlot.UpdateValue();
            }
            else
            {
                toSlot.ClearSlot();
            }
            
            if (item2 != null)
            {
                fromSlot.SetItem(item2);
                fromSlot.UpdateValue();
            }
            else
            {
                fromSlot.ClearSlot();
            }
        }
    }
}