using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounterRef;
    [SerializeField] private AudioSource stoveAudioSource;
    [SerializeField] private AudioClip stoveSizzlingSound;

    private void Start( )
    {
        stoveAudioSource.clip = stoveSizzlingSound;
        stoveCounterRef.OnStateChanged += StoveCounterRef_OnStateChanged;
    }

    private void StoveCounterRef_OnStateChanged( object sender, StoveCounter.OnStateChangedEventArgs e )
    {
        if(e.stoveState == StoveCounter.StoveState.Frying || e.stoveState == StoveCounter.StoveState.Fried)
        {
            stoveAudioSource.Play( );
        }
        else
        {
            stoveAudioSource.Pause( );
        }
    }
}
