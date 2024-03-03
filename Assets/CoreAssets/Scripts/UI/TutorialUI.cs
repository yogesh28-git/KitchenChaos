using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [Header("Key Names")]
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

    private void Start( )
    {
        UpdateVisual( );
        Show( );

        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
    }

    private void KitchenGameManager_OnStateChanged( object sender, System.EventArgs e )
    {
        if ( KitchenGameManager.Instance.isCountDownActive( ) )
        {
            Hide( );
        }
    }

    private void UpdateVisual( )
    {
        moveUpText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Move_Up );
        moveLeftText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Move_Left );
        moveDownText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Move_Down );
        moveRightText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Move_Right );
        interactText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Interact );
        interactAltText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Interact_Alt );
        pauseText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Pause );
        gamepadInteractText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Gamepad_Interact );
        gamepadAltText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Gamepad_Interact_Alt );
        gamepadPauseText.text = GameInput.Instance.GetBindingText( GameInput.Binding.Gamepad_Pause );
    }

    private void Show( )
    {
        gameObject.SetActive( true );
    }

    private void Hide( )
    {
        gameObject.SetActive( false );
    }
}




