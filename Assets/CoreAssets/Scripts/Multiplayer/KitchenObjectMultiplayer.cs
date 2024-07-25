using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class KitchenObjectMultiplayer : NetworkBehaviour
{
    [SerializeField] private KitchenObjectListSO kitchenObjectListSO;

    public static KitchenObjectMultiplayer Instance { get; private set; }

    private void Awake( )
    {
        Instance = this;
    }
    public void SpawnKitchenObject( KitchenObjectSO kitchenObjectSO, IKitchenObjectParent parentToAssign )
    {
        int kitchenObjectSOIndex = kitchenObjectListSO.kitchenObjectsSOList.IndexOf( kitchenObjectSO );
        NetworkObject parentNetworkObject = parentToAssign.GetNetworkObject( );
        SpawnKitchenObjectServerRpc( kitchenObjectSOIndex, parentNetworkObject );
    }

    [ServerRpc( RequireOwnership = false )]
    private void SpawnKitchenObjectServerRpc( int kitchenObjectSOIndex, NetworkObjectReference parentNetworkObjectReference )
    {
        KitchenObjectSO kitchenObjectSO = kitchenObjectListSO.kitchenObjectsSOList[kitchenObjectSOIndex];

        KitchenObject kitchenObject = Instantiate<KitchenObject>( kitchenObjectSO.prefab );
        kitchenObject.NetworkObject.Spawn( true );

        if ( parentNetworkObjectReference.TryGet( out NetworkObject parentNetworkObject ))
        {
            IKitchenObjectParent parentToAssign = parentNetworkObject.GetComponent<IKitchenObjectParent>( );
            if ( parentToAssign != null )
            {
                kitchenObject.SetKitchenObjectParent( parentToAssign );
            }
        }
    }
}
