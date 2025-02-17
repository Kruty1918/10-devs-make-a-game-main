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
            OnDescriptionActive += ActiveDescription;
        }

        private void ActiveDescription(bool active)
        {
            if (active) DrawText();
            else CleatText();
        }

        private void DrawText()
        {
            info.text = $"Collect <b>{_droppedItem.ItemToGet.ItemName}</b>";
        }

        private void CleatText()
        {
            info.text = string.Empty;
        }
    }
}