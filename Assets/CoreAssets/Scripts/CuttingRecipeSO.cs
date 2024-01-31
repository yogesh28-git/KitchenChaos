using UnityEngine;

[CreateAssetMenu(fileName = "NewCuttingRecipe", menuName = "ScriptableObjects/CuttingRecipeSO")]
public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObject input;
    public KitchenObject output;
}
