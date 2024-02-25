using UnityEngine;
using UnityEngine.UI;

public class GameClockUI : MonoBehaviour
{
    [SerializeField] private Image clockFillImage;

    private void Update( )
    {
        clockFillImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized( );
    }
}
