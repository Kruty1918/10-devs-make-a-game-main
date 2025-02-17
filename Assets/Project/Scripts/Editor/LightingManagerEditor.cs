using Bonjoura.Services;
using UnityEditor;

namespace Bonjoura.Editor
{
    [CustomEditor(typeof(LightingManager))]
    public class LightingManagerEditor : UnityEditor.Editor
    {
        private LightingManager _lightingManager;
        private bool drawDayNight;

        private void OnEnable()
        {
            _lightingManager = (LightingManager)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Прапорець для малювання UI дня і ночі
            drawDayNight = EditorGUILayout.Toggle("Draw Day & Night", drawDayNight);

            if (drawDayNight)
            {
                _lightingManager.Update();
                DrawDayNightUI();
            }
        }

        private void DrawDayNightUI()
        {
            // Логіка для малювання UI дня і ночі, якщо прапорець активований
            if (_lightingManager.IsItNight)
            {
                EditorGUILayout.LabelField("Night Time", EditorStyles.boldLabel);
                // Ви можете додати малювання UI або інші ефекти для ночі
            }
            else
            {
                EditorGUILayout.LabelField("Day Time", EditorStyles.boldLabel);
                // Ви можете додати малювання UI або інші ефекти для дня
            }
        }
    }
}