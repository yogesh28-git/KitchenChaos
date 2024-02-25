using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform recipeContainer;
    [SerializeField] private RecipeTemplateSingleUI recipeTemplate;

    private void Start( )
    {
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;

        recipeTemplate.gameObject.SetActive( false );
        recipeTemplate.ClearData();
    }

    private void DeliveryManager_OnRecipeCompleted( object sender, DeliveryManager.RecipeInfoEventArgs e )
    {
        ClearVisual( e.recipeSO );
    }

    private void DeliveryManager_OnRecipeSpawned( object sender, DeliveryManager.RecipeInfoEventArgs e )
    {
        CreateOrUpdateVisual( e.recipeSO );
    }

    private void CreateOrUpdateVisual( RecipeSO recipeSO )
    {
        RecipeTemplateSingleUI recipeVisual = null;
        foreach(Transform child in recipeContainer )
        {
            if(!child.gameObject.activeSelf && child != recipeTemplate.transform )
            {
                //Find an inactive visual and assign it
                recipeVisual = child?.GetComponent<RecipeTemplateSingleUI>();
                break;
            }
        }
        if(recipeVisual == null)
        {
            recipeVisual = CreateVisual( );
        }

        UpdateVisual( recipeVisual, recipeSO );
    }

    private void UpdateVisual(RecipeTemplateSingleUI recipeVisual, RecipeSO recipeSO)
    {
        recipeVisual.gameObject.SetActive( true );
        recipeVisual.SetRecipeData( recipeSO );
    }

    private void ClearVisual( RecipeSO recipeSO )
    {
        foreach ( Transform child in recipeContainer )
        {
            if ( child.gameObject.activeSelf )
            {
                RecipeTemplateSingleUI recipeVisual = child?.GetComponent<RecipeTemplateSingleUI>( );
                if ( recipeVisual == null )
                    continue;
                if ( !recipeVisual.isThisRecipe( recipeSO ) )
                    continue;

                //Clear only one such visual.
                recipeVisual.ClearData( );
                recipeVisual.gameObject.SetActive( false );
                return;
            }
        }
    }

    private RecipeTemplateSingleUI CreateVisual( )
    {
        RecipeTemplateSingleUI recipeVisual = Instantiate<RecipeTemplateSingleUI>( recipeTemplate, recipeContainer );
        recipeVisual.gameObject.SetActive( false );
        return recipeVisual;
    }

    private void OnDestroy( )
    {
        DeliveryManager.Instance.OnRecipeSpawned -= DeliveryManager_OnRecipeSpawned;
        DeliveryManager.Instance.OnRecipeCompleted -= DeliveryManager_OnRecipeCompleted;
    }


}
