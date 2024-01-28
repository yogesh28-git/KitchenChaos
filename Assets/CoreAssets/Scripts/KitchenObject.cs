using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;

    public string Name { get { return kitchenObjectsSO.objName; } }

    private IKitchenObjectParent kitchenObjectParent;

    public void SetKitchenObjectParent( IKitchenObjectParent _kitchenObjectParent )
    {
        if(this.kitchenObjectParent != null )
        {
            kitchenObjectParent.ClearKitchenObject( );
        }

        this.kitchenObjectParent = _kitchenObjectParent;
        transform.SetParent( _kitchenObjectParent.GetKitchenObjectFollowTransform( ) );
        transform.localPosition = Vector3.zero;

        _kitchenObjectParent.SetKitchenObject( this );
    }
}
