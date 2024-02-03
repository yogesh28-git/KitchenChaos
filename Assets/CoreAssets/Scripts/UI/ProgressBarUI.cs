using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private CuttingCounter cuttingCounter;

    private void Start( )
    {
        cuttingCounter.OnCutProgressed += CuttingCounter_OnCutProgressed;

        HideProgressBar( );
    }

    private void CuttingCounter_OnCutProgressed( object sender, CuttingCounter.OnCutProgressedEventArgs e )
    {
        progressBar.fillAmount = e.cuttingProgressNormalized;

        if(progressBar.fillAmount == 0 ||  progressBar.fillAmount >= 1 )
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
