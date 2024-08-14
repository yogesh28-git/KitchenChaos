using System;
using Unity.Netcode;
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

    private NetworkVariable<StoveState> currentState = new NetworkVariable<StoveState>( StoveState.IDLE );

    private NetworkVariable<float> fryTimer = new NetworkVariable<float>( 0f );

    private NetworkVariable<float> burnTimer = new NetworkVariable<float>( 0f );

    private float fryTimerMax = 1f;
    private float burnTimerMax = 1f;

    private KitchenObjectSO inputSO;
    private KitchenObjectSO friedOutput;
    private KitchenObjectSO burnedOutput;

    public enum StoveState
    {
        IDLE,
        FRYING,
        FRIED,
        BURNED
    }

    private void Start( )
    {
        if ( !IsServer )
            return;

        ResetStoveServerRpc( );
    }

    public override void OnNetworkSpawn( )
    {
        base.OnNetworkSpawn( );

        // these will be called for all clients
        fryTimer.OnValueChanged += FriedTimer_OnValueChanged;
        burnTimer.OnValueChanged += BurnTimer_OnValueChanged;
        currentState.OnValueChanged += StoveState_OnValueChanged;
    }

    private void FriedTimer_OnValueChanged( float previousValue, float newValue )
    {
        OnProgressChanged?.Invoke( this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = fryTimer.Value / fryTimerMax } );
    }

    private void BurnTimer_OnValueChanged( float previousValue, float newValue )
    {
        OnProgressChanged?.Invoke( this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = burnTimer.Value / burnTimerMax } );
    }

    private void StoveState_OnValueChanged( StoveState previousState, StoveState newState )
    {
        OnStateChanged?.Invoke( this, new OnStateChangedEventArgs { stoveState = currentState.Value } );
    }

    public override void Interact( IKitchenObjectParent player )
    {
        if ( !HasKitchenObject( ) )
        {
            //Player is only allowed to place those kitchenObjects that have a frying recipe.
            if ( player.HasKitchenObject( ) && HasFryingRecipe( player.GetKitchenObject( )?.GetKitchenObjectSO( ) ) )
            {
                //Player is placing an object.
                KitchenObject kitchenObject = player.GetKitchenObject( );
                kitchenObject.SetKitchenObjectParent( this );
                int kitchenObjectSOIndex = KitchenObjectMultiplayer.Instance.GetIndexOfKitchenObjectSO( kitchenObject.GetKitchenObjectSO( ) );
                StartFryingServerRpc( kitchenObjectSOIndex );
            }
        }
        else
        {
            //player is free handed
            if ( !player.HasKitchenObject( ) )
            {
                GetKitchenObject( ).SetKitchenObjectParent( player );
                ResetStoveServerRpc( );
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
                        ResetStoveServerRpc( );
                    }
                }
            }
        }
    }

    private void Update( )
    {
        if ( !IsServer )
            return;

        switch ( currentState.Value )
        {
            case StoveState.IDLE:
                break;
            case StoveState.FRYING:
                fryTimer.Value += Time.deltaTime;

                if ( fryTimer.Value > fryTimerMax )
                {
                    KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    KitchenObject.SpawnKitchenObject( friedOutput, this );
                    ChangeState( StoveState.FRIED );
                }
                break;
            case StoveState.FRIED:
                burnTimer.Value += Time.deltaTime;
                
                if ( burnTimer.Value > burnTimerMax )
                {
                    KitchenObject.DestroyKitchenObject( GetKitchenObject( ) );
                    KitchenObject.SpawnKitchenObject( burnedOutput, this );
                    ChangeState( StoveState.BURNED );
                }
                break;
            case StoveState.BURNED:
                break;
        }
    }

    [ServerRpc( RequireOwnership = false )]
    private void ResetStoveServerRpc( )
    {
        fryTimer.Value = 0;
        burnTimer.Value = 0;
        ChangeState( StoveState.IDLE );
    }


    [ServerRpc( RequireOwnership = false )]
    private void StartFryingServerRpc( int kitchenObjectSOIndex )
    {
        StartFryingClientRpc( kitchenObjectSOIndex );

        ChangeState( StoveState.FRYING );
    }

    [ClientRpc]
    private void StartFryingClientRpc( int kitchenObjectSOIndex )
    {
        inputSO = KitchenObjectMultiplayer.Instance.GetKitchenObjectSOFromIndex( kitchenObjectSOIndex );
        FryingRecipeSO recipeSO = GetFryingRecipeSOWithInput( inputSO );
        friedOutput = GetFriedOuputForInput( inputSO );
        burnedOutput = GetBurnedOutputForinput( inputSO );

        fryTimerMax = recipeSO.fryTimeMax;
        burnTimerMax = recipeSO.burnTimeMax;
    }

    private void ChangeState( StoveState targetState )
    {
        currentState.Value = targetState; 
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

    public bool IsFried( )
    {
        return currentState.Value == StoveState.FRIED;
    }
}
