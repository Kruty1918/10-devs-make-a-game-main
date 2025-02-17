using Bonjoura.Player;
using SGS29.Utilities;
using UnityEngine;

namespace Bonjoura.Services
{
    public abstract class BaseRaycastLittleRaycastDetect : MonoBehaviour
    {
        protected abstract void OnIgnore();
        protected abstract void OnDetect();

        private void RaycastDetect()
        {
            if (SM.Instance<PlayerController>().InteractRaycast.CurrentDetectObject != gameObject)
            {
                OnIgnore();
                return;
            }
            OnDetect();
        }

        private void OnEnable()
        {
            SM.Instance<PlayerController>().InteractRaycast.OnRaycastEvent += RaycastDetect;
        }

        private void OnDisable()
        {
            if (SM.HasSingleton<PlayerController>())
                SM.Instance<PlayerController>().InteractRaycast.OnRaycastEvent -= RaycastDetect;
        }
    }
}