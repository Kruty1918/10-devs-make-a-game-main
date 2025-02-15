using UnityEngine;
using System.Collections;

public class SFXSource : MonoBehaviour
{
    [SerializeField]private AudioSource audioSource;
    public SFXPoolManager poolManager;
    private SfxSO _localSfxData;
    private bool _sfxStartToPlay;

    private void Awake(){
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_sfxStartToPlay)
        {
            if (poolManager == null)
            {
                if (_localSfxData.needToBeDelletedAfterOwnerIsDead)
                {
                    Destroy(gameObject);
                }
            }
        }
    }


    public void PlaySFX(SfxSO sfxData, SFXPoolManager pool){
        poolManager = pool;
        if (sfxData != null){
            _localSfxData = sfxData;
            Debug.Log(audioSource);
            audioSource.PlayOneShot(sfxData.audioclip);
            _sfxStartToPlay = true;
            StartCoroutine(ReturnToPoolAfterTime(sfxData.audioclip.length));

        }
        else
        {
            Debug.LogWarning("SFX DATA = NULL !");
        }
    }

    private IEnumerator ReturnToPoolAfterTime(float delay){
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
