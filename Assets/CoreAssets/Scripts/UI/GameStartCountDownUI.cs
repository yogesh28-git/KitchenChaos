using TMPro;
using UnityEngine;

public class GameStartCountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;

    private void Start( )
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManger_OnStateChanged;
        Hide( );
    }

    private void Update( )
    {
        countDownText.text = Mathf.Ceil( KitchenGameManager.Instance.GetCountDownTimer( ) ).ToString( );
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
