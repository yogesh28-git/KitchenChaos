using System;
using Unity.Netcode;
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
            ContainerDoorOpenServerRpc( );
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ContainerDoorOpenServerRpc( )
    {
        ContainerDoorOpenClientRpc( );
    }

    [ClientRpc]
    private void ContainerDoorOpenClientRpc( )
    {
        OnCounterDoorOpen?.Invoke( this, EventArgs.Empty );
    }
}
