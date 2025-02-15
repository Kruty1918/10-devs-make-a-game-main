using Bonjoura.Resource;
using Bonjoura.Services;
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
        private void OnDie()
        {
            
            if (_dieSound != null) //You can just don`t asign the sound in Refs, so then it will not be played.
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
}