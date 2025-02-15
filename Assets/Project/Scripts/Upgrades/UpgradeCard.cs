using System;
using System.Collections.Generic;
using Bonjoura.Inventory;
using Bonjoura.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour
{
    [SerializeField] private bool _unlockOnStart = false;
    [SerializeField] private List<UpgradeCard> _toUnlock;
    [Header("Cost")]
    [SerializeField] private BaseInventoryItem _needItem;
    [SerializeField] private int _neededQuantity;
    [SerializeField] private int _needXp;

    [Header("UI Elements")]
    [SerializeField] private GameObject _purchasedPanel;
    [SerializeField] private GameObject _lockPanel;
    [SerializeField] private GameObject _needPanel;
    [SerializeField] private Image _heedItemImage;
    [SerializeField] private TMP_Text _needQuantityText;
    [SerializeField] private TMP_Text _needExpText;
    [Space]
    public UnityEvent OnUpgrade;
    
    public bool IsUnlocked { get; private set; }
    public bool IsPurchased { get; private set; }

    private void Start()
    {
        if (_unlockOnStart)
        {
            Unlock();
        }

        _heedItemImage.sprite = _needItem.ItemIcon;
        _needQuantityText.text = _neededQuantity.ToString();
        _needExpText.text = _needXp.ToString();
        UpdateUI();
    }

    public void Upgrade()
    {
        if(!IsUnlocked || IsPurchased) return;
        
        if(TrySpendNeededItems() == false) return;
        
        OnUpgrade?.Invoke();
        IsPurchased = true;
        UpdateUI();
        UnlockNextUpgrades();
    }

    private void Unlock()
    {
        IsUnlocked = true;
        UpdateUI();
    }

    private void UnlockNextUpgrades()
    {
        if(_toUnlock == null || _toUnlock.Count == 0)
            return;
        
        foreach (var upgradeCard in _toUnlock)
        {
            if(upgradeCard != null)
                upgradeCard.Unlock();
        }
    }

    private bool TrySpendNeededItems()
    {
        var inventory = PlayerController.Instance.ItemInventory;
        int xp = PlayerController.Instance.GetExperienceScript().experience;
        int quantity = inventory.GetItemQuantity(_needItem);
        Debug.Log(quantity);
        bool canRemove = quantity >= _neededQuantity;

        if (!canRemove) return canRemove;//check resource

        canRemove = xp >= _needXp;//check xp

        if (canRemove)
        {
            for (int i = 0; i < _neededQuantity; i++)
            {
                inventory.RemoveItem(_needItem);
            }
            PlayerController.Instance.GetExperienceScript().RemoveXP(_needXp);
        }
        return canRemove;
    }

    private void UpdateUI()
    {
        _lockPanel.SetActive(!IsUnlocked);
        _needPanel.SetActive(!IsPurchased);
        _purchasedPanel.SetActive(IsPurchased);
    }
}
