using Bonjoura.UI;
using Bonjoura.UI.Inventory;
using System.Collections;
using UnityEngine;

namespace Bonjoura.Player
{
    public class Fireplace : MonoBehaviour
    {
        [SerializeField] private BaseInventoryItem itemToGet;
        [SerializeField] private InventoryUI _inventoryUI;
        [SerializeField] private SpriteRenderer _cookingItemSpriteRenderer;
        [SerializeField] private GameObject _droppedItemReference;
        private BaseInventoryItem _rawItem;
        private BaseInventoryItem _cookedItem;
        private bool _isCooking;

        public float TimeToCook;

        [Header("Burning")]
        private bool _isBurning = true;
        [SerializeField] private float _maxBurningTime;
        [SerializeField] private float _burningTime;
        [SerializeField] private ParticleSystem fireEffect;
        [SerializeField] private Light _light;

        public bool HasFood => _rawItem != null;

        public bool IsBurning => _isBurning;

        private void Start()
        {
            _burningTime = _maxBurningTime;
        }

        private void Update()
        {
            if (_burningTime > 0)
            {
                _isBurning = true;
                _burningTime -= Time.deltaTime;
            }
            else
            {
                _isBurning = false;
            }

            if (_isBurning)
            {
                if (!fireEffect.isPlaying)
                {
                    fireEffect.Play();
                }
                if (!_light.enabled)
                {
                    _light.enabled = true;
                }
            }
            else
            {
                if (fireEffect.isPlaying)
                {
                    fireEffect.Stop();
                }
                if (_light.enabled)
                {
                    _light.enabled = false;
                }
            }
        }
        private void Using()
        {
            if (PlayerController.Instance.InteractRaycast.CurrentDetectObject != gameObject) return;
            if (!InputManager.Instance.Player.Interact.WasPressedThisFrame()) return;


            if (_isCooking) return;

            if (HasFood) TryTakeCookedFood();
            else if (_isBurning) TryPlaceFood();
            TryPlaceFuel();
        }

        private void TryPlaceFuel()
        {
            if (_burningTime >= _maxBurningTime - 5) return;
            BaseInventoryItem placedFood = _inventoryUI.ReturnSelectedItem().ItemInSlot?.item;

            if (placedFood == null || !placedFood.OtherTypes.Contains(OtherType.Oak)) return;

            //print($"You put {placedFood.ItemName} to fuel up the fireplace");
            PlayerController.Instance.ItemInventory.RemoveItem(placedFood);
            _burningTime += 5;
            _burningTime = Mathf.Min(_burningTime, _maxBurningTime);
            PlaceItemOnFire();
        }

        private void TryPlaceFood()
        {

            if (_isCooking || HasFood) return;
            BaseInventoryItem placedFood = _inventoryUI.ReturnSelectedItem().ItemInSlot?.item;

            if (placedFood == null || !placedFood.OtherTypes.Contains(OtherType.Raw)) return;

            //print($"You put {placedFood.ItemName} to cook in the fireplace");
            _rawItem = placedFood;
            _cookedItem = placedFood.GetCookedVersion();
            StartCoroutine(CookingFood());
        }

        private void TryTakeCookedFood()
        {
            if (!_isCooking && _cookedItem != null)
            {
                DropItemFromFire();
                _rawItem = null;
                _cookedItem = null;
            }
        }

        IEnumerator CookingFood()
        {
            _isCooking = true;
            PlaceItemOnFire();
            yield return new WaitForSeconds(TimeToCook);
            if (_rawItem.DropWhenCoocked != null)
            {
                _cookingItemSpriteRenderer.sprite = _rawItem.DropWhenCoocked.ItemIcon;
            }
            else
            {
                _cookingItemSpriteRenderer.sprite = itemToGet.ItemIcon;
            }

            _isCooking = false;
        }

        void PlaceItemOnFire()
        {
            PlayerController.Instance.ItemInventory.RemoveItem(_rawItem);
            if (_cookedItem != null)
                _cookingItemSpriteRenderer.sprite = _cookedItem.ItemIcon;
        }

        void DropItemFromFire()
        {
            Vector3 randomOffset = Random.insideUnitSphere;
            randomOffset.y = 0.5f;
            if (_rawItem.DropWhenCoocked != null)
            {
                GameObject droppedItemObject = Instantiate(_droppedItemReference, transform.position + randomOffset, Quaternion.identity);

                DroppedItem droppedItem = droppedItemObject.GetComponent<DroppedItem>();
                Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
                droppedItem.Drop(randomDirection, 2);

                droppedItem.SetSprite(_rawItem.DropWhenCoocked.ItemIcon);
                droppedItem.SetSpriteScale(_rawItem.DropWhenCoocked.IconScale);
                droppedItem.Drop(randomDirection, 2);
                droppedItem.SetItem(_rawItem.DropWhenCoocked);
            }
            else
            {
                GameObject droppedItemObject = Instantiate(_droppedItemReference, transform.position + randomOffset, Quaternion.identity);

                DroppedItem droppedItem = droppedItemObject.GetComponent<DroppedItem>();
                Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
                droppedItem.Drop(randomDirection, 2);

                droppedItem.SetSprite(itemToGet.ItemIcon);
                droppedItem.SetSpriteScale(itemToGet.IconScale);
                droppedItem.Drop(randomDirection, 2);
                droppedItem.SetItem(itemToGet);
            }

            _cookingItemSpriteRenderer.sprite = null;
        }


        private void OnEnable()
        {
            PlayerController.Instance.InteractRaycast.OnRaycastEvent += Using;
        }

        private void OnDisable()
        {
            PlayerController.Instance.InteractRaycast.OnRaycastEvent -= Using;
        }
    }
}