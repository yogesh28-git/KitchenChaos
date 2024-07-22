using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Image deliveryImage;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failureSprite;
    [SerializeField] private Image background;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failureColor;

    private const string DELIVERY_SUCCESS = "DELIVERY\nSUCCESS";
    private const string DELIVERY_FAILED = "DELIVERY\nFAILED";
    private const string POP_UP = "Popup";

    private void Start( )
    {
        DeliveryManager.OnDeliverySuccess += DeliveryManager_OnDeliverySuccess;
        DeliveryManager.OnDeliveryFailed += DeliveryManager_OnDeliveryFailed;

        Hide( );
    }
    private void DeliveryManager_OnDeliverySuccess( object sender, System.EventArgs e )
    {
        Show( );
        background.color = successColor;
        messageText.text = DELIVERY_SUCCESS;
        deliveryImage.sprite = successSprite;
        Invoke( nameof( Hide ), 2f );
    }

    private void DeliveryManager_OnDeliveryFailed( object sender, System.EventArgs e )
    {
        Show( );
        background.color = failureColor;
        messageText.text = DELIVERY_FAILED;
        deliveryImage.sprite = failureSprite;
        Invoke( nameof( Hide ), 1f );
    }

    private void OnDestroy( )
    {
        DeliveryManager.OnDeliverySuccess -= DeliveryManager_OnDeliverySuccess;
        DeliveryManager.OnDeliveryFailed -= DeliveryManager_OnDeliveryFailed;
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
