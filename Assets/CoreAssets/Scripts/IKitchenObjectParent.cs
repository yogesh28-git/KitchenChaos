using UnityEngine;

public interface IKitchenObjectParent
{
    bool HasKitchenObject( );

    KitchenObject GetKitchenObject( );
    void SetKitchenObject( KitchenObject _kitchenObject );

    void ClearKitchenObject( );

    Transform GetKitchenObjectFollowTransform( );
}
