using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scenes
    {
        MainMenuScene = 0,
        GameScene = 1,
        LoadingScene = 2
    }

    public static Scenes targetScene;

    public static void Load( Scenes target)
    {
        targetScene = target;

        SceneManager.LoadScene( ( int ) Scenes.LoadingScene );
        
    }

    public static void LoadActualScene( )
    {
        SceneManager.LoadScene( ( int ) targetScene );
    }
}
