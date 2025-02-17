using System;
using System.Collections.Generic;
using Bonjoura.UI;
using Bonjoura.Player;
using UnityEngine;
using SGS29.Utilities;

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
        if (SM.Instance<PlayerController>().InteractRaycast.CurrentDetectObject != gameObject) return;
        if (!SM.Instance<InputManager>().Player.Interact.WasPressedThisFrame()) return;
        CheckForSpawnMimic();

        GiveLoot(_loots);
        Destroy(gameObject);
    }

    private void GiveLoot(List<Loot> loots)
    {
        var inventory = SM.Instance<PlayerController>().ItemInventory;

        foreach (var loot in loots)
        {
            inventory.AddItem(loot.Item, loot.Quantity);
        }
    }

    private void OnEnable()
    {
        SM.Instance<PlayerController>().InteractRaycast.OnRaycastEvent += Getting;
    }

    private void OnDisable()
    {
        SM.Instance<PlayerController>().InteractRaycast.OnRaycastEvent -= Getting;
    }
}

[Serializable]
public struct Loot
{
    public BaseInventoryItem Item;
    public int Quantity;
}
