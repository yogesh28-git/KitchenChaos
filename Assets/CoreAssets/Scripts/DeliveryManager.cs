using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : NetworkBehaviour
{
    public static event EventHandler OnDeliveryFailed;
    public static event EventHandler OnDeliverySuccess;
    public event EventHandler<RecipeInfoEventArgs> OnRecipeSpawned;
    public event EventHandler<RecipeInfoEventArgs> OnRecipeCompleted;

    public class RecipeInfoEventArgs : EventArgs
    {
        public RecipeSO recipeSO;
    }
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO validRecipeList;
    private List<RecipeSO> waitingRecipeList;

    private float spawnTimer = 4f;
    private float spawnTimerMax = 4f;
    private int recipeMaxCount = 4;
    private int successfulRecipesCount = 0;

    private void Awake( )
    {
        if ( Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy( this.gameObject );
        }

        waitingRecipeList = new List<RecipeSO>( );
    }

    private void Update( )
    {
        // Only Server Should spawn recipes
        if ( !IsServer )
            return;

        if ( KitchenGameManager.Instance.isGamePlaying( ) )
            spawnTimer -= Time.deltaTime;
        if ( spawnTimer < 0 )
        {
            spawnTimer = spawnTimerMax;
            //Spawn the recipe
            if ( waitingRecipeList.Count < recipeMaxCount )
            {
                int randomRecipeIndex = Random.Range( 0, validRecipeList.recipes.Count );
                GenerateRecipeClientRpc( randomRecipeIndex );
            }
        }
    }

    [ClientRpc]
    public void GenerateRecipeClientRpc( int randomRecipeIndex )
    {
        RecipeSO randomRecipe = validRecipeList.recipes[randomRecipeIndex];
        OnRecipeSpawned?.Invoke( this, new RecipeInfoEventArgs { recipeSO = randomRecipe } );
        waitingRecipeList.Add( randomRecipe );
    }

    public void DeliverRecipe( PlateKitchenObject plate )
    {
        bool isIngredientMatch = false;
        bool isRecipeMatch = false;

        for ( int i = 0; i < waitingRecipeList.Count; i++ )
        {
            RecipeSO recipe = waitingRecipeList[i];
            if ( recipe.ingredients.Count == plate.GetKitchenObjectSOList( ).Count )
            {
                isRecipeMatch = true;
                foreach ( KitchenObjectSO ingredient in recipe.ingredients )
                {
                    isIngredientMatch = false;
                    foreach ( KitchenObjectSO kitchenObject in plate.GetKitchenObjectSOList( ) )
                    {
                        if ( kitchenObject == ingredient )
                        {
                            isIngredientMatch = true;
                            break;
                        }
                    }
                    if ( !isIngredientMatch )
                    {
                        isRecipeMatch = false;
                        break;
                    }
                }
                if ( isRecipeMatch )
                {
                    DeliverySuccessServerRpc( i );
                    break;
                }
            }
        }
        if ( !isRecipeMatch )
        {
            DeliveryFailedServerRpc( );
        }
    }
    [ServerRpc( RequireOwnership = false )]
    private void DeliverySuccessServerRpc( int recipeIndex )
    {
        DeliverySuccessClientRpc( recipeIndex );
    }

    [ClientRpc]
    private void DeliverySuccessClientRpc( int recipeIndex )
    {
        OnRecipeCompleted?.Invoke( this, new RecipeInfoEventArgs { recipeSO = waitingRecipeList[recipeIndex] } );
        waitingRecipeList.RemoveAt( recipeIndex );
        OnDeliverySuccess?.Invoke( this, EventArgs.Empty );
        successfulRecipesCount++;
    }

    [ServerRpc( RequireOwnership = false )]
    private void DeliveryFailedServerRpc( )
    {
        DeliveryFailedClientRpc( );
    }
    [ClientRpc]
    private void DeliveryFailedClientRpc( )
    {
        OnDeliveryFailed?.Invoke( this, EventArgs.Empty );
    }

    public List<RecipeSO> GetWaitingRecipeList( )
    {
        return waitingRecipeList;
    }
    public int GetDeliveredRecipeCount( )
    {
        return successfulRecipesCount;
    }

    public static void ResetStaticData( )
    {
        OnDeliveryFailed = null;
        OnDeliverySuccess = null;
    }
}
