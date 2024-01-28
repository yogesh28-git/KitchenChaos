using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopTransform;
    [SerializeField] private KitchenObjectsSO kitchenObjectSO;

    private KitchenObject kitchenObject;

    private void Start( )
    {
        kitchenObject = Instantiate<KitchenObject>( kitchenObjectSO.prefab );
        kitchenObject.SetKitchenObjectParent( this );
    }
    public void Interact( IKitchenObjectParent player)
    {
        if(this.kitchenObject == null )
        {
            kitchenObject = player.GetKitchenObject( );
            kitchenObject.SetKitchenObjectParent( this );
        }
        else
        {
            player.SetKitchenObject(kitchenObject );
        } 
    }

    public bool HasKitchenObject( )
    {
        return kitchenObject != null;
    }
    public void SetKitchenObject( KitchenObject _kitchenObject )
    {
        this.kitchenObject = _kitchenObject;
    }
    public KitchenObject GetKitchenObject() 
    { 
        return kitchenObject; 
    }
    public void ClearKitchenObject( )
    {
        this.kitchenObject = null;
    }
    public Transform GetKitchenObjectFollowTransform( )
    {
        return this.counterTopTransform;
    }
}
