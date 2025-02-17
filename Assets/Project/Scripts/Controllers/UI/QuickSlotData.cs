using PrimeTween;
using UnityEngine;

namespace Bonjoura.Inventory
{
    [CreateAssetMenu(fileName = "QuickSlotData", menuName = "Scriptable Objects/QuickSlotData")]
    public sealed class QuickSlotData : ScriptableObject
    {
        [Header("Animation")] 
        [SerializeField] private float durationAnimation;
        [SerializeField] private Ease ease;
        [SerializeField] private Vector3 rotateTo;
        [SerializeField] private Vector3 scaleTo;
        
        public float DurationAnimation => durationAnimation;
        public Ease Ease => ease;
        public Vector3 RotateTo => rotateTo;
        public Vector3 ScaleTo => scaleTo;
    }
}

