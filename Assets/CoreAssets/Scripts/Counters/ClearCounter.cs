using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact( IKitchenObjectParent player )
    {
        if ( !HasKitchenObject( ) )
        {
            if ( player.HasKitchenObject( ) )
            {
                player.GetKitchenObject().SetKitchenObjectParent( this );
            }
        }
        else
        {
            if(!player.HasKitchenObject( ) )
            {
                GetKitchenObject( ).SetKitchenObjectParent( player );
            }
        }
    }
}
