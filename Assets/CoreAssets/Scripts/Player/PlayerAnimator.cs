using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Player player;

    private string IS_WALKING = "IsWalking";

    private void Update( )
    {
        if ( !IsOwner )
            return;

        playerAnimator.SetBool( IS_WALKING, player.IsWalking( ) );
    }
}
