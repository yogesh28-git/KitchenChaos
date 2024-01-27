using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject seletedCounterVisual;
    private void Start( )
    {
        Player.Instance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged;
        HideSelectedCounterVisual();
    }

    private void Instance_OnSelectedCounterChanged( object sender, Player.OnSelectedCounterChangedEventArgs e )
    {
        if(e.selectedCounter == clearCounter)
        {
            ShowSelectedCounterVisual();
        }
        else
        {
            HideSelectedCounterVisual();
        }
    }

    private void ShowSelectedCounterVisual( )
    {
        seletedCounterVisual.SetActive( true );
    }
    private void HideSelectedCounterVisual( )
    {
        seletedCounterVisual.SetActive( false );
    }
}
