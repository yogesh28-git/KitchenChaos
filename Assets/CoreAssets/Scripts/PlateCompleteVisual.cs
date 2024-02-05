using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO_GameObject[] kitchenObjectArray;
    [SerializeField] private PlateKitchenObject plateKitchenObject;

    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObj;
    }
    private void Start( )
    {
        plateKitchenObject.OnIngredientAdd += PlateKitchenObject_OnIngredientAdd;

        Array.ForEach( kitchenObjectArray, ( i ) => { i.gameObj.SetActive(false ); });
    }

    private void PlateKitchenObject_OnIngredientAdd( object sender, PlateKitchenObject.OnIngredientAddEventArgs e )
    {
        Array.Find( kitchenObjectArray, i => i.kitchenObjectSO == e.addedIngredient ).gameObj.SetActive( true );
    }
}
