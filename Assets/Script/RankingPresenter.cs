using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class RankingPresenter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetUserData().Forget();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private async UniTask GetUserData()
    {
        var data = await AppApi.GetRankUserData();

        foreach(var d in data){
            Debug.Log($"Name:{d.Name} 最大勝利数:{d.MaxClearCount}");
        }
    }
}
