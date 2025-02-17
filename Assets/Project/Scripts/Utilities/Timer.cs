using UnityEngine;

namespace Bonjoura.Utilities
{
    public static class Timer
    {
        public static bool SimpleTimer(float timeDelay, float time)
        {
            return !(Time.time - timeDelay < time);
        }
    }
}