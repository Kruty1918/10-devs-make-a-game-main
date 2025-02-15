using UnityEngine;

namespace Bonjoura.Services
{
    public class DescriptionByRaycast : BaseRaycastLittleRaycastDetect
    {
        [SerializeField] private GameObject descriptionObject;

        protected override void OnIgnore()
        {
            descriptionObject.SetActive(false);
        }

        protected override void OnDetect()
        {
            descriptionObject.SetActive(true);
            ActiveDescription();
        }

        protected virtual void ActiveDescription()
        {
            
        }
    }
}

