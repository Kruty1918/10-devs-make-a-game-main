using UnityEngine;

//This is a sound effect SO. You need to adjust time, that takes to play your sound, and the audio clip itself <---------

[CreateAssetMenu(fileName = "NewSFX", menuName = "SFX Manager/New SFX SO")]
public class SfxSO : ScriptableObject
{
    public AudioClip audioclip;
    public float timeToPlay;
    public bool needToBeDelletedAfterOwnerIsDead;
}
