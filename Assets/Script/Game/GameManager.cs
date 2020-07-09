using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Async;
using UniRx.Triggers;
using System.Linq;
using System;
using TMPro;

public class WaveInfo
{
    public float SpeedRate = 1f;
    public int ArrowCount = 3;
}

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private static readonly float BASE_SPEED = -50f;
    public float Speed { set;get; } = BASE_SPEED;
    [SerializeField]
    private Bullet _bullet;
    [SerializeField]
    private Transform _bulletParent;

    [SerializeField]
    private TextMeshProUGUI _waveText;
    [SerializeField]
    private Transform _respownTrans;
    [SerializeField]
    private Rigidbody2D _respownRb;
    [SerializeField]
    private Transform _firstTrans;
    private int _currentWaveCount = 0;
    private WaveInfo _currentWaveInfo;
    private BulletPool _bulletPool;
    private List<Bullet> _currentBulletList = new List<Bullet>();
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    public Vector2 CacheTapPos { private set; get; }
    public bool IsTapmiddle { private set; get; } = false;
    public bool IsBreakHome { set; get; } = false;

    private void Start()
    {
        _bulletPool = new BulletPool(_bulletParent, _bullet);
        _currentWaveInfo = new WaveInfo();
        IsBreakHome = false;
        _bulletPool.PreloadAsync(5, 3).Subscribe();
        Initialize();
    }

    private async UniTask MainLoop()
    {
        // 家が壊れるまで続ける
        while (!IsBreakHome)
        {
            _waveText.text = $"{_currentWaveCount} Wave";
            _waveText.gameObject.SetActive(true);
            await UniTask.WaitWhile(() => _waveText.gameObject.activeSelf);
            var time = UnityEngine.Random.Range(3,10);
            await CreateBullet(8);
            await UniTask.WaitWhile(() => _currentBulletList.Any(bullet => !bullet.IsCut) && !IsBreakHome);
            _currentBulletList.ForEach(bullet => bullet.Reset());
            _currentWaveCount++;
        }
    }

    private void LateUpdate()
    {
        SetCuurentTapPos();
    }

    private void Initialize()
    {
        MainLoop().Forget();
    }

    /// <summary>
    /// 弾生成
    /// </summary>
    /// <param name="waveCount">弾数</param>
    private async UniTask CreateBullet(int waveCount)
    {
        // リスポーン地点用のオブジェクトを初期化
        _respownTrans.position = _firstTrans.position;
        _compositeDisposable.Clear();
        this.UpdateAsObservable().Subscribe(_ => {
                var force = new Vector2(-Speed, 0);
                _respownRb.velocity = force;
            }).AddTo(_compositeDisposable);

        _currentBulletList.Clear();
        for (int i = 0; i < waveCount; i++)
        {
            var time = UnityEngine.Random.Range(100, 700);
            await UniTask.Delay(time);
            var bullet = _bulletPool.Rent();
            bullet.Initialize();
            _currentBulletList.Add(bullet);
            bullet.transform.position = _respownTrans.position;

            // 家が壊れた判定
            bullet.OnTriggerHome.Subscribe(_ => IsBreakHome = true).AddTo(bullet.CompositeDisposable);

            // 帰還処理だが、レンタルするたびに購読されるのであとで直す
            bullet.UpdateAsObservable()
                .Where(_ => bullet.IsCut)
                .Where(_ => bullet.IsOutFrame)
                .Delay(TimeSpan.FromMilliseconds(1000))
                .Subscribe(_ => {
                    bullet.CompositeDisposable.Clear();
                    _bulletPool.Return(bullet);
                    })
                .AddTo(bullet.CompositeDisposable);
        }

        _currentBulletList.ForEach(bullet => bullet.IsStart = true);
    }

    private async UniTask Settlement(bool isWin)
    {
        if (isWin)
        {
            RunTimeData.PlayerData.WinCount++;
            Debug.Log("勝ち");
        }
        else
        {
            RunTimeData.PlayerData.LoseCount++;
        }

        await AppApi.SaveUserData(RunTimeData.PlayerData);

        await UniTask.Delay(3000);

        SceneLoadManager.Instance.LoadScene(Scenes.Home);
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
