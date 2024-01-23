using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    private Vector3 moveDir;
    
    private void Update( )
    {
        Vector2 inputVector = Vector2.zero;

        if ( Input.GetKey( KeyCode.W ) )
            inputVector.y = 1;
        if ( Input.GetKey( KeyCode.S ) )
            inputVector.y = -1;
        if ( Input.GetKey( KeyCode.D ) )
            inputVector.x = 1;
        if ( Input.GetKey( KeyCode.A ) )
            inputVector.x = -1;

        inputVector = inputVector.normalized;
        moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        transform.position += moveDir * moveSpeed * Time.deltaTime;
        if ( moveDir.x != 0f || moveDir.z != 0f )
            transform.forward = Vector3.Slerp( transform.forward, moveDir, Time.deltaTime * 10f );
    }
}
