using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnCounterDoorOpen;

    [SerializeField] private KitchenObjectsSO kitchenObjectSO;

    public override void Interact( IKitchenObjectParent player )
    {
        //spawn something and give to player only if player is empty handed
        if ( !player.HasKitchenObject( ) )
        {
            KitchenObject kitchenObject = Instantiate<KitchenObject>( kitchenObjectSO.prefab );
            kitchenObject.SetKitchenObjectParent( player );
            OnCounterDoorOpen?.Invoke(this, EventArgs.Empty );
        }
    }
}
