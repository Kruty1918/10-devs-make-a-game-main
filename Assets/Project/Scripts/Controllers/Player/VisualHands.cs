using UnityEngine;
using SGS29.Utilities;
using Bonjoura.UI.Inventory;

namespace Bonjoura.Player
{
    /// <summary>
    /// The class responsible for whether the hand is shown 
    /// </summary>
    public class VisualHands : MonoBehaviour
    {
        [SerializeField] private GameObject armObj;

        void Start()
        {
            // Subscribe to the event 
            SM.Instance<InventoryUI>().OnSlotChanged += SlotChanged;
        }

        private void SlotChanged(SlotData data)
        {
            // Logic 
            armObj.SetActive(!data.IsOccupied);
        }
    }
}