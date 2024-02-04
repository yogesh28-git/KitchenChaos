using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject stoveParticleEffect;
    [SerializeField] private GameObject stoveONVisual;
    [SerializeField] private StoveCounter stoveCounter;

    private void Start( )
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged( object sender, StoveCounter.OnStateChangedEventArgs e )
    {
        if(e.stoveState == StoveCounter.StoveState.Frying || e.stoveState == StoveCounter.StoveState.Fried )
        {
            ShowStoveVisualEffects( );
        }
        else
        {
            HideStoveVisualEffects();
        }
    }

    private void ShowStoveVisualEffects( )
    {
        stoveParticleEffect.SetActive( true );
        stoveONVisual.SetActive( true );
    }
    private void HideStoveVisualEffects( )
    {
        stoveParticleEffect.SetActive( false );
        stoveONVisual.SetActive( false );
    }
}
