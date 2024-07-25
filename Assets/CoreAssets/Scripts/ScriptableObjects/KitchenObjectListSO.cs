using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKitchenObjectListSO", menuName = "ScriptableObjects/KitchenObjectListSO")]
public class KitchenObjectListSO : ScriptableObject
{
    public List<KitchenObjectSO> kitchenObjectsSOList;
}
