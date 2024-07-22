using TMPro;
using UnityEngine;

public class GameStartCountDownUI : MonoBehaviour
{ 
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private Animator countDownAnimator;
    private const string COUNT_DOWN = "CountDown";
    private int previousCountDown = 0;

    private void Start( )
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManger_OnStateChanged;
        Hide( );
    }

    private void Update( )
    {
        int currentCountDown = Mathf.CeilToInt( KitchenGameManager.Instance.GetCountDownTimer( ) );
        countDownText.text = currentCountDown.ToString( );
        if( previousCountDown != currentCountDown )
        {
            previousCountDown = currentCountDown;
            countDownAnimator.SetTrigger( COUNT_DOWN );
            SoundManager.Instance.PlayCountDownSound( );
        }
    }

    private void KitchenGameManger_OnStateChanged( object sender, System.EventArgs e )
    {
        if ( KitchenGameManager.Instance.isCountDownActive( ) )
        {
            Show( );
        }
        else
        {
            Hide( );
        }
    }

    private void Show( )
    {
        gameObject.SetActive( true );
    }
    private void Hide( )
    {
        gameObject.SetActive( false );
    }
    private void OnDestroy( )
    {
        KitchenGameManager.Instance.OnStateChanged -= KitchenGameManger_OnStateChanged;
    }
}
