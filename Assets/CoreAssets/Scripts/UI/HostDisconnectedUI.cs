using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnectedUI : MonoBehaviour
{
    [SerializeField] private Button playAgainButton;

    private void Start( )
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;

        playAgainButton.onClick.AddListener( ( ) =>
        {
            NetworkManager.Singleton.Shutdown( );
            SceneLoader.Load( SceneLoader.Scenes.MainMenuScene );
        } );
        Hide( );
    }

    private void NetworkManager_OnClientDisconnectCallback( ulong clientId )
    {
        if ( clientId == NetworkManager.ServerClientId )
        {
            Show( );
        }
    }

    private void Show( )
    {
        this.gameObject.SetActive( true );
    }

    private void Hide( )
    {
        this.gameObject.SetActive( false );
    }
}
