using System;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;
    public override void Interact( IKitchenObjectParent player )
    {
        if ( !HasKitchenObject( ) )
        {
            if ( player.HasKitchenObject( )  && HasCuttingRecipe( player.GetKitchenObject( )?.GetKitchenObjectSO( )) )
            {
                //Player is placing an object.
                player.GetKitchenObject( ).SetKitchenObjectParent( this );
                cuttingProgress = 0;
                OnProgressChanged?.Invoke( this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = cuttingProgress } );
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
        if ( HasKitchenObject( ) && HasCuttingRecipe( GetKitchenObject( )?.GetKitchenObjectSO( ) ))
        {
            cuttingProgress++;
            int cuttingProgressMax = GetCuttingRecipeSOWithInput( GetKitchenObject( )?.GetKitchenObjectSO( ) ).cuttingProgressMax;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = (float)cuttingProgress/cuttingProgressMax});
            OnCut?.Invoke( this, EventArgs.Empty );

            if(cuttingProgress >= cuttingProgressMax )
            {
                KitchenObjectSO slices = GetOuputForInput( GetKitchenObject( ).GetKitchenObjectSO( ) );
                GetKitchenObject( ).DestroySelf( );
                KitchenObject.SpawnKitchenObject( slices, this );
            }
        }
    }

    private bool HasCuttingRecipe(KitchenObjectSO input)
    {
        return ( GetCuttingRecipeSOWithInput( input ) != null );
    }
    private KitchenObjectSO GetOuputForInput(KitchenObjectSO input )
    {
        return GetCuttingRecipeSOWithInput( input )?.output ?? null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO input )
    {
        foreach(var cuttingRecipeSO in cuttingRecipeSOArray )
        {
            if(input == cuttingRecipeSO.input )
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
