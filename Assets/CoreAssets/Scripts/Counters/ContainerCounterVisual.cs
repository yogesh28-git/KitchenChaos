using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;
    [SerializeField] private Animator containerCounterAnimator;

    private const string OPEN_CLOSE = "OpenClose";

    private void Start( )
    {
        containerCounter.OnCounterDoorOpen += ContainerCounter_OnCounterDoorOpen;
    }

    private void ContainerCounter_OnCounterDoorOpen( object sender, System.EventArgs e )
    {
        containerCounterAnimator.SetTrigger(OPEN_CLOSE);
    }
}
