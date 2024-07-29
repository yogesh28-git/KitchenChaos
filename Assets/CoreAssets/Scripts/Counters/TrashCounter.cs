using System;
using Unity.Netcode;
public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyTrashed;
    public override void Interact( IKitchenObjectParent player )
    {
        KitchenObject.DestroyKitchenObject( player.GetKitchenObject( ) );
        InteractLogicClientRpc( );
    }

    public new static void ResetStaticData( )
    {
        OnAnyTrashed = null;
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc( )
    {
        InteractLogicClientRpc( );
    }

    [ClientRpc]
    private void InteractLogicClientRpc( )
    {
        OnAnyTrashed?.Invoke( this, EventArgs.Empty );
    }
}
