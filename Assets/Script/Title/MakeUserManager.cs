using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;
using UniRx.Async;

public class MakeUserManager : MonoBehaviour
{
    [SerializeField]
    private InputField _inputName;

    [SerializeField]
    private Button _matchingButton;

    void Start()
    {
        Initialize();
        BindEvent();
    }

    private void Initialize() {
        LoadUserData();
    }

    private void LoadUserData(){
        var key = UserData.USER_HASH_KEY;
        var json = PlayerPrefs.GetString(key);
        var data = JsonUtility.FromJson<UserDataParamater>(json);
        _inputName.text = data?.Name;
    }

    private void BindEvent() {
        _matchingButton.OnClickAsObservable()
            .Subscribe(_ => { 
                SetUserData().Forget();
            })
            .AddTo(this);
    }

    private async UniTask SetUserData() {
        var userData = new UserData() {
            Name = _inputName.text,
        };

        var userHash = await AppApi.CreateUserData(userData);
        var key = UserData.USER_HASH_KEY;
        PlayerPrefs.SetString(key,userHash);
        PlayerPrefs.Save();
        RunTimeData.PlayerData = userData;
        SceneLoadManager.Instance.LoadScene(Scenes.Game);
    }
}
