using UnityEngine;
using Unity.Netcode;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectsSO;
    private Transform followTransform;

    public string Name { get { return kitchenObjectsSO.objName; } }

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO( )
    {
        return kitchenObjectsSO;
    }

    public void SetKitchenObjectParent( IKitchenObjectParent _kitchenObjectParent )
    {
        SetKitchenObjectParentServerRpc( _kitchenObjectParent.GetNetworkObject( ) );
    }

    [ServerRpc( RequireOwnership = false )]
    private void SetKitchenObjectParentServerRpc( NetworkObjectReference kitchenObjectParentNetworkObjectReference )
    {
        SetKitchenObjectParentClientRpc( kitchenObjectParentNetworkObjectReference );
    }

    [ClientRpc]
    private void SetKitchenObjectParentClientRpc( NetworkObjectReference kitchenObjectParentNetworkObjectReference )
    {
        if ( this.kitchenObjectParent != null )
        {
            kitchenObjectParent.ClearKitchenObject( );
        }

        if (!kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject ) )
        {
            return;
        }
        this.kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>( );

        followTransform = kitchenObjectParent.GetKitchenObjectFollowTransform( );

        kitchenObjectParent.SetKitchenObject( this );
    }

    protected virtual void Update( )
    {
        if ( followTransform != null )
        {
            transform.position = followTransform.position;
            transform.rotation = followTransform.rotation;
        }
    }

    public void DestroySelf( )
    {
        this.kitchenObjectParent.ClearKitchenObject( );
        DestroyImmediate( this.gameObject );
    }

    public bool TryGetPlate( out PlateKitchenObject plateKitchenObject )
    {
        plateKitchenObject = this as PlateKitchenObject;
        return plateKitchenObject != null;
    }


    public static void SpawnKitchenObject( KitchenObjectSO kitchenObjectSO, IKitchenObjectParent parentToAssign )
    {
        KitchenObjectMultiplayer.Instance.SpawnKitchenObject( kitchenObjectSO, parentToAssign );
    }
}
