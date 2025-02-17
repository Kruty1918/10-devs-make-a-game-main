using Bonjoura.UI;
using Bonjoura.Player;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using SGS29.Utilities;

namespace Bonjoura.UI.Inventory
{
    public sealed class InventoryUI : MonoBehaviour
    {
        [SerializeField] private QuickSlot[] quickSlots;
        [SerializeField] private BaseSlot[] baseSlots;
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private Transform arrowTransform;
        [SerializeField] private Image _itemSpriteInHand;
        [SerializeField] private MenuOpenClose _openClose;

        [SerializeField] private float _scrollDelay;
        private bool isDelay = false;

        private int _currentSelectQuickSlotIndex;
        private int _previousSelectedQuickSlotIndex;

        private BaseSlot _selectedSlot;

        private void Start()
        {
            SelectQuickSlot(0);
            for (int i = 0; i < baseSlots.Length; i++)
            {
                baseSlots[i].SetIndex(i);
            }
        }

        private void Update()
        {
            if (SM.Instance<InputManager>().CurrentControls == Controls.KeyboardAndMouse) ChangeQuickSlot();

            if (SM.Instance<InputManager>().ScrollAxis != Vector2.zero)
                ChangeSlotByScroll();

            MoveItem();
        }

        private void MoveItem()
        {
            if (!_openClose.IsOpened) return;

            if (_selectedSlot)
                _selectedSlot.GroupUI.transform.position = new Vector3(SM.Instance<InputManager>().GetMousePosition().x, SM.Instance<InputManager>().GetMousePosition().y, _selectedSlot.GroupUI.transform.position.z);

            if (!SM.Instance<InputManager>().Player.Attack.WasPressedThisFrame()) return;
            foreach (var baseSlot in baseSlots)
            {
                if (!baseSlot.IsMouseEnter) continue;

                if (!_selectedSlot)
                {
                    _selectedSlot = baseSlot;
                    return;
                }

                if (_selectedSlot)
                {
                    _selectedSlot.GroupUI.transform.localPosition = new Vector3(0, 0, _selectedSlot.GroupUI.transform.position.z);
                    BaseSlot.ReplaceItem(_selectedSlot, baseSlot);
                    PlayerController.Instance.ItemInventory.SwapSlots(_selectedSlot.SlotIndex, baseSlot.SlotIndex);
                    _selectedSlot = null;

                    PutInHandItem();

                    return;
                }
            }
        }


        private void ChangeSlotByScroll()
        {
            if (!isDelay)
            {
                isDelay = true;
                int axis = (int)Mathf.Sign(SM.Instance<InputManager>().ScrollAxis.y);
                _previousSelectedQuickSlotIndex = _currentSelectQuickSlotIndex;
                _currentSelectQuickSlotIndex -= axis;

                _currentSelectQuickSlotIndex = _currentSelectQuickSlotIndex < 0 ? quickSlots.Length - 1 : _currentSelectQuickSlotIndex;
                _currentSelectQuickSlotIndex = _currentSelectQuickSlotIndex >= quickSlots.Length ? 0 : _currentSelectQuickSlotIndex;

                quickSlots[_previousSelectedQuickSlotIndex].Deselect();
                SelectQuickSlot(_currentSelectQuickSlotIndex);
                arrowTransform.transform.position =
                    new Vector2(quickSlots[_currentSelectQuickSlotIndex].transform.position.x,
                        arrowTransform.transform.position.y);
                PutInHandItem();

                StartCoroutine(ChangeScrollDelay());
            }
        }

        IEnumerator ChangeScrollDelay()
        {
            yield return new WaitForSeconds(_scrollDelay);
            isDelay = false;
        }
        private void ChangeQuickSlot()
        {
            for (int i = 0; i < quickSlots.Length; i++)
            {
                if (!SM.Instance<InputManager>().NumberKeys[i + 1].wasPressedThisFrame) continue;
                if (i == _currentSelectQuickSlotIndex) return;

                quickSlots[_currentSelectQuickSlotIndex].Deselect();
                SelectQuickSlot(i);
                arrowTransform.transform.position =
                    new Vector2(quickSlots[i].transform.position.x,
                        arrowTransform.transform.position.y);

                PutInHandItem();

                return;
            }
        }

        private void SelectQuickSlot(int index)
        {
            quickSlots[index].SelectSlot();
            PlayerController.Instance.ItemInventory.SelectSlot(index);
            _currentSelectQuickSlotIndex = index;

            PutInHandItem();
        }

        private void DropItemFromQuickSlot(InputAction.CallbackContext obj)
        {
            QuickSlot quickSlot = quickSlots[_currentSelectQuickSlotIndex];
            if (quickSlot.ItemInSlot == null) return;
            PlayerController.Instance.ItemInventory.RemoveSelectedItemWithDropped();

            PutInHandItem();
        }

        private void RefreshSlots()
        {
            for (int i = 0; i < PlayerController.Instance.ItemInventory.InventorySlots.Count; i++)
            {
                var slot = PlayerController.Instance.ItemInventory.InventorySlots[i];
                var slotUI = baseSlots[i];

                if (slot.IsEmpty)
                {
                    baseSlots[i].ClearSlot();
                }
                else
                {
                    slotUI.SetItem(slot);
                    slotUI.UpdateValue();

                    PutInHandItem();
                }
            }
        }

        private void ReplaceSlots()
        {
            for (int i = 0; i < PlayerController.Instance.ItemInventory.InventorySlots.Count; i++)
            {
                var slot = PlayerController.Instance.ItemInventory.InventorySlots[i];
                var slotUI = baseSlots[i];

                if (slot.IsEmpty)
                {
                    baseSlots[i].ClearSlot();
                }
                else
                {
                    slotUI.SetItem(slot);
                    slotUI.UpdateValue();
                }
            }
        }

        public string ReturnItemName()
        {
            QuickSlot quickSlot = quickSlots[_currentSelectQuickSlotIndex];

            return quickSlot.ItemInSlot.item.name.ToString();
        }

        public QuickSlot ReturnSelectedItem()
        {
            QuickSlot quickSlot = quickSlots[_currentSelectQuickSlotIndex];

            return quickSlot;
        }

        public void PutInHandItem()
        {
            QuickSlot quickSlot = quickSlots[_currentSelectQuickSlotIndex];

            if (quickSlot.ItemInSlot != null)
            {
                if (quickSlot.ItemInSlot.item != null)
                    _itemSpriteInHand.sprite = quickSlot.ItemInSlot.item.ItemIcon;
                else
                    _itemSpriteInHand.color = new Color(1, 1, 1, 0); ;

                _itemSpriteInHand.SetNativeSize();

                Vector2 newSize = _itemSpriteInHand.sprite.bounds.size * 6;

                _itemSpriteInHand.rectTransform.sizeDelta = newSize;


                _itemSpriteInHand.color = new Color(1, 1, 1, 1);
            }
            else
            {
                _itemSpriteInHand.color = new Color(1, 1, 1, 0);
            }
        }

        public BaseInventoryItem GetItemFromSelectedQuickSlot()
        {
            QuickSlot selectedQuickSlot = quickSlots[_currentSelectQuickSlotIndex];
            if (selectedQuickSlot.ItemInSlot != null)
            {
                return selectedQuickSlot.ItemInSlot.item;
            }
            return null;
        }

        private void OnEnable()
        {
            PlayerController.Instance.ItemInventory.OnRefreshItemEvent += RefreshSlots;

            SM.Instance<InputManager>().Player.DropItem.started += DropItemFromQuickSlot;
        }

        private void OnDisable()
        {
            PlayerController.Instance.ItemInventory.OnRefreshItemEvent -= RefreshSlots;

            SM.Instance<InputManager>().Player.DropItem.started -= DropItemFromQuickSlot;
        }
    }
}