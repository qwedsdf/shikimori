using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Async;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private Bullet _bullet;
    public Vector2 CacheTapPos { private set; get; }
    public bool IsTapmiddle { private set; get; } = false;

    private void Start()
    {
        _bullet.OnSettlement.Subscribe(isWin => Settlement(isWin).Forget());
    }

    private void LateUpdate()
    {
        SetCuurentTapPos();
    }

    private async UniTask Settlement(bool isWin)
    {
        if(isWin){
            RunTimeData.PlayerData.WinCount++;
            Debug.Log("勝ち");
        } else {
            RunTimeData.PlayerData.LoseCount++;
        }

        await AppApi.SaveUserData(RunTimeData.PlayerData);
    }

    private void SetCuurentTapPos()
    {
#if !UNITY_EDITOR
        if (Input.touchCount > 0)
        {
            CacheTapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Camera.main.transform.forward);
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                IsTapmiddle = true;
                return;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                IsTapmiddle = false;
                return;
            }
        }
#else
        CacheTapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Camera.main.transform.forward);
#endif

    }
}
