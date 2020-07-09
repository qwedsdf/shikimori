using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    Title,
    MakeUser,
    Home,
    Game,
    Ranking,
    Data,
}

public class SceneLoadManager : SingletonMonoBehaviour<SceneLoadManager>
{
    private IReadOnlyDictionary<Scenes, string> _sceneNameDic = new Dictionary<Scenes, string>(){
        { Scenes.Title,"Title" },
        { Scenes.MakeUser,"MakeUser" },
        { Scenes.Home, "Home" },
        { Scenes.Game, "Game" },
        { Scenes.Ranking, "Ranking" },
        { Scenes.Data, "Data" },
    };

    private IReadOnlyDictionary<Scenes, string> _sceneBGMDic = new Dictionary<Scenes, string>(){
        { Scenes.Title,"Title" },
        { Scenes.MakeUser,"Title" },
        { Scenes.Home,"Home" },
        { Scenes.Game,"Game" },
        { Scenes.Ranking,"Title" },
    };

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadScene(Scenes scene,LoadSceneMode mode = LoadSceneMode.Single)
    {
        var sceneName = _sceneNameDic[scene];
        SoundManager.Instance.ChangeBGM(_sceneBGMDic[scene]);
        SceneManager.LoadScene(sceneName,mode);
    }
}
