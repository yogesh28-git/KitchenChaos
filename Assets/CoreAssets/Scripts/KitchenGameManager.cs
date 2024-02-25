using System;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance { get; private set; }  

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    public enum GameState
    {
        WAITING,
        COUNT_DOWN,
        GAMEPLAYING,
        GAMEOVER
    }
    private GameState state;

    private float waitingTimer = 2f;
    private float countDownTimer = 3f;
    private float gamePlayingTimer;
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
       
        state = GameState.WAITING;
    }

    private void Start( )
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnPauseAction( object sender, EventArgs e )
    {
        TogglePause( );
    }

    private void Update( )
    {
        switch(state )
        {
            case GameState.WAITING:
                waitingTimer -= Time.deltaTime;
                if ( waitingTimer < 0 )
                {
                    state = GameState.COUNT_DOWN;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.COUNT_DOWN:
                countDownTimer -= Time.deltaTime;
                if ( countDownTimer < 0 )
                {
                    state = GameState.GAMEPLAYING;
                    OnStateChanged?.Invoke( this, EventArgs.Empty );
                    gamePlayingTimer = gamePlayingTimerMax;
                }
                break;
            case GameState.GAMEPLAYING:
                gamePlayingTimer -= Time.deltaTime;
                if ( gamePlayingTimer < 0 )
                {
                    state = GameState.GAMEOVER;
                    OnStateChanged?.Invoke( this, EventArgs.Empty );
                }  
                break;
            case GameState.GAMEOVER:
                break;
        }
    }

    public bool isCountDownActive()
    {
        return state == GameState.COUNT_DOWN;
    }
    public float GetCountDownTimer( ) => countDownTimer; 

    public bool isGamePlaying( )
    {
        return state == GameState.GAMEPLAYING;
    }

    public bool isGameOver( )
    {
        return state == GameState.GAMEOVER;
    }

    public float GetGamePlayingTimerNormalized( )
    {
        return 1 - ( gamePlayingTimer / gamePlayingTimerMax );
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
