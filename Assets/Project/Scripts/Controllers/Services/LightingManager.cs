using System;
using UnityEngine;

namespace Bonjoura.Services
{
    public class LightingManager : MonoBehaviour
    {
        [SerializeField] private Light _directionalLight;
        [SerializeField] private LightingPreset _presetLight;

        [SerializeField, Range(0, 300)] private float _timeOfDay;
        private bool isItNight;

        [Header("Clock UI")]
        [SerializeField] private RectTransform clockBackground;

        public bool IsItNight { get => isItNight; set => isItNight = value; }

        private void Start()
        {
            _timeOfDay = 0;
        }

        public void Update()
        {
            if (_presetLight == null)
            {
                return;
            }

            _timeOfDay += Time.deltaTime;
            _timeOfDay %= 300;
            float timePercent = _timeOfDay / 300;

            UpdateLighting(timePercent);

            isItNight = _timeOfDay <= 90 || _timeOfDay >= 240 ? true : false;

            UpdateUI(timePercent);
        }

        private void UpdateUI(float timePercent)
        {
            clockBackground.eulerAngles = new Vector3(0, 0, -198 + 360 * timePercent);
        }

        private void UpdateLighting(float timePercent)
        {
            RenderSettings.ambientLight = _presetLight.AmbientColor.Evaluate(timePercent);
            RenderSettings.fogColor = _presetLight.FogColor.Evaluate(timePercent);

            if (_directionalLight != null)
            {
                _directionalLight.color = _presetLight.AmbientColor.Evaluate(timePercent);
                _directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360) - 90, 170, 0));
            }
        }

#if UNITY_EDITOR
        [Obsolete]
        private void OnValidate()
        {
            if (_directionalLight != null)
            {
                return;
            }

            if (RenderSettings.sun != null)
            {
                _directionalLight = RenderSettings.sun;
                return;
            }

            Light[] lights = FindObjectsOfType<Light>();

            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    _directionalLight = light;
                    return;
                }
            }
        }
#endif
    }
}