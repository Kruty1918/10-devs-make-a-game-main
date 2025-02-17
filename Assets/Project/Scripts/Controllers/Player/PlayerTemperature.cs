using Bonjoura.Services;
using System;
using TMPro;
using UnityEngine;

namespace Bonjoura.Player
{
    public class PlayerTemperature : MonoBehaviour
    {
        [Header("Temperature Settings")]
        [SerializeField] private float _temperature = 37f;
        [SerializeField] private int _damageFromCold = 2;

        [Tooltip("How often does cold should damage the player")]
        [SerializeField] private float _damageRatio = 1.5f;

        [SerializeField] private float _minTemperature = 20f;
        [SerializeField] private float _maxTemperature = 47f;
        [SerializeField] private float _raycastDistance = 1.5f;
        //[SerializeField] private float _warmUpDelay = 5f; never used

        [SerializeField] private ParticleSystem _freezeParticleSystem;
        [SerializeField] private LightingManager _lightingManager;

        [Header("UI Settings")]
        [SerializeField] private TextMeshProUGUI _temperatureText;
        [SerializeField] private Color _colorCold, _colorMild, _colorHot;

        private float _initDamageRatio;
        private bool _isInWater = false;
        private bool _isCloseToFireplace = false;
        [SerializeField] private float _coldRate = 0.25f;

        public bool IsCloseToFireplace { get => _isCloseToFireplace; set => _isCloseToFireplace = value; }
        public float Temperature => _temperature;

        private void Start()
        {
            _initDamageRatio = _damageRatio;
            _colorMild = _temperatureText.color;
        }
        private void Update()
        {
            CheckIfStandingOnWater();
            if (_isCloseToFireplace)
            {
                _temperature += _coldRate * RateMultiplier() * Time.deltaTime;
                _temperature = Mathf.Min(_temperature, _maxTemperature);
            }
            else
            {
                if (!_isInWater)
                {
                    if (_temperature > 33)
                    {
                        _freezeParticleSystem.Stop();
                    }
                }
                _temperature -= _coldRate * RateMultiplier() * Time.deltaTime;
                _temperature = Mathf.Max(_temperature, _minTemperature);
            }
            if (_temperature <= 30 || _temperature >= 44)
            {
                CheckForDamageFromTemperature();
            }
            else
                _damageRatio = _initDamageRatio;
            UpdateTemperatureUI();
        }

        private void CheckIfStandingOnWater()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance))
            {
                if (hit.collider.CompareTag("Water"))
                {
                    _isInWater = true;
                    return;
                }
            }
            _isInWater = false;
        }

        private void CheckForDamageFromTemperature()
        {
            if (_damageRatio >= 0)
            {
                _damageRatio -= Time.deltaTime;
                if (_temperature <= 33 && _temperature > 30)
                {
                    _freezeParticleSystem.Play();
                }
            }
            else
            {
                _damageRatio = _initDamageRatio;
                if (_temperature <= 30 || _temperature >= 44)
                {
                    GetComponent<Health>().Damage(_damageFromCold, _temperature <= 37 ? "cold" : "heat");
                }
            }
        }
        private float RateMultiplier()
        {
            if (_isInWater)
            {
                return !_lightingManager.IsItNight ? 3f : 3 * 1.5f;
            }
            else if (_isCloseToFireplace)
            {
                return !_lightingManager.IsItNight ? 4f : 4 / 1.5f;
            }
            return !_lightingManager.IsItNight ? 1 : 1.5f;
        }
        private void UpdateTemperatureUI()
        {
            if (_temperatureText != null)
            {
                _temperatureText.text = $"{Mathf.RoundToInt(_temperature)}°C";
                switch (_temperature)
                {
                    case >= 44:
                        _temperatureText.color = _colorHot;
                        break;
                    case <= 30:
                        _temperatureText.color = _colorCold;
                        break;
                    default:
                        _temperatureText.color = _colorMild;
                        break;
                }
            }
        }
    }
}
