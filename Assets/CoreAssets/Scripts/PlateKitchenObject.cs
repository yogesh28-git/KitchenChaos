using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddEventArgs> OnIngredientAdd;

    public class OnIngredientAddEventArgs : EventArgs
    {
        public KitchenObjectSO addedIngredient;
    }

    [SerializeField] private List<KitchenObjectSO> validIngredientsList;
    private List<KitchenObjectSO> ingredientsList;

    private void Awake( )
    {
        ingredientsList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO )
    {
        //Not a valid ingredient
        if(!validIngredientsList.Contains( kitchenObjectSO ))
        {
            return false;
        }

        //Cannot add duplicates of same type
        if(ingredientsList.Contains( kitchenObjectSO ) )
        {
            return false;
        }

        //Add the ingredient
        ingredientsList.Add( kitchenObjectSO );
        OnIngredientAdd?.Invoke(this, new OnIngredientAddEventArgs { addedIngredient =  kitchenObjectSO });
        return true;
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList( )
    {
        return ingredientsList;
    }
}
