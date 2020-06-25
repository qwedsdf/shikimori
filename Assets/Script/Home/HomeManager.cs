using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class HomeManager : MonoBehaviour
{
    [SerializeField]
    private Button _GoToGameButton;
    [SerializeField]
    private Button _GoToRankButton;

    private void Start()
    {
        BindEvent();
    }

    private void BindEvent()
    {
        _GoToGameButton.OnClickAsObservable()
            .Subscribe(_ => SceneLoadManager.Instance.LoadScene(Scenes.Game))
            .AddTo(this);

        _GoToRankButton.OnClickAsObservable()
            .Subscribe(_ => SceneLoadManager.Instance.LoadScene(Scenes.Ranking))
            .AddTo(this);
    }
}
