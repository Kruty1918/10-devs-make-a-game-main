using Bonjoura.UI;
using Bonjoura.Player;
using Bonjoura.UI.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
                    PlayerController.Instance.ItemInventory.RemoveItem(item);

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
                PlayerController.Instance.ItemInventory.RemoveItem(item);

                _currentHunger += _repairHunger;

                if (_currentHunger > _maxHunger)
                {
                    _currentHunger = _maxHunger;
                }

                _hungerSlider.value = _currentHunger;
                _inventoryUI.PutInHandItem();
            } else if (item != null && item == _meatCookedItem)
            {
                PlayerController.Instance.ItemInventory.RemoveItem(item);

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
