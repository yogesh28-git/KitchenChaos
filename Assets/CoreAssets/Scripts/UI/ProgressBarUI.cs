using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private GameObject hasProgressCounter;

    private IHasProgress hasProgress;

    private void Start( )
    {
        hasProgress = hasProgressCounter.GetComponent<IHasProgress>();
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        HideProgressBar( );
    }

    private void HasProgress_OnProgressChanged( object sender, IHasProgress.OnProgressChangedEventArgs e )
    {
        progressBar.fillAmount = e.progressNormalized;

        if ( progressBar.fillAmount == 0 || progressBar.fillAmount >= 1 )
        {
            HideProgressBar( );
        }
        else
        {
            ShowProgressBar( );
        }
    }

    private void ShowProgressBar( )
    {
        gameObject.SetActive( true );
    }
    private void HideProgressBar( )
    {
        gameObject.SetActive( false );
    }
}
