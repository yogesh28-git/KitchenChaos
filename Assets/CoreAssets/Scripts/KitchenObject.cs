using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectsSO;

    public string Name { get { return kitchenObjectsSO.objName; } }

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO( )
    {
        return kitchenObjectsSO;
    }

    public void SetKitchenObjectParent( IKitchenObjectParent _kitchenObjectParent )
    {
        if(this.kitchenObjectParent != null )
        {
            kitchenObjectParent.ClearKitchenObject( );
        }

        this.kitchenObjectParent = _kitchenObjectParent;
        transform.SetParent( _kitchenObjectParent.GetKitchenObjectFollowTransform( ) );
        transform.localPosition = Vector3.zero;

        kitchenObjectParent.SetKitchenObject( this );
    }

    public void DestroySelf( )
    {
        this.kitchenObjectParent.ClearKitchenObject();
        DestroyImmediate( this.gameObject );
    }

    public bool TryGetPlate( out PlateKitchenObject plateKitchenObject )
    {
        plateKitchenObject = this as PlateKitchenObject;
        return plateKitchenObject != null;
    }
    

    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent parentToAssign)
    {
        KitchenObject kitchenObject = Instantiate<KitchenObject>( kitchenObjectSO.prefab );
        kitchenObject.SetKitchenObjectParent( parentToAssign );

    }
}
