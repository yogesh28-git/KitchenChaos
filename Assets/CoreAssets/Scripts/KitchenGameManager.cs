using System;
using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using TMPro;

public class KitchenGameManager : NetworkBehaviour
{
    public static KitchenGameManager Instance { get; private set; }  

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    public event EventHandler OnLocalPlayerReadyChanged;

    public enum GameState
    {
        WAITING,
        COUNT_DOWN,
        GAMEPLAYING,
        GAMEOVER
    }

    private Dictionary<ulong, bool> playerReadyDictionary;

    private NetworkVariable<GameState> state = new NetworkVariable<GameState>(GameState.WAITING);
    private bool isLocalPlayerReady = false;

    private NetworkVariable<float> countDownTimer = new NetworkVariable<float>(3f);
    private NetworkVariable<float> gamePlayingTimer = new NetworkVariable<float>(0f);
    private float gamePlayingTimerMax = 10f;
    private bool isPaused = false;

    private void Awake( )
    {
        if(Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy( this.gameObject );
        }
    }

    private void Start( )
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;

        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public override void OnNetworkSpawn( )
    {
        state.OnValueChanged += State_OnValueChanged;
    }

    private void State_OnValueChanged( GameState previousValue, GameState newValue )
    {
        OnStateChanged?.Invoke( this, EventArgs.Empty );
    }

    private void GameInput_OnInteractAction( object sender, EventArgs e )
    {
        if(state.Value == GameState.WAITING )
        {
            isLocalPlayerReady = true;

            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);

            SetPlayerReadyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc( ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach ( ulong clientId in NetworkManager.Singleton.ConnectedClientsIds )
        {
            if( !playerReadyDictionary.ContainsKey( clientId ) || !playerReadyDictionary[clientId] )
            {
                allClientsReady = false;
            }
        }

        if ( allClientsReady )
        {
            state.Value = GameState.COUNT_DOWN;
        }
    }

    private void GameInput_OnPauseAction( object sender, EventArgs e )
    {
        TogglePause( );
    }

    private void Update( )
    {
        if ( !IsServer )
        {
            return;
        }

        switch(state.Value )
        {
            case GameState.WAITING:
                break;
            case GameState.COUNT_DOWN:
                countDownTimer.Value -= Time.deltaTime;
                if ( countDownTimer.Value < 0 )
                {
                    state.Value = GameState.GAMEPLAYING;
                    gamePlayingTimer.Value = gamePlayingTimerMax;
                }
                break;
            case GameState.GAMEPLAYING:
                gamePlayingTimer.Value -= Time.deltaTime;
                if ( gamePlayingTimer.Value < 0 )
                {
                    state.Value = GameState.GAMEOVER;
                }  
                break;
            case GameState.GAMEOVER:
                break;
        }
    }

    public bool isCountDownActive()
    {
        return state.Value == GameState.COUNT_DOWN;
    }
    public float GetCountDownTimer( ) => countDownTimer.Value; 

    public bool isGamePlaying( )
    {
        return state.Value == GameState.GAMEPLAYING;
    }

    public bool isGameOver( )
    {
        return state.Value == GameState.GAMEOVER;
    }

    public bool IsLocalPlayerReady( )
    {
        return isLocalPlayerReady;
    }

    public float GetGamePlayingTimerNormalized( )
    {
        return 1 - ( gamePlayingTimer.Value / gamePlayingTimerMax );
    }

    public void TogglePause( )
    {
        isPaused = !isPaused;

        if ( isPaused )
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke( this, EventArgs.Empty );
        }
    }
}
