using UnityEngine;

namespace Bonjoura
{
    public enum GameState
    {
        Played = 1,
        Paused = 2,
        Dead = 3
    }

    [System.Flags]
    public enum GameStateGroup
    {
        None = 0,
        Played = 1 << 0,
        Paused = 2 << 1,
        Dead = 3 << 3,
    }
}