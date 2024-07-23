using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] selectedCounterVisualArray;
    private void Start( )
    {
        //Player.Instance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged;
        HideSelectedCounterVisual( );
    }

    private void Instance_OnSelectedCounterChanged( object sender, Player.OnSelectedCounterChangedEventArgs e )
    {
        if ( e.selectedCounter == baseCounter )
        {
            ShowSelectedCounterVisual( );
        }
        else
        {
            HideSelectedCounterVisual( );
        }
    }

    private void ShowSelectedCounterVisual( )
    {
        foreach ( GameObject selectedCounterVisual in selectedCounterVisualArray )
        {
            selectedCounterVisual.SetActive( true );
        }
    }
    private void HideSelectedCounterVisual( )
    {
        foreach ( GameObject selectedCounterVisual in selectedCounterVisualArray )
        {
            selectedCounterVisual.SetActive( false );
        }
    }
}
