using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Bonjoura
{
    public static class SceneLoader
    {
        private static Canvas canvas;
        private static RectTransform panel;

        static SceneLoader()
        {
            canvas = CreateCanvas();
            panel = CreateFullScreenPanel();
            Object.DontDestroyOnLoad(canvas.gameObject); // Робимо canvas незнищуваним при зміні сцен
        }

        // Створення Canvas
        private static Canvas CreateCanvas()
        {
            GameObject canvasObject = new GameObject("Canvas");
            Canvas newCanvas = canvasObject.AddComponent<Canvas>();
            newCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
            return newCanvas;
        }

        // Створення панелі, яка буде покривати весь екран
        private static RectTransform CreateFullScreenPanel()
        {
            GameObject panelObject = new GameObject("Panel");
            panelObject.transform.SetParent(canvas.transform);
            RectTransform rectTransform = panelObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0f, 0f);
            rectTransform.anchorMax = new Vector2(1f, 1f);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            // Задаємо чорний колір панелі
            Image image = panelObject.AddComponent<Image>();
            image.color = new Color(0f, 0f, 0f, 0f); // Початково прозорий

            return rectTransform;
        }

        // Анімація входу (розтягування панелі)
        private static async Task AnimateInAsync(float timeIn)
        {
            panel.gameObject.SetActive(true); // Робимо панель видимою
            await FadeInAsync(timeIn); // Починаємо анімацію входу
        }

        // Анімація виходу (приховання панелі)
        private static async Task AnimateOutAsync(float timeOut)
        {
            await FadeOutAsync(timeOut); // Починаємо анімацію виходу
        }

        // Логіка для анімації Fade In
        private static async Task FadeInAsync(float timeIn)
        {
            float elapsedTime = 0f;
            Image image = panel.GetComponent<Image>();
            Color color = image.color;

            // Плавне збільшення альфа-каналу
            while (elapsedTime < timeIn)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / timeIn);
                image.color = new Color(color.r, color.g, color.b, alpha);
                await Task.Yield(); // Даємо можливість Unity оновити кадр
            }

            image.color = new Color(color.r, color.g, color.b, 1f); // Забезпечуємо, щоб альфа не залишалась менше 1
        }

        // Логіка для анімації Fade Out
        private static async Task FadeOutAsync(float timeOut)
        {
            float elapsedTime = 0f;
            Image image = panel.GetComponent<Image>();
            Color color = image.color;

            // Плавне зменшення альфа-каналу
            while (elapsedTime < timeOut)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / timeOut);
                image.color = new Color(color.r, color.g, color.b, alpha);
                await Task.Yield(); // Даємо можливість Unity оновити кадр
            }

            image.color = new Color(color.r, color.g, color.b, 0f); // Забезпечуємо, щоб альфа не залишалась більше 0
            panel.gameObject.SetActive(false); // Сховуємо панель після анімації
        }

        // Асинхронне завантаження сцени з анімацією
        public static async Task LoadSceneAsync(string sceneName, float timeIn = 0.5f, float timeOut = 0.8f)
        {
            // Почати анімацію входу
            await AnimateInAsync(timeIn);

            // Завантаження сцени
            await LoadSceneInternalAsync(sceneName, timeOut);
        }

        // Реальне асинхронне завантаження сцени
        private static async Task LoadSceneInternalAsync(string sceneName, float timeOut)
        {
            // Очікуємо завершення анімації входу
            await Task.Delay((int)(timeOut * 1000)); // Перетворюємо timeOut у мілісекунди для затримки

            // Завантаження сцени
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;

            // Ожидаємо завершення завантаження сцени
            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true; // Завершити завантаження сцени
                }
                await Task.Yield(); // Даємо можливість Unity оновити кадр
            }

            // Після завантаження, почати анімацію виходу
            await AnimateOutAsync(timeOut);
        }
    }
}
