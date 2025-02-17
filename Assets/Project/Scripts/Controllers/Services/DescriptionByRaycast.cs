using System;
using UnityEngine;

namespace Bonjoura.Services
{
    public class DescriptionByRaycast : BaseRaycastLittleRaycastDetect
    {
        [SerializeField] private GameObject descriptionObject;
        public event Action<bool> OnDescriptionActive;

        protected override void OnIgnore() => DescriptionSetActive(false);
        protected override void OnDetect() => DescriptionSetActive(true);

        private void DescriptionSetActive(bool active)
        {
            descriptionObject.SetActive(active);
            OnDescriptionActive.Invoke(active);
        }
    }
}