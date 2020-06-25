using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Async;

public class TitlePresenter : MonoBehaviour
{
    [SerializeField]
    private TitleView _view;

    private void Start() {
        Initialize();
    }

    private void Initialize() {
        BindEvent();
    }

    private void BindEvent() {
        _view.OnClickChangeSceneButton.Subscribe(_ => ChangeScene()).AddTo(this);
    }

    private void ChangeScene() {
        var hasKey = PlayerPrefs.HasKey(UserData.USER_HASH_KEY);
        if(!hasKey) {
            // ユーザーデータがないときの処理
            SceneLoadManager.Instance.LoadScene(Scenes.MakeUser);
            return;
        }
        
        // ユーザーデータがある時の処理
        LoadPlayerData().Forget();
    }

    private async UniTask LoadPlayerData() {
        var userHash = PlayerPrefs.GetString(UserData.USER_HASH_KEY);
        RunTimeData.PlayerData = await AppApi.GetUserData(userHash);
        SceneLoadManager.Instance.LoadScene(Scenes.Home);
    }
}
