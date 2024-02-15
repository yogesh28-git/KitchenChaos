using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance;
    public void Awake( )
    {
        if (Instance == null )
        {
            Instance = this;
        }
    }
    public override void Interact( IKitchenObjectParent player )
    {
        if(player.HasKitchenObject())
        {
            if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                DeliveryManager.Instance.DeliverRecipe( plateKitchenObject );
                plateKitchenObject.DestroySelf();
            }
        }
    }
}
