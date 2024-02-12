using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO validRecipeList;
    private List<RecipeSO> waitingRecipeList;

    private float spawnTimer = 0;
    private float spawnTimerMax = 4f;
    private int recipeMaxCount = 4;

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
        spawnTimer = spawnTimerMax;
    }

    private void Update( )
    {
        spawnTimer -= Time.deltaTime;
        if ( spawnTimer < 0 )
        {
            spawnTimer = spawnTimerMax;

            //Spawn the recipe
            if(waitingRecipeList.Count < recipeMaxCount )
            {
                RecipeSO randomRecipe = validRecipeList.recipes[Random.Range( 0, validRecipeList.recipes.Count )];
                waitingRecipeList.Add( randomRecipe );
                Debug.Log( "name: " + randomRecipe );
            }
        }
    }

    bool isIngredientMatch = false;
    bool isRecipeMatch = false;

    public void DeliverRecipe(PlateKitchenObject plate)
    {
        foreach(RecipeSO recipe in waitingRecipeList )
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
                    Debug.Log( "Player Delivered the correct Recipe" );
                    waitingRecipeList.Remove( recipe );
                    break;
                }
            }
        }
        if ( !isRecipeMatch )
        {
            Debug.Log( "WRONG RECIPE" );
        }
    }

}
