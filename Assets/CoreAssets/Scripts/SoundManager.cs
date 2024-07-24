using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioReferencesSO audioRefSO;
    [SerializeField] private AudioSource musicAudioSource;

    private float volumeMultiplier = 0.1f;
    private float musicVolume = 0.1f;

    private const string SFX_VOL = "SFX_VOL";
    private const string MUSIC_VOL = "MUSIC_VOL";

    private void Awake( )
    {
        if ( Instance == null )
        {
            Instance = this;
            DontDestroyOnLoad( this.gameObject );
        }
        else
        {
            Destroy( this.gameObject );
        }

        InitializeVolume( );
    }

    public void Start( )
    {
        DeliveryManager.OnDeliveryFailed += DeliveryManager_OnDeliveryFailed;
        DeliveryManager.OnDeliverySuccess += DeliveryManager_OnDeliverySuccess;

        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.OnPickUpSomething += Player_OnPickUpSomething;
        BaseCounter.OnAnyDrop += BaseCounter_OnAnyDrop;
        TrashCounter.OnAnyTrashed += TrashCounter_OnAnyTrashed;
    }

    private void InitializeVolume( )
    {
        musicVolume = PlayerPrefs.GetFloat( MUSIC_VOL, musicVolume );
        musicAudioSource.volume = musicVolume;
        volumeMultiplier = PlayerPrefs.GetFloat( SFX_VOL, volumeMultiplier );
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
        Player player = sender as Player;
        PlaySFX( audioRefSO.foodPickup, player.transform.position );
    }

    private void CuttingCounter_OnAnyCut( object sender, System.EventArgs e )
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySFX( audioRefSO.chop, cuttingCounter.transform.position );
    }

    private void DeliveryManager_OnDeliverySuccess( object sender, System.EventArgs e )
    {
        PlaySFX( audioRefSO.deliverySuccess, DeliveryCounter.Instance.transform.position );
    }

    private void DeliveryManager_OnDeliveryFailed( object sender, System.EventArgs e )
    {
        PlaySFX( audioRefSO.deliveryFail, DeliveryCounter.Instance.transform.position );
    }

    public void PlayerFootStepsSound( Vector3 playerPosition, float volume )
    {
        PlaySFX( audioRefSO.footStep, playerPosition, volume );
    }
    public void PlayCountDownSound( )
    {
        PlaySFX( audioRefSO.warning, Camera.main.transform.position );
    }
    public void PlayWarningSound( Vector3 position )
    {
        PlaySFX( audioRefSO.warning, position );
    }

    private void PlaySFX( AudioClip[] clipArray, Vector3 position, float volume = 1f )
    {
        PlaySFX( clipArray[UnityEngine.Random.Range( 0, clipArray.Length )], position, volume );
    }
    private void PlaySFX( AudioClip clip, Vector3 position, float volume = 1f )
    {
        AudioSource.PlayClipAtPoint( clip, position, volume * volumeMultiplier );
    }

    public void ChangeSoundEffectsVolume( )
    {
        volumeMultiplier += 0.1f;

        if ( volumeMultiplier > 1.05 )
        {
            volumeMultiplier = 0;
        }

        PlayerPrefs.SetFloat( SFX_VOL, volumeMultiplier );
    }

    public void ChangeMusicVolume( )
    {
        musicVolume += 0.1f;

        if ( musicVolume > 1.05 )
        {
            musicVolume = 0;
        }

        musicAudioSource.volume = musicVolume;

        PlayerPrefs.SetFloat( MUSIC_VOL, musicVolume );
    }

    public float GetSFXVolume( )
    {
        return volumeMultiplier;
    }
    public float GetMusicVolume( )
    {
        return musicVolume;
    }
}
