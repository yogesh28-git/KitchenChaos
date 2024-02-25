using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCanvasUI : MonoBehaviour
{
    [SerializeField] private FoodIconSingleUI ingredientIconTemplate;
    [SerializeField] private PlateKitchenObject plate;

    private void Start( )
    {
        plate.OnIngredientAdd += Plate_OnIngredientAdd;
        ingredientIconTemplate.gameObject.SetActive( false );
    }

    private void Plate_OnIngredientAdd( object sender, PlateKitchenObject.OnIngredientAddEventArgs e )
    {
        FoodIconSingleUI iconUI = Instantiate<FoodIconSingleUI>( ingredientIconTemplate, this.transform );
        iconUI.icon.sprite = e.addedIngredient.sprite;
        iconUI.gameObject.SetActive( true );
    }

    private void OnDestroy( )
    {
        plate.OnIngredientAdd -= Plate_OnIngredientAdd;
    }

}
