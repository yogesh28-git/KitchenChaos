using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake( )
    {
        playButton.onClick.AddListener( ( ) =>
        {
            SceneLoader.Load( SceneLoader.Scenes.GameScene );
        } );

        quitButton.onClick.AddListener( ( ) =>
        {
            Application.Quit( );
        } );
    }

    private void Start( )
    {
        Time.timeScale = 1.0f; 
    }
}
