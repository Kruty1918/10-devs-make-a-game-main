using Bonjoura.Player;
using UnityEngine;

namespace Bonjoura.Services
{
    public abstract class BaseRaycastLittleRaycastDetect : MonoBehaviour
    {
        protected abstract void OnIgnore();
        protected abstract void OnDetect();
        
        private void RaycastDetect()
        {
            if (PlayerController.Instance.InteractRaycast.CurrentDetectObject != gameObject)
            {
                OnIgnore();
                return;
            }
            OnDetect();
        }

        private void OnEnable()
        {
            PlayerController.Instance.InteractRaycast.OnRaycastEvent += RaycastDetect;
        }
        
        private void OnDisable()
        {
            PlayerController.Instance.InteractRaycast.OnRaycastEvent -= RaycastDetect;
        }
    }
}