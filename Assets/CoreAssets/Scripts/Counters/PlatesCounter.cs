using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO plateSO;

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    private float plateSpawnTimer = 3f;
    private float plateSpawnDelay = 6f;
    private int plateCapacity = 4;
    private int plateCount = 0;

    private void Update( )
    {
        if ( plateCount < plateCapacity )
        {
            plateSpawnTimer += Time.deltaTime;
            if ( plateSpawnTimer > plateSpawnDelay )
            {
                //spawn a plate
                plateCount++;
                plateSpawnTimer = 0f;
                OnPlateSpawned?.Invoke( this, EventArgs.Empty);
            }
        }
    }

    public override void Interact( IKitchenObjectParent player )
    {
        if ( !player.HasKitchenObject( ) && plateCount > 0)
        {
            plateCount--;
            KitchenObject.SpawnKitchenObject( plateSO, player );
            OnPlateRemoved?.Invoke( this, EventArgs.Empty );
        }
    }


}
