using Bonjoura.UI;
using PrimeTween;
using UnityEngine;

namespace Bonjoura.UI.Inventory
{
    public sealed class QuickSlot : BaseSlot
    {
        [SerializeField] private QuickSlotData data;

        private Quaternion _initEuler;
        private Vector3 _originalScale;

        protected override void Init()
        {
            _initEuler = transform.localRotation;
            _originalScale = transform.localScale;
        }

        public void SelectSlot()
        {
            AnimationSelect();
        }

        public void Deselect()
        {
            AnimationDeselect();
        }

        private void AnimationSelect()
        {
            Tween.LocalRotation(transform, data.RotateTo, data.DurationAnimation, data.Ease);
            Tween.Scale(transform, data.ScaleTo, data.DurationAnimation, data.Ease);
        }

        private void AnimationDeselect()
        {
            Tween.LocalRotation(transform, _initEuler, data.DurationAnimation, data.Ease);
            Tween.Scale(transform, _originalScale, data.DurationAnimation, data.Ease);
        }
    }
}