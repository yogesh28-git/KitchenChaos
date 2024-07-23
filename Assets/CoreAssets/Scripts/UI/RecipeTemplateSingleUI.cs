using TMPro;
using UnityEngine;

public class RecipeTemplateSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeName;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private FoodIconSingleUI iconTemplate;

    private RecipeSO recipeSO;
    private void CreateOrUpdateIcons(RecipeSO recipeSO )
    {

        int numOfIngredients = recipeSO.ingredients.Count;
        int index = 0;

        FoodIconSingleUI iconVisual = null;
        foreach(Transform child in iconContainer )
        {
            if ((index >= numOfIngredients))
            {
                return;
            }
            if ( !child.gameObject.activeSelf )
            {
                iconVisual = child?.GetComponent<FoodIconSingleUI>();
                iconVisual.icon.sprite = recipeSO.ingredients[index].sprite;
                iconVisual.gameObject.SetActive( true );
                index++;
            }
        }
        while(index < numOfIngredients )
        {
            iconVisual = CreateIcon( );
            iconVisual.icon.sprite = recipeSO.ingredients[index].sprite;
            iconVisual.gameObject.SetActive( true );
            index++;
        }


    }

    private FoodIconSingleUI CreateIcon( )
    {
        FoodIconSingleUI iconVisual = Instantiate<FoodIconSingleUI>( iconTemplate, iconContainer);
        iconVisual.gameObject.SetActive( false );
        return iconVisual;
    }
    public void SetRecipeData(RecipeSO recipeSO )
    {
        recipeName.text = recipeSO.name;
        this.recipeSO = recipeSO;

        CreateOrUpdateIcons( recipeSO );
    }

    public void ClearData( )
    {
        recipeName.text = "";

        foreach(Transform child in iconContainer )
        {
            child.gameObject.SetActive( false );
        }
    }

    public bool isThisRecipe(RecipeSO recipeSO )
    {
        return this.recipeSO == recipeSO;
    }
}
