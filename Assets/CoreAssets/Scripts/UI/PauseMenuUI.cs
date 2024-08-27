using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    [SerializeField] private OptionsUI optionsUI;

    private void Awake( )
    {
        optionsButton.onClick.AddListener( ( ) =>
        {
            Hide( );
            optionsUI.Show(Show );
        });
        resumeButton.onClick.AddListener( ( ) =>
        {
            KitchenGameManager.Instance.TogglePause( );
        } );
        mainMenuButton.onClick.AddListener( ( ) =>
        {
            SceneLoader.Load( SceneLoader.Scenes.MainMenuScene );
        } );
    }

    private void Start( )
    {
        KitchenGameManager.Instance.OnLocalPaused += KitchenGameManager_OnLocalPaused;
        KitchenGameManager.Instance.OnLocalUnpaused += KitchenGameManager_OnLocalUnpaused;
        Hide( );
    }

    private void KitchenGameManager_OnLocalUnpaused( object sender, System.EventArgs e )
    {
        Hide( );
    }

    private void KitchenGameManager_OnLocalPaused( object sender, System.EventArgs e )
    {
        Show( );
    }

    private void Show( )
    {
        resumeButton.Select( );
        gameObject.SetActive(true );
    }

    private void Hide( )
    {
        gameObject.SetActive( false );
    }

    private void OnDestroy( )
    {
        KitchenGameManager.Instance.OnLocalPaused -= KitchenGameManager_OnLocalPaused;
        KitchenGameManager.Instance.OnLocalUnpaused -= KitchenGameManager_OnLocalUnpaused;
    }
}
