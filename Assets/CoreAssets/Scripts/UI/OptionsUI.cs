using System;
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

    [Space(10)]
    [Header( "Key Bindings" )]
    [SerializeField] private AutoSizeButton moveUp;
    [SerializeField] private AutoSizeButton moveLeft;
    [SerializeField] private AutoSizeButton moveDown;
    [SerializeField] private AutoSizeButton moveRight;
    [SerializeField] private AutoSizeButton interact;
    [SerializeField] private AutoSizeButton interactAlt;
    [SerializeField] private AutoSizeButton pause;
    [Space( 5 )]
    [SerializeField] private AutoSizeButton gamepadInteract;
    [SerializeField] private AutoSizeButton gamepadAlt;
    [SerializeField] private AutoSizeButton gamepadPause;
    [Space( 5 )]
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [Space( 5 )]
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadAltText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;
    [Space( 5 )]
    [SerializeField] private Transform keyRebindVisual;

    private Action onOptionsHide;

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
        moveUp.AddListener( ( ) => { RebindBinding( GameInput.Binding.Move_Up); } );
        moveLeft.AddListener( ( ) => { RebindBinding( GameInput.Binding.Move_Left ); } );
        moveDown.AddListener( ( ) => { RebindBinding( GameInput.Binding.Move_Down ); } );
        moveRight.AddListener( ( ) => { RebindBinding( GameInput.Binding.Move_Right ); } );
        interact.AddListener( ( ) => { RebindBinding( GameInput.Binding.Interact ); } );
        interactAlt.AddListener( ( ) => { RebindBinding( GameInput.Binding.Interact_Alt ); } );
        pause.AddListener( ( ) => { RebindBinding( GameInput.Binding.Pause ); } );

        UpdateVisual( );
        Hide( );
        HideKeyRebindVisual( );

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

        moveUpText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Move_Up );
        moveLeftText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Move_Left );
        moveDownText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Move_Down );
        moveRightText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Move_Right );
        interactText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Interact );
        interactAltText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Interact_Alt );
        pauseText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Pause );
        gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact );
        gamepadAltText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Gamepad_Interact_Alt );
        gamepadPauseText.text = GameInput.Instance.GetBindingText ( GameInput.Binding.Gamepad_Pause );

        moveUp.ResizeButton( );
        moveLeft.ResizeButton( );
        moveDown.ResizeButton( );
        moveRight.ResizeButton( );
        interact.ResizeButton( );
        interactAlt.ResizeButton( );
        pause.ResizeButton( );
        gamepadInteract.ResizeButton( );
        gamepadAlt.ResizeButton( );
        gamepadPause.ResizeButton( );
    }

    private void RebindBinding( GameInput.Binding binding )
    {
        ShowKeyRebindVisual( );
        GameInput.Instance.RebindBinding( binding, ( ) =>
        {
            UpdateVisual( );
            HideKeyRebindVisual( );
        } );
    }
    
    public void Show(Action onOptionsHide)
    {
        this.onOptionsHide = onOptionsHide;
        musicButton.Select( );
        gameObject.SetActive( true );
    }

    public void Hide( )
    {
        onOptionsHide?.Invoke( );
        gameObject.SetActive ( false );
    }

    private void ShowKeyRebindVisual( )
    {
        keyRebindVisual.gameObject.SetActive( true );
    }

    private void HideKeyRebindVisual( )
    {
        keyRebindVisual.gameObject.SetActive( false );
    }

    private void OnDestroy( )
    {
        KitchenGameManager.Instance.OnGameUnpaused -= KitchenGameManager_OnGameUnpaused;
    }
}
