using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopTransform;
    private KitchenObject kitchenObject;

    public virtual void Interact( IKitchenObjectParent player )
    {
        Debug.LogError( "Base Container Class Accessed" );
    }

    public virtual void InteractAlt( )
    {
        Debug.LogError( "Base Container Class Accessed" );
    }

    public bool HasKitchenObject( )
    {
        return kitchenObject != null;
    }
    public void SetKitchenObject( KitchenObject _kitchenObject )
    {
        kitchenObject = _kitchenObject;
    }
    public KitchenObject GetKitchenObject( )
    {
        return kitchenObject;
    }
    public void ClearKitchenObject( )
    {
        kitchenObject = null;
    }
    public Transform GetKitchenObjectFollowTransform( )
    {
        return counterTopTransform;
    }
}
