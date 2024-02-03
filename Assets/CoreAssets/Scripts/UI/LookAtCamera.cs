using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private FacingMode mode;
    private enum FacingMode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted
    }
    private void LateUpdate( )
    {
        switch ( mode )
        {
            case FacingMode.LookAt:
                transform.LookAt(Camera.main.transform.position); 
                break;
            case FacingMode.LookAtInverted:
                transform.LookAt( -Camera.main.transform.position );
                break;
            case FacingMode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case FacingMode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
        
    }
}
