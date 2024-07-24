using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] selectedCounterVisualArray;
    private void Start( )
    {
        if ( Player.LocalInstance != null )
        {
            Player.LocalInstance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged;
        }
        else
        {
            Player.OnAnyPlayerJoin += Player_OnAnyPlayerJoin;
        }

        HideSelectedCounterVisual( );
    }

    private void Player_OnAnyPlayerJoin( object sender, System.EventArgs e )
    {
        if ( Player.LocalInstance != null )
        {
            Player.LocalInstance.OnSelectedCounterChanged -= Instance_OnSelectedCounterChanged;
            Player.LocalInstance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged;
        }
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
