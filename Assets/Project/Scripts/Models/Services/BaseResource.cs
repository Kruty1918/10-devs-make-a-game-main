using Bonjoura.UI;
using Bonjoura.Player;
using PrimeTween;
using UnityEngine;
using SGS29.Utilities;

namespace Bonjoura.Services
{
    public sealed class BaseResource : MonoBehaviour
    {
        [SerializeField] private int maxStepsToGet;

        [Header("Item")]
        [SerializeField] private BaseInventoryItem itemToGet;
        [SerializeField] private GameObject droppedItemReference;
        [SerializeField] private int countGet;
        [SerializeField] private float spawnRadius;
        [SerializeField] private float spreadForce = 5f;
        private int exp = 100;

        [Header("Animation")]
        [SerializeField] private Transform transformToAnimation;
        [SerializeField] private float durationAnimation;
        [SerializeField] private Ease ease;
        [SerializeField] private Vector3 unscaling;

        private int _stepToGet;

        private Sequence _sequence;

        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = transformToAnimation.localScale;
        }

        private void Start()
        {
            exp = 100;
        }
        private void Getting()
        {
            if (PlayerController.Instance.InteractRaycast.CurrentDetectObject != gameObject) return;
            if (!SM.Instance<InputManager>().Player.Attack.WasPressedThisFrame()) return;

            _stepToGet++;
            Animation();
            if (_stepToGet < maxStepsToGet) return;
            for (int i = 0; i < countGet; i++)
            {
                Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
                randomOffset.y = 0.5f;

                GameObject droppedItemObject = Instantiate(droppedItemReference, transform.position + randomOffset, Quaternion.identity);
                DroppedItem droppedItem = droppedItemObject.GetComponent<DroppedItem>();

                Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));

                droppedItem.SetSprite(itemToGet.ItemIcon);
                droppedItem.SetSpriteScale(itemToGet.IconScale);
                droppedItem.Drop(randomDirection, spreadForce);
                droppedItem.SetItem(itemToGet);
            }
            PlayerController.Instance.GetExperienceScript().AddXP(exp);
            PlayerController.Instance.GetXPParticle().transform.position = transform.position;
            PlayerController.Instance.GetXPParticle().Play();
            Destroy(gameObject);
        }

        public void Get()
        {
            _stepToGet++;
            Animation();
            if (_stepToGet < maxStepsToGet) return;
            for (int i = 0; i < countGet; i++)
            {
                Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
                randomOffset.y = 0.5f;

                GameObject droppedItemObject = Instantiate(droppedItemReference, transform.position + randomOffset, Quaternion.identity);
                DroppedItem droppedItem = droppedItemObject.GetComponent<DroppedItem>();

                Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));

                droppedItem.SetSprite(itemToGet.ItemIcon);
                droppedItem.SetSpriteScale(itemToGet.IconScale);
                droppedItem.Drop(randomDirection, spreadForce);
                droppedItem.SetItem(itemToGet);
            }
        }

        private void Animation()
        {
            _sequence.Complete();
            _sequence = Sequence.Create();

            _sequence.Group(Tween.Scale(transformToAnimation, _originalScale - unscaling, durationAnimation, ease))
                .Chain(Tween.Scale(transformToAnimation, _originalScale, durationAnimation, ease));
        }

        private void OnEnable()
        {
            PlayerController.Instance.InteractRaycast.OnRaycastEvent += Getting;
        }

        private void OnDisable()
        {
            PlayerController.Instance.InteractRaycast.OnRaycastEvent -= Getting;
        }
    }
}
