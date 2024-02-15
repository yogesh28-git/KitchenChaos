using TMPro;
using UnityEngine;

public class GameStartCountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;

    private void Start( )
    {
        KitchenGameManger.Instance.OnStateChanged += KitchenGameManger_OnStateChanged;
    }

    private void Update( )
    {
        countDownText.text = Mathf.Ceil( KitchenGameManger.Instance.GetCountDownTimer( ) ).ToString( );
    }

    private void KitchenGameManger_OnStateChanged( object sender, System.EventArgs e )
    {
        if ( KitchenGameManger.Instance.isCountDownActive( ) )
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
}
