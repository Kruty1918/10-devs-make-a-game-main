using System;
using Bonjoura.Services;
using UnityEngine;
using UnityEngine.Events;

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

}