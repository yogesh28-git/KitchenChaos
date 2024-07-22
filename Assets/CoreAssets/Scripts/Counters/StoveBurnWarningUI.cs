using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private void Start( )
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }
    private void StoveCounter_OnProgressChanged( object sender, IHasProgress.OnProgressChangedEventArgs e )
    {
        float warningThreshold = 0.5f;
        if(stoveCounter.IsFried( ) && e.progressNormalized >= warningThreshold )
        {
            ShowWarningUI( );
        }
        else
        {
            HideWarningUI( );
        }

        if ( e.progressNormalized >= 1f )
            HideWarningUI( );
    }

    private void ShowWarningUI( )
    {
        gameObject.SetActive( true );
    }
    private void HideWarningUI( )
    {
        gameObject.SetActive( false );
    }
}
