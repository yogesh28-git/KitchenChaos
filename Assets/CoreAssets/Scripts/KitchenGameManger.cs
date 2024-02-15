using System;
using UnityEngine;

public class KitchenGameManger : MonoBehaviour
{
    public static KitchenGameManger Instance { get; private set; }  

    public event EventHandler OnStateChanged;
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
    private float gamePlayingTimer = 10f;

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
}
