using Bonjoura.UI.Inventory;
using SGS29.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Bonjoura.Player
{
    public class PlayerHungerSystem : MonoBehaviour
    {
        [SerializeField] private InventoryUI _inventoryUI;
        [SerializeField] private PlayerHealth _health;
        [SerializeField] private int _damage;
        [SerializeField] private Slider _hungerSlider;
        /*[SerializeField] private BaseInventoryItem _meatRawItem;
        [SerializeField] private BaseInventoryItem _meatCookedItem;*/
        [SerializeField] float _maxHunger, _recountHunger, _recountHungerTime/*, _repairHunger, _repairHungerFromCooked*/;
        [SerializeField] private float hungervalue;

        public float RecountHunger
        {
            get => _recountHunger;
            set => _recountHunger = value;
        }

        [SerializeField] private float _currentHunger;
        public float CurrentHunger => _currentHunger;

        private void Start()
        {
            _currentHunger = _maxHunger;

            _hungerSlider.value = _currentHunger;
            _hungerSlider.maxValue = _maxHunger;

            InvokeRepeating(nameof(RecalculateHunger), _recountHungerTime, _recountHungerTime);
        }

        private void Update()
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (_inventoryUI.ReturnSelectedItem().ItemInSlot == null)
                    return;
                var item = _inventoryUI.ReturnSelectedItem().ItemInSlot.item;

                if (item != null)
                {
                    if (item.RepairHunger > 0)
                    {
                        SM.Instance<PlayerController>().ItemInventory.RemoveItem(item);

                        _currentHunger += item.RepairHunger;

                        if (_currentHunger > _maxHunger)
                        {
                            _currentHunger = _maxHunger;
                        }

                        _hungerSlider.value = _currentHunger;
                        _inventoryUI.PutInHandItem();
                    }
                }

                /*if (item != null && item == _meatRawItem)
                {
                    SM.Instance<PlayerController>().ItemInventory.RemoveItem(item);

                    _currentHunger += _repairHunger;

                    if (_currentHunger > _maxHunger)
                    {
                        _currentHunger = _maxHunger;
                    }

                    _hungerSlider.value = _currentHunger;
                    _inventoryUI.PutInHandItem();
                } else if (item != null && item == _meatCookedItem)
                {
                    SM.Instance<PlayerController>().ItemInventory.RemoveItem(item);

                    _currentHunger += _repairHungerFromCooked;

                    if (_currentHunger > _maxHunger)
                    {
                        _currentHunger = _maxHunger;
                    }

                    _hungerSlider.value = _currentHunger;
                    _inventoryUI.PutInHandItem();
                }*/
            }
        }

        private void RecalculateHunger()
        {
            if (_currentHunger > 0)
            {
                _currentHunger -= _recountHunger + hungervalue;
                _hungerSlider.value = _currentHunger;
            }
            else
            {
                _health.Damage(_damage, "hunger");
            }
        }

        public void HungerSet(float _hungervalue)
        {
            hungervalue = _hungervalue;
        }
    }
}