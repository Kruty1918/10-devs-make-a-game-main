namespace Bonjoura.UI.Inventory
{
    public class SlotData
    {
        public bool IsOccupied { get; private set; }

        public SlotData(bool isOccupied)
        {
            IsOccupied = isOccupied;
        }
    }
}