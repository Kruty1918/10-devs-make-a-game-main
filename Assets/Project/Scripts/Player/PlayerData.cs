using UnityEngine;

namespace Bonjoura.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
    public sealed class PlayerData : ScriptableObject
    {
        [Header("Physics")]
        [SerializeField] private float gravityForce = -45.5f;
        
        [Header("Movement")]
        [SerializeField] private float speedMove = 8.5f;
        [SerializeField] private float deltaMove = 10;
        [SerializeField] private float flyDelta = 6.5f;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 5.5f;

        public float GravityForce => gravityForce;
        public float JumpForce => jumpForce;
        public float SpeedMove => speedMove;
        public float DeltaMove => deltaMove;
        public float FlyDelta => flyDelta;
    }
}

