using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact( IKitchenObjectParent player )
    {
        //Counter is empty
        if ( !HasKitchenObject( ) )
        {
            //Player has something 
            if ( player.HasKitchenObject( ) )
            {
                player.GetKitchenObject( ).SetKitchenObjectParent( this );
            }
        }
        //Counter has something on it
        else
        {
            //Player is free handed
            if ( !player.HasKitchenObject( ) )
            {
                GetKitchenObject( ).SetKitchenObjectParent( player );
            }
            //Player has something
            else
            {
                //Player holding a plate
                if ( player.GetKitchenObject( ).TryGetPlate( out PlateKitchenObject plateKitchenObject ) )
                {
                    //Add the kitchen object that was on counter to the plate and destroy it from the counter
                    if ( plateKitchenObject.TryAddIngredient( GetKitchenObject( ).GetKitchenObjectSO( ) ) )
                    {
                        GetKitchenObject( ).DestroySelf( );
                    }
                }
                else
                {
                    //Counter has plate on it
                    if ( GetKitchenObject( ).TryGetPlate( out plateKitchenObject ) )
                    {
                        //Add the kitchen object that was on player to the plate and destroy it from the player
                        if ( plateKitchenObject.TryAddIngredient( player.GetKitchenObject( ).GetKitchenObjectSO( ) ) )
                        {
                            player.GetKitchenObject( ).DestroySelf( );
                        }
                    }
                }
            }
        }
    }
}
