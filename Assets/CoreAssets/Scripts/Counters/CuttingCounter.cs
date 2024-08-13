using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;
    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;
    public override void Interact( IKitchenObjectParent player )
    {
        //Counter is empty
        if ( !HasKitchenObject( ) )
        {
            //player carrying something and tries to place a valid item
            if ( player.HasKitchenObject( )  && HasCuttingRecipe( player.GetKitchenObject( )?.GetKitchenObjectSO( )) )
            {
                player.GetKitchenObject( ).SetKitchenObjectParent( this );

                CuttingProgressResetServerRpc( );
            }
        }
        //counter has something on it
        else
        {
            //player is free 
            if ( !player.HasKitchenObject( ) )
            {
                GetKitchenObject( ).SetKitchenObjectParent( player );
            }
            //player also carrying something
            else
            {
                //Player holding a plate
                if ( player.GetKitchenObject( ).TryGetPlate( out PlateKitchenObject plateKitchenObject ) )
                {
                    //Add the kitchen object that was on counter to the plate and destroy it from the counter
                    if ( plateKitchenObject.TryAddIngredient( GetKitchenObject( ).GetKitchenObjectSO( ) ) )
                    {
                        KitchenObject.DestroyKitchenObject( GetKitchenObject( ) );
                    }
                }
            }
        }
    }

    public override void InteractAlt()
    {
        if ( HasKitchenObject( ) && HasCuttingRecipe( GetKitchenObject( )?.GetKitchenObjectSO( ) ))
        {
            InteractAltLogicServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CuttingProgressResetServerRpc( )
    {
        CuttingProgressResetClientRpc( );
    }

    [ClientRpc]
    private void CuttingProgressResetClientRpc( )
    {
        cuttingProgress = 0;
        OnProgressChanged?.Invoke( this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = cuttingProgress } );
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractAltLogicServerRpc( )
    {
        InteractAltLogicClientRpc( );
        TestCuttingProgressServerRpc( );
    }

    [ClientRpc]
    private void InteractAltLogicClientRpc( )
    {
        cuttingProgress++;
        int cuttingProgressMax = GetCuttingRecipeSOWithInput( GetKitchenObject( )?.GetKitchenObjectSO( ) ).cuttingProgressMax;
        OnProgressChanged?.Invoke( this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = ( float ) cuttingProgress / cuttingProgressMax } );
        OnCut?.Invoke( this, EventArgs.Empty );
        OnAnyCut?.Invoke( this, EventArgs.Empty );
    }

    [ServerRpc( RequireOwnership = false )]
    private void TestCuttingProgressServerRpc( )
    {
        int cuttingProgressMax = GetCuttingRecipeSOWithInput( GetKitchenObject( )?.GetKitchenObjectSO( ) ).cuttingProgressMax;

        if ( cuttingProgress >= cuttingProgressMax )
        {
            KitchenObjectSO slices = GetOuputForInput( GetKitchenObject( ).GetKitchenObjectSO( ) );
            KitchenObject.DestroyKitchenObject( GetKitchenObject( ) );
            KitchenObject.SpawnKitchenObject( slices, this );
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

    public new static void ResetStaticData( )
    {
        OnAnyCut = null;
    }
}
