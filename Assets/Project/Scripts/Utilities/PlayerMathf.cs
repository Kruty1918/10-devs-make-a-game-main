using Bonjoura.Player;
using SGS29.Utilities;
using UnityEngine;

namespace Bonjoura.Enemy
{
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