using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Bonjoura.UI
{
    public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        // Задано кольори для дефолтного, виділеного та клікнутого станів
        [SerializeField] private Color defaultColor = new Color(0.5f, 0.5f, 0.5f);  // сірий
        [SerializeField] private Color highlightColor = Color.white;  // білий
        [SerializeField] private Color clickColor = new Color(0.4f, 0.4f, 0.4f);  // темніший сірий

        [SerializeField] private Graphic[] graphics;
        [SerializeField] private AudioClip highlightClip;
        [SerializeField] private AudioClip clickClip;
        [SerializeField] private AudioClip deselectClip;

        [SerializeField] private UnityEvent onClick;

        private AudioSource audioSource;

        private void Start()
        {
            // Отримуємо AudioSource компоненти для відтворення звуків
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Highlight();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Deselect();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Select();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Відтворюємо UnityEvent (OnClick)
            onClick.Invoke();
            PlayClickSound();
        }

        private void Select()
        {
            SetColor(clickColor);
            PlaySound(clickClip);
        }

        private void Deselect()
        {
            SetColor(defaultColor);
            PlaySound(deselectClip);
        }

        private void Highlight()
        {
            SetColor(highlightColor);
            PlaySound(highlightClip);
        }

        private void SetColor(Color color)
        {
            foreach (var graphic in graphics)
            {
                graphic.color = color;
            }
        }

        private void PlaySound(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }

        private void PlayClickSound()
        {
            audioSource.clip = clickClip;
            audioSource.Play();
        }
    }
}