namespace Bonjoura.Services
{
    public sealed class OutlineByRaycast : BaseRaycastLittleRaycastDetect
    {
        private Outline _outline;

        private void Awake()
        {
            _outline = GetComponent<Outline>();
            _outline.enabled = false;
        }

        protected override void OnIgnore()
        {
            _outline.enabled = false;
        }

        protected override void OnDetect()
        {
            _outline.enabled = true;
        }
    }
}