using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
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

    private float spawnTimer = 0;
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
            Destroy(this.gameObject);
        }

        waitingRecipeList = new List<RecipeSO>();
    }

    private void Update( )
    {
        if( KitchenGameManager.Instance.isGamePlaying( ) )
            spawnTimer -= Time.deltaTime;
        if ( spawnTimer < 0 )
        {
            spawnTimer = spawnTimerMax;
            //Spawn the recipe
            if(waitingRecipeList.Count < recipeMaxCount )
            {
                RecipeSO randomRecipe = validRecipeList.recipes[Random.Range( 0, validRecipeList.recipes.Count )];
                OnRecipeSpawned?.Invoke(this, new RecipeInfoEventArgs  { recipeSO = randomRecipe });
                waitingRecipeList.Add( randomRecipe );
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plate)
    {
        bool isIngredientMatch = false;
        bool isRecipeMatch = false;

        foreach (RecipeSO recipe in waitingRecipeList )
        {
            if(recipe.ingredients.Count == plate.GetKitchenObjectSOList().Count )
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
                    OnRecipeCompleted?.Invoke( this, new RecipeInfoEventArgs { recipeSO = recipe } );
                    waitingRecipeList.Remove( recipe );
                    OnDeliverySuccess?.Invoke(this, EventArgs.Empty );
                    successfulRecipesCount++;
                    break;
                }
            }
        }
        if ( !isRecipeMatch )
        {
            OnDeliveryFailed?.Invoke( this, EventArgs.Empty );
        }
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
