using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounterRef;
    [SerializeField] private AudioSource stoveAudioSource;
    [SerializeField] private AudioClip stoveSizzlingSound;

    private float warningDelayTimer;
    private bool playWarningSound;

    private void Start( )
    {
        stoveAudioSource.clip = stoveSizzlingSound;
        stoveCounterRef.OnStateChanged += StoveCounterRef_OnStateChanged;
        stoveCounterRef.OnProgressChanged += StoveCounterRef_OnProgressChanged;
    }

    private void StoveCounterRef_OnProgressChanged( object sender, IHasProgress.OnProgressChangedEventArgs e )
    {
        float warningThreshold = 0.5f;
        playWarningSound = stoveCounterRef.IsFried( ) && e.progressNormalized >= warningThreshold;

        if ( e.progressNormalized >= 1f )
            playWarningSound = false;
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

    private void Update( )
    {
        if ( playWarningSound )
        {
            warningDelayTimer -= Time.deltaTime;
            if(warningDelayTimer <= 0 )
            {
                float warningDelay = 0.2f;
                warningDelayTimer = warningDelay;
                SoundManager.Instance.PlayWarningSound( stoveCounterRef.transform.position);
            }
        }
    }
}
