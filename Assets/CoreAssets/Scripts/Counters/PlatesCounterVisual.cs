using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform plateVisualObject;
    [SerializeField] private PlatesCounter platesCounter;

    private Transform[] platesArray;
    private int plateCapacity = 4;
    private int plateCount = 0;

    private void Start( )
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;

        platesArray = new Transform[plateCapacity];
        float OffsetY = 0.1f;
        for(int i=0; i<plateCapacity; i++ )
        {
            platesArray[i] = Instantiate( plateVisualObject, platesCounter.GetKitchenObjectFollowTransform() );
            platesArray[i].position = platesCounter.GetKitchenObjectFollowTransform( ).position + Vector3.up * OffsetY * i;
            platesArray[i].gameObject.SetActive( false );
        }
    }
    private void PlatesCounter_OnPlateSpawned( object sender, System.EventArgs e )
    {
        platesArray[plateCount].gameObject.SetActive( true );
        plateCount++;
    }
    private void PlatesCounter_OnPlateRemoved( object sender, System.EventArgs e )
    {
        plateCount--;
        platesArray[plateCount].gameObject.SetActive ( false );
    }

}
