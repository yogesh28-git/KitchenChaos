using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Player player;

    private string IS_WALKING = "IsWalking";

    private void Update( )
    {
        playerAnimator.SetBool(IS_WALKING, player.IsWalking());
    }
}
