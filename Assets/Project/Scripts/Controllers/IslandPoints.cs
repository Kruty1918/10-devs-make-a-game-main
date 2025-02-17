using System;
using System.Collections.Generic;
using UnityEngine;

public class IslandPoints : MonoBehaviour
{
    [field:SerializeField] public List<Transform> points { get; private set; }
    public bool isSpawned;// { get; private set; }

    private void Start()
    {
        Invoke(nameof(SetSpawned), 0.05f);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("island"))
        {
            Debug.Log("island");
            if(other.GetComponent<IslandPoints>().isSpawned == true)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(other.gameObject);
                SetSpawned();
            }
        }
    }

    private void SetSpawned()
    {
        isSpawned = true;
    }
}
