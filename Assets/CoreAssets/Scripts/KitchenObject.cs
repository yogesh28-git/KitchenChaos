using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;

    public string Name { get { return kitchenObjectsSO.objName; } }

    private ClearCounter clearCounter;
    public KitchenObject GetKitchenObject( )
    {
        return this;
    }

    public void SetCounter( ClearCounter _clearCounter )
    {
        if(this.clearCounter != null )
        {
            clearCounter.ClearKitchenObject( );
        }

        this.clearCounter = _clearCounter;
        transform.SetParent(_clearCounter.GetCounterTopTransform( ) );
        transform.localPosition = Vector3.zero;

        _clearCounter.SetKitchenObject( this );
    }
    public ClearCounter GetCounter()
    {
        return this.clearCounter;
    }

}
