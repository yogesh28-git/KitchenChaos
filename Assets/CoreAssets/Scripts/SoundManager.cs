using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioReferencesSO audioRefSO;

    private void Awake( )
    {
        if(Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Start( )
    {
        DeliveryManager.Instance.OnDeliveryFailed += DeliveryManager_OnDeliveryFailed;
        DeliveryManager.Instance.OnDeliverySuccess += DeliveryManager_OnDeliverySuccess;

        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickUpSomething += Player_OnPickUpSomething;
        BaseCounter.OnAnyDrop += BaseCounter_OnAnyDrop;
        TrashCounter.OnAnyTrashed += TrashCounter_OnAnyTrashed;
    }

    private void TrashCounter_OnAnyTrashed( object sender, System.EventArgs e )
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySFX( audioRefSO.trash, trashCounter.transform.position );
    }

    private void BaseCounter_OnAnyDrop( object sender, System.EventArgs e )
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySFX( audioRefSO.foodDrop, baseCounter.transform.position );
    }

    private void Player_OnPickUpSomething( object sender, System.EventArgs e )
    {
        PlaySFX( audioRefSO.foodPickup, Player.Instance.transform.position );
    }

    private void CuttingCounter_OnAnyCut( object sender, System.EventArgs e )
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySFX( audioRefSO.chop,  cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnDeliverySuccess( object sender, System.EventArgs e )
    {
        DeliveryCounter deliveryCounter = sender as DeliveryCounter;
        PlaySFX(audioRefSO.deliverySuccess, deliveryCounter.transform.position );
    }

    private void DeliveryManager_OnDeliveryFailed( object sender, System.EventArgs e )
    {
        DeliveryCounter deliveryCounter = sender as DeliveryCounter;
        PlaySFX( audioRefSO.deliveryFail, deliveryCounter.transform.position);
    }

    public void PlayerFootStepsSound( Vector3 playerPosition, float volume)
    {
        PlaySFX(audioRefSO.footStep, playerPosition, volume);
    }

    private void PlaySFX(AudioClip[] clipArray, Vector3 position, float volume = 1f )
    {
        PlaySFX( clipArray[Random.Range( 0, clipArray.Length )], position );
    }
    private void PlaySFX( AudioClip clip, Vector3 position, float volume = 1f )
    {
        AudioSource.PlayClipAtPoint( clip, position );
    }
}
