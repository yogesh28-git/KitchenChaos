using UnityEngine;

[CreateAssetMenu( fileName = "NewFryingRecipe", menuName = "ScriptableObjects/FryingRecipeSO" )]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO friedOutput;
    public KitchenObjectSO burnedOutput;
    public float fryTimeMax;
    public float burnTimeMax;
}
