using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioReferenceSO", menuName = "ScriptableObjects/AudioReferenceSO")]
public class AudioReferencesSO : ScriptableObject
{
    public AudioClip[] chop;
    public AudioClip[] deliveryFail;
    public AudioClip[] deliverySuccess;
    public AudioClip[] footStep;
    public AudioClip[] foodDrop;
    public AudioClip[] foodPickup;
    public AudioClip[] trash;
    public AudioClip[] warning;
}
