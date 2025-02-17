using UnityEngine;

public class BodyBlowing : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    // Метод, що буде викликаний при події 'BlowUp'
    public void BlowUp()
    {
        var go = Instantiate(prefab);
        Destroy(go, 5);
        Destroy(gameObject);
    }
}
