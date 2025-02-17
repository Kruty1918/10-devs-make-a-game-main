namespace Bonjoura.UI.Inventory
{
    /// <summary>
    /// A class that describes an inventory slot 
    /// </summary>
    public class SlotData
    {
        /// <summary>
        /// Whether the slot is occupied 
        /// </summary>
        public bool IsOccupied { get; private set; }

        public SlotData(bool isOccupied)
        {
            IsOccupied = isOccupied;
        }
    }
}