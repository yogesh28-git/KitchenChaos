public class TrashCounter : BaseCounter
{
    public override void Interact( IKitchenObjectParent player )
    {
        player.GetKitchenObject( )?.DestroySelf( );
    }
}
