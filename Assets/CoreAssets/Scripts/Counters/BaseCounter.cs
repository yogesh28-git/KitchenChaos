using System;
using Unity.Netcode;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopTransform;
    private KitchenObject kitchenObject;

    public static event EventHandler OnAnyDrop;

    public virtual void Interact( IKitchenObjectParent player )
    {
        Debug.LogError( "Base Container Class Accessed" );
    }

    public virtual void InteractAlt( )
    {
        //Do Nothing
    }

    public bool HasKitchenObject( )
    {
        return kitchenObject != null;
    }
    public void SetKitchenObject( KitchenObject _kitchenObject )
    {
        kitchenObject = _kitchenObject;

        if(kitchenObject != null )
        {
            OnAnyDrop?.Invoke(this, EventArgs.Empty);
        }
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
    
    public static void ResetStaticData( )
    {
        OnAnyDrop = null;
    }

    public NetworkObject GetNetworkObject( )
    {
        return null;
    }
}
