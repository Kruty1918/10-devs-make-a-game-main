using UnityEngine;
using Bonjoura.Player;

namespace Player
{
    public class Heater : MonoBehaviour
    {
        private PlayerTemperature playerTemperature;
        private Fireplace fireplace;
        private void Start()
        {
            fireplace = GetComponentInParent<Fireplace>();
        }
        private void OnTriggerEnter(Collider other)
        {
            playerTemperature = other.GetComponent<PlayerTemperature>();
            if (fireplace.IsBurning)
            {
                playerTemperature.IsCloseToFireplace = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (fireplace.IsBurning && playerTemperature != null)
            {
                playerTemperature.IsCloseToFireplace = false;
            }
            playerTemperature = null;
        }
        private void OnTriggerStay(Collider other)
        {
            if (!fireplace.IsBurning && playerTemperature != null)
            {
                playerTemperature.IsCloseToFireplace = false;
            }
            else if (fireplace.IsBurning && playerTemperature != null)
            {
                playerTemperature.IsCloseToFireplace = true;
            }
        }
    }
}