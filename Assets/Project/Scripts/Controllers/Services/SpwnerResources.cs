using UnityEngine;

public class SpwnerResources : MonoBehaviour
{
    [SerializeField]
    GameObject SpawnGO;
    GameObject spawnedGameObject;
    float timer;
    float spawnTime;
    bool isSetTime = false;

    void Start()
    {
        spawnedGameObject = Instantiate(SpawnGO, transform); //idk wtf it is, but i think HP sliders on the pig don't work bcs of this shit (Not me writed this code)
    }

    
    void Update()
    {
        if(spawnedGameObject == null)
        {
            if (isSetTime)
            {
                timer += Time.deltaTime;
                if(timer >= spawnTime)
                {
                    spawnedGameObject = Instantiate(SpawnGO, transform);
                    isSetTime = false;
                }
            }
            else
            {
                timer = 0;
                spawnTime = Random.Range(30, 60);
                isSetTime = true;
            }
        }
    }
}
