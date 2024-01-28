using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact( IKitchenObjectParent player )
    {
        if ( !HasKitchenObject( ) )
        {
            SetKitchenObject( player.GetKitchenObject( ) ) ;
            GetKitchenObject().SetKitchenObjectParent( this );
        }
        else
        {
            GetKitchenObject( ).SetKitchenObjectParent( player );
        }
    }
}
