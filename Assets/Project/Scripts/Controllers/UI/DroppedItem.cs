using Bonjoura.Player;
using SGS29.Utilities;
using UnityEngine;

namespace Bonjoura.UI
{
    public sealed class DroppedItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Rigidbody _rigidbody;
        private BaseInventoryItem _itemToGet;

        public BaseInventoryItem ItemToGet => _itemToGet;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;
        public void SetSpriteScale(Vector3 scale) => spriteRenderer.transform.localScale = scale;
        public void SetItem(BaseInventoryItem newItem) => _itemToGet = newItem;
        public void Drop(Vector3 forward, float force = 0) => _rigidbody.AddForce(forward * force, ForceMode.Impulse);

        private void Getting()
        {
            if (SM.Instance<PlayerController>().InteractRaycast.CurrentDetectObject != gameObject) return;
            if (!SM.Instance<InputManager>().Player.Interact.WasPressedThisFrame()) return;
            if (!SM.Instance<PlayerController>().ItemInventory.AddItem(_itemToGet)) return;
            Destroy(gameObject);
        }

        private void OnEnable()
        {
            SM.Instance<PlayerController>().InteractRaycast.OnRaycastEvent += Getting;
        }

        private void OnDisable()
        {
            if (SM.HasSingleton<PlayerController>())
                SM.Instance<PlayerController>().InteractRaycast.OnRaycastEvent -= Getting;
        }
    }
}