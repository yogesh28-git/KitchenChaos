using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerPauseUI : MonoBehaviour
{
    private void Start( )
    {
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        Hide( );
    }
    private void OnDestroy( )
    {
        KitchenGameManager.Instance.OnGamePaused -= KitchenGameManager_OnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused -= KitchenGameManager_OnGameUnpaused;
    }

    private void KitchenGameManager_OnGameUnpaused( object sender, System.EventArgs e )
    {
        Hide( );
    }

    private void KitchenGameManager_OnGamePaused( object sender, System.EventArgs e )
    {
        Show( );
    }

    private void Show( )
    {
        this.gameObject.SetActive( true );
    }
    private void Hide( )
    {
        this .gameObject.SetActive( false );
    }
}
