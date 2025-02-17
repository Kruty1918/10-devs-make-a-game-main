using System;
using System.Collections.Generic;
using Bonjoura.UI;
using Bonjoura.Player;
using UnityEngine;

public class LootBox : MonoBehaviour
{
    [SerializeField] private List<Loot> _loots = new();
    [SerializeField] private GameObject _mimicPrefab;

    public int chanceForSpawnMimic = 20;
    private void CheckForSpawnMimic()
    {

        int randomValue = UnityEngine.Random.Range(1, 101);
        if (randomValue <= chanceForSpawnMimic)
        {
            Instantiate(_mimicPrefab, transform.position, Quaternion.identity, null);
            Destroy(gameObject);
        }
    }

    private void Getting()
    {
        if (PlayerController.Instance.InteractRaycast.CurrentDetectObject != gameObject) return;
        if (!InputManager.Instance.Player.Interact.WasPressedThisFrame()) return;
        CheckForSpawnMimic();

        GiveLoot(_loots);
        Destroy(gameObject);
    }

    private void GiveLoot(List<Loot> loots)
    {
        var inventory = PlayerController.Instance.ItemInventory;

        foreach (var loot in loots)
        {
            inventory.AddItem(loot.Item, loot.Quantity);
        }
    }

    private void OnEnable()
    {
        PlayerController.Instance.InteractRaycast.OnRaycastEvent += Getting;
    }

    private void OnDisable()
    {
        PlayerController.Instance.InteractRaycast.OnRaycastEvent -= Getting;
    }
}

[Serializable]
public struct Loot
{
    public BaseInventoryItem Item;
    public int Quantity;
}
