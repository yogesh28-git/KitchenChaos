using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private float footStepTimer = 0;
    private float footStepTimerMax = 0.1f;
    private float volume = 1f;

    private void Update( )
    {
        footStepTimer += Time.deltaTime;
        if( footStepTimer > footStepTimerMax )
        {
            footStepTimer = 0;

            if ( Player.Instance.IsWalking( ) )
            {
                SoundManager.Instance.PlayerFootStepsSound( Player.Instance.transform.position, volume );
            } 
        }
    }
}
