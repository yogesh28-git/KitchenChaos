using UnityEngine;

[CreateAssetMenu(fileName ="NewKitchenObjectSO", menuName = "ScriptableObjects/KitchenObjectSO")]
public class KitchenObjectsSO : ScriptableObject
{
    public KitchenObject prefab;
    public Sprite sprite;
    public string objName;
}
