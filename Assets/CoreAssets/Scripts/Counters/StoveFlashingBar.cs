using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveFlashingBar : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";

    [SerializeField] private Animator animator;
    [SerializeField] private StoveCounter stoveCounter;

    private void Start( )
    {
        animator.SetBool( IS_FLASHING, false );
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged( object sender, IHasProgress.OnProgressChangedEventArgs e )
    {
        float warningThreshold = 0.5f;
        if ( stoveCounter.IsFried( ) && e.progressNormalized >= warningThreshold )
        {
            animator.SetBool ( IS_FLASHING, true );
        }
        else
        {
            animator.SetBool(IS_FLASHING, false );
        }

        if ( e.progressNormalized >= 1f )
            animator.SetBool( IS_FLASHING, false );
    }
}
