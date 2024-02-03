using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnCounterDoorOpen;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact( IKitchenObjectParent player )
    {
        //spawn something and give to player only if player is empty handed
        if ( !player.HasKitchenObject( ) )
        {
            KitchenObject.SpawnKitchenObject( kitchenObjectSO, player );
            OnCounterDoorOpen?.Invoke(this, EventArgs.Empty );
        }
    }
}
