using UnityEngine;

[CreateAssetMenu(fileName = "NewCuttingRecipe", menuName = "ScriptableObjects/CuttingRecipeSO")]
public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public int cuttingProgressMax;
}
