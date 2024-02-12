using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipeSO", menuName = "ScriptableObjects/RecipeSO")]
public class RecipeSO : ScriptableObject
{
    public List<KitchenObjectSO> ingredients;

    public string name;
}
