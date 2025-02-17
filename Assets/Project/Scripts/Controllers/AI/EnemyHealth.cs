using System;
using Bonjoura.Player;
using Bonjoura.Services;
using SGS29.Utilities;
using UnityEngine;

namespace Bonjoura.Enemy
{
    public class EnemyHealth : Health
    {
        [SerializeField] private BaseResource _resource;

        [Header("SFX")]
        [SerializeField] private SFXPoolManager _sfxPoolManager;
        [SerializeField] private SfxSO _dieSound;
        private void Start()
        {
            _sfxPoolManager = GetComponentInChildren<SFXPoolManager>();
        }

        private void OnDie(string reason)
        {
            if (_dieSound != null) //You can just don`t assign the sound in Refs, so then it will not be played.
                _sfxPoolManager.GiveSFXSourceToTheObject(_dieSound, gameObject);//SoundEffect
            else
                Debug.Log("Sound, that you try to play is NULL. But its ok, if you don`t want to play sound for this mob :)");

            _sfxPoolManager.GiveSFXSourceToTheObject(_dieSound, gameObject);
            _resource.Get();
            Destroy(gameObject);
        }

        private void OnEnable()
        {
            OnDieEvent += OnDie;
        }

        private void OnDisable()
        {
            OnDieEvent -= OnDie;
        }
    }

    public static class PlayerMathf
    {
        /// <summary>
        /// Обчислює відстань від гравця до заданої точки.
        /// </summary>
        /// <param name="to">Координати цільової точки.</param>
        /// <returns>Відстань між гравцем і цільовою точкою.</returns>
        public static float DistanceFrom(Vector3 to)
        {
            return Vector3.Distance(GetPlayerPosition(), to);
        }

        /// <summary>
        /// Обчислює нормалізований вектор напряму від гравця до цільової точки.
        /// </summary>
        /// <param name="to">Координати цільової точки.</param>
        /// <returns>Нормалізований вектор напряму від гравця до цільової точки.</returns>
        public static Vector3 DirectionFrom(Vector3 to)
        {
            // Отримуємо орієнтацію гравця (forward) і нормалізуємо її
            Vector3 playerForward = GetPlayerForward();

            // Якщо вам потрібно враховувати лише рух вперед (без бічних відхилень),
            // використовуйте просто forward.
            return playerForward; // Це зробить моба рухатися тільки вперед від гравця
        }

        private static Vector3 GetPlayerPosition()
        {
            return SM.Instance<PlayerController>().transform.position;
        }

        private static Vector3 GetPlayerForward()
        {
            return SM.Instance<PlayerController>().transform.forward; // Напрямок вперед від гравця
        }
    }

}