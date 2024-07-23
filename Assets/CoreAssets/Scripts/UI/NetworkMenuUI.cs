using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkMenuUI : MonoBehaviour
{
    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;

    private void Awake( )
    {
        HostButton.onClick.AddListener( ( ) =>
        {
            NetworkManager.Singleton.StartHost( );
            Hide( );
            Debug.Log( "Joined HOST" );
        } );

        ClientButton.onClick.AddListener( ( ) =>
        {
            NetworkManager.Singleton.StartClient( );
            Hide( );
            Debug.Log( "Joined CLIENT" );
        } );
    }

    private void Hide( )
    {
        this.gameObject.SetActive( false );
    }
}
