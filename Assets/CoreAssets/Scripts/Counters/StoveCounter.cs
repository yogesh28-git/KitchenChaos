using System;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public StoveState stoveState;
    }

    private StoveState currentState;

    private float fryTimer;
    private float burnTimer;
    private float fryTimerMax;
    private float burnTimerMax;

    private KitchenObjectSO inputSO;
    private KitchenObjectSO friedOutput;
    private KitchenObjectSO burnedOutput;

    public enum StoveState
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    private void Start( )
    {
        ResetStove();
    }
    public override void Interact( IKitchenObjectParent player )
    {
        if ( !HasKitchenObject( ) )
        {
            //Player is only allowed to place those kitchenObjects that have a frying recipe.
            if ( player.HasKitchenObject( ) && HasFryingRecipe( player.GetKitchenObject( )?.GetKitchenObjectSO( ) ) )
            {
                //Player is placing an object.
                player.GetKitchenObject( ).SetKitchenObjectParent( this );
                StartFrying( );
            }
        }
        else
        {
            if ( !player.HasKitchenObject( ) )
            {
                GetKitchenObject( ).SetKitchenObjectParent( player );
                ResetStove( );
            }
        }
    }

    private void Update( )
    {
        switch(currentState )
        {
            case StoveState.Idle:
                break;
            case StoveState.Frying:
                fryTimer += Time.deltaTime;
                Debug.Log( "FryTimer: " + fryTimer );
                OnProgressChanged?.Invoke( this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = fryTimer/fryTimerMax } );
                if (fryTimer > fryTimerMax )
                {
                    GetKitchenObject().DestroySelf( );
                    KitchenObject.SpawnKitchenObject( friedOutput, this );
                    ChangeState( StoveState.Fried );
                }
                break;
            case StoveState.Fried:
                burnTimer += Time.deltaTime;
                Debug.Log( "BurnTimer: " + burnTimer );
                OnProgressChanged?.Invoke( this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = burnTimer/burnTimerMax } );
                if (burnTimer > burnTimerMax )
                {
                    GetKitchenObject( ).DestroySelf( );
                    KitchenObject.SpawnKitchenObject( burnedOutput, this );
                    ChangeState( StoveState.Burned );
                }
                break;
            case StoveState.Burned:
                break;
        }
    }

    private void ResetStove( )
    {
        fryTimer = 0;
        burnTimer = 0;
        ChangeState(StoveState.Idle );

        OnProgressChanged?.Invoke( this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0 } );
    }

    private void StartFrying( )
    {
        inputSO = GetKitchenObject( ).GetKitchenObjectSO( );
        FryingRecipeSO recipeSO = GetFryingRecipeSOWithInput(inputSO );
        friedOutput = GetFriedOuputForInput( inputSO );
        burnedOutput = GetBurnedOutputForinput( inputSO );

        fryTimerMax = recipeSO.fryTimeMax;
        burnTimerMax = recipeSO.burnTimeMax;
        
        ChangeState(StoveState.Frying);
    }

    private void ChangeState(StoveState targetState )
    {
        currentState = targetState;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { stoveState = targetState });
    }

    private bool HasFryingRecipe( KitchenObjectSO input )
    {
        return ( GetFryingRecipeSOWithInput( input ) != null );
    }
    private KitchenObjectSO GetFriedOuputForInput( KitchenObjectSO input )
    {
        return GetFryingRecipeSOWithInput( input )?.friedOutput ?? null;
    }

    private KitchenObjectSO GetBurnedOutputForinput( KitchenObjectSO input )
    {
        return GetFryingRecipeSOWithInput( input )?.burnedOutput ?? null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput( KitchenObjectSO input )
    {
        foreach ( var fryingRecipeSO in fryingRecipeSOArray )
        {
            if ( input == fryingRecipeSO.input )
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
}
