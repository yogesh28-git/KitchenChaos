using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private Transform counterTopTransform;
    [SerializeField] private KitchenObjectsSO kitchenObjectSO;
    [SerializeField] private ClearCounter secondCounter;

    private KitchenObject kitchenObject;
    public bool isTesting = false;
    private void Update( )
    {
        if(isTesting && Input.GetKeyDown(KeyCode.T))
        {
            kitchenObject.SetCounter( secondCounter );
        }
    }
    public void Interact( )
    {
        if(this.kitchenObject == null )
        {
            this.kitchenObject = Instantiate<KitchenObject>( kitchenObjectSO.prefab );
            kitchenObject.SetCounter( this );
        }
        else
        {
            Debug.Log( kitchenObject.Name );
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

    public Transform GetCounterTopTransform( )
    {
        return this.counterTopTransform;
    }
}
