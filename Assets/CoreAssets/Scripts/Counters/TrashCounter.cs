using System;
public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyTrashed;
    public override void Interact( IKitchenObjectParent player )
    {
        OnAnyTrashed?.Invoke(this, EventArgs.Empty);
        player.GetKitchenObject( )?.DestroySelf( );
    }

    public new static void ResetStaticData( )
    {
        OnAnyTrashed = null;
    }
}
