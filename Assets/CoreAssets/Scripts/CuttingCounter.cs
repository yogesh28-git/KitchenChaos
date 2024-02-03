using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOList;
    public override void Interact( IKitchenObjectParent player )
    {
        if ( !HasKitchenObject( ) )
        {
            if ( player.HasKitchenObject( ) )
            {
                player.GetKitchenObject( ).SetKitchenObjectParent( this );
            }
        }
        else
        {
            if ( !player.HasKitchenObject( ) )
            {
                GetKitchenObject( ).SetKitchenObjectParent( player );
            }
        }
    }

    public override void InteractAlt()
    {
        if ( HasKitchenObject( ) && HasCuttingRecipe( GetKitchenObject( )?.GetKitchenObjectSO( ) ) )
        {
            KitchenObjectSO slices = GetOuputForInput( GetKitchenObject( ).GetKitchenObjectSO( ) );
            GetKitchenObject( ).DestroySelf( );
            KitchenObject.SpawnKitchenObject(slices,this );
        }
    }

    private bool HasCuttingRecipe(KitchenObjectSO input)
    {
        foreach(var cuttingRecipeSO in cuttingRecipeSOList)
        {
            if(input == cuttingRecipeSO.input)
            { return true; }
        }
        return false;
    }
    private KitchenObjectSO GetOuputForInput(KitchenObjectSO input )
    {
        foreach ( var cuttingRecipeSO in cuttingRecipeSOList )
        {
            if ( input == cuttingRecipeSO.input )
            { return cuttingRecipeSO.output; }
        }
        return null;
    }
}
