using Bonjoura.Services;
using TMPro;
using UnityEngine;

namespace Bonjoura.UI
{
    public sealed class CollectDescription : DescriptionByRaycast
    {
        [SerializeField] private TMP_Text info;
        private DroppedItem _droppedItem;

        private void Awake()
        {
            _droppedItem = GetComponent<DroppedItem>();
        }

        protected override void ActiveDescription()
        {
            info.text = $"Collect <b>{_droppedItem.ItemToGet.ItemName}</b>";
        }
    }
}