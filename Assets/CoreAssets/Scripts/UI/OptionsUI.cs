using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI soundText;
    [SerializeField] private TextMeshProUGUI musicText;


    private void Awake( )
    {
        soundEffectsButton.onClick.AddListener( ( ) =>
        {
            SoundManager.Instance.ChangeSoundEffectsVolume( );
            UpdateVisual( );
        } );

        musicButton.onClick.AddListener( ( ) =>
        {
            SoundManager.Instance.ChangeMusicVolume( );
            UpdateVisual( );
        } );

        closeButton.onClick.AddListener( ( ) =>
        {
            Hide( );
        } );
    }
    private void Start( )
    {
        UpdateVisual( );
        Hide( );

        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
    }

    private void KitchenGameManager_OnGameUnpaused( object sender, System.EventArgs e )
    {
        Hide( );
    }

    private void UpdateVisual( )
    {
        int soundEffectsVolume =  (int)(SoundManager.Instance.GetSFXVolume( ) * 10); //Multiply 10 to convert to whole number.
        soundText.text = "SFX Volume: " + soundEffectsVolume; 

        int musicVolume = (int) ( SoundManager.Instance.GetMusicVolume( ) * 10 ); //Multiply 10 to convert to whole number.
        musicText.text = "Music: " + musicVolume;
    }
    
    public void Show( )
    {
        gameObject.SetActive( true );
    }

    public void Hide( )
    {
        gameObject.SetActive ( false );
    }

    private void OnDestroy( )
    {
        KitchenGameManager.Instance.OnGameUnpaused -= KitchenGameManager_OnGameUnpaused;
    }
}
