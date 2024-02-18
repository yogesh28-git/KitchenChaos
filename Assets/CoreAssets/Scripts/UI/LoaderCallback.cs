using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;

    private void Update( )
    {
        if(isFirstUpdate )
        {
            SceneLoader.LoadActualScene( );
            isFirstUpdate = false;
        }
    }
}
