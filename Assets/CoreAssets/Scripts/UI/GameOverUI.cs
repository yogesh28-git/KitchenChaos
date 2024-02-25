using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeCount;

    private void Start( )
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManger_OnStateChanged;
        Hide( );
    }

    private void KitchenGameManger_OnStateChanged( object sender, System.EventArgs e )
    {
        if ( KitchenGameManager.Instance.isGameOver( ) )
        {
            recipeCount.text = DeliveryManager.Instance.GetDeliveredRecipeCount().ToString();
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
