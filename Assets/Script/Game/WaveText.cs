using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public class WaveText : MonoBehaviour
{
    private static readonly float MOVE_SPEED = 1000f;
    private static readonly float FIRST_POS = 1111f;
    private static readonly float END_POS = -999f;
    private static readonly int WAIT_TIME = 1000;

    [SerializeField]
    private RectTransform _rectTrans;


    void OnEnable()
    {
        Animation().Forget();
    }

    private async UniTask Animation()
    {
        _rectTrans.anchoredPosition = new Vector2(FIRST_POS, 0);

        // 真ん中に来るまで繰り返す
        while (_rectTrans.anchoredPosition.x > 0)
        {
            Move();

            await UniTask.DelayFrame(1);
        }

        await UniTask.Delay(WAIT_TIME);

        // 真ん中に来るまで繰り返す
        while (_rectTrans.anchoredPosition.x > END_POS)
        {
            Move();
            await UniTask.DelayFrame(1);
        }

        this.gameObject.SetActive(false);
    }

    private void Move()
    {
        var pos = _rectTrans.anchoredPosition;
        pos.x -= MOVE_SPEED * Time.deltaTime;
        _rectTrans.anchoredPosition = pos;
    }
}
