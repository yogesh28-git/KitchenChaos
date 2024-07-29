using System;
using Unity.Netcode;
using UnityEngine;

public class PlatesCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO plateSO;

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    private float plateSpawnTimer = 6f;
    private float plateSpawnDelay = 6f;
    private int plateCapacity = 4;
    private int plateCount = 0;

    private void Update( )
    {
        if ( !IsServer )
            return;

        if ( KitchenGameManager.Instance.isGamePlaying( ) && plateCount < plateCapacity )
        {
            plateSpawnTimer += Time.deltaTime;
            if ( plateSpawnTimer > plateSpawnDelay )
            {
                //spawn a plate
                PlateSpawnServerRpc( );
                plateSpawnTimer = 0f;
                
            }
        }
    }

    [ServerRpc]
    private void PlateSpawnServerRpc( )
    {
        PlateSpawnClientRpc( );
    }

    [ClientRpc]
    private void PlateSpawnClientRpc( )
    {
        plateCount++;
        OnPlateSpawned?.Invoke( this, EventArgs.Empty );
    }

    public override void Interact( IKitchenObjectParent player )
    {
        if ( !player.HasKitchenObject( ) && plateCount > 0)
        {
            PlatesCounterInteractServerRpc( );
            KitchenObject.SpawnKitchenObject( plateSO, player );
            
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PlatesCounterInteractServerRpc( )
    {
        PlatesCounterInteractClientRpc( );
    }

    [ClientRpc]
    private void PlatesCounterInteractClientRpc( )
    {
        plateCount--;
        OnPlateRemoved?.Invoke( this, EventArgs.Empty );
    }
}
