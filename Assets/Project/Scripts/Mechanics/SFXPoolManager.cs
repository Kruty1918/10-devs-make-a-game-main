using UnityEngine;
using System.Collections.Generic;

public class SFXPoolManager : MonoBehaviour
{

    [SerializeField] private GameObject audioSourcePrefab;

    public void GiveSFXSourceToTheObject(SfxSO SFXData,GameObject objectThatNeedSFX)
    {

        var _sfxToGive = Instantiate(audioSourcePrefab, objectThatNeedSFX.transform.position, Quaternion.identity, null);
        SFXSource _thisSource = _sfxToGive.GetComponent<SFXSource>();
        _thisSource.poolManager = this;
        _thisSource.PlaySFX(SFXData, this);
         
    }

}
