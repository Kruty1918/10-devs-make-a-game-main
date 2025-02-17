using Bonjoura.Player;
using UnityEngine;

public class HungerUpgrade : MonoBehaviour
{
    [SerializeField] private float _hungerMultiplier = 0.5f;
    
    public void Upgrade()
    {
        PlayerController.Instance.PlayerHungerSystem.RecountHunger *= _hungerMultiplier;
    }
}
