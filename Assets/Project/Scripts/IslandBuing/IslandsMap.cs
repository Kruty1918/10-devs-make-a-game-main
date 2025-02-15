using Bonjoura.Player;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IslandsMap : MonoBehaviour //сюди лізти тільки в хімзахисті, дуже небезпечний, токсичний гавнокод
{
    [SerializeField] private int cost;

    [SerializeField] private GameObject[] prefabs;

    [SerializeField] private TMP_Text _totalLandtext;
    [SerializeField] private TMP_Text _landCosttext;

    [SerializeField] private LayerMask islandLayer;

    private List<IslandPoints> totalLands = new List<IslandPoints>();
    [SerializeField] private List<IslandPoints> islands = new List<IslandPoints>();

    private void Start()
    {
        _totalLandtext.text = $"Buy New Land: {totalLands.Count}";
        _landCosttext.text = $"Cost: {cost}";
    }
    public void TrySpawnIsland()
    {
        if (PlayerController.Instance.GetExperienceScript().experience >= cost)
            SpawnRandomIsland();
        else
            Debug.Log("NotEnoughEXP");
    }


    private void SpawnRandomIsland()
    {
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

        IslandPoints islandSpawn = islands[Random.Range(0, islands.Count)];

        List<Transform> points = islandSpawn.points;

        int randInt = Random.Range(0, points.Count);
        Transform point = points[randInt];

        Vector2 vector2 = point.GetComponent<Pointer>().vector2;

        Vector3 spawnpos = new Vector3(point.position.x - vector2.x, 0, point.position.y - vector2.y);
        Quaternion rotation = new Quaternion(0, Random.Range(0, 380), 0, 0);


        if(Physics.Raycast(spawnpos + Vector3.up * 100, Vector3.down, out var k, 5000, islandLayer))
        {
            Destroy(islandSpawn.points[randInt].gameObject);
            islandSpawn.points.Remove(islandSpawn.points[randInt]);

            if (islandSpawn.points.Count <= 0)
                islands.Remove(islandSpawn);

            SpawnRandomIsland();
            return;
        }

        IslandPoints obj = Instantiate(prefab, spawnpos, rotation).GetComponent<IslandPoints>();

        obj.transform.rotation = rotation;

        if (randInt == 0)
            obj.points.Remove(obj.points[2]);
        else if (randInt == 2)
            obj.points.Remove(obj.points[0]);
        if (randInt == 1)
            obj.points.Remove(obj.points[3]);
        else if (randInt == 3)
            obj.points.Remove(obj.points[1]);

        islands.Add(obj);
        totalLands.Add(obj);

        Debug.Log($"Spawned {prefab.name} on {spawnpos} with rotation {rotation.y}");

        islandSpawn.points.Remove(point);

        if (islandSpawn.points.Count <= 0)
            islands.Remove(islandSpawn);

        PlayerController.Instance.GetExperienceScript().RemoveXP(cost);

        cost += 50;

        _totalLandtext.text = $"Buy New Land: {totalLands.Count}";
        _landCosttext.text = $"Cost: {cost}" ;
    }
}