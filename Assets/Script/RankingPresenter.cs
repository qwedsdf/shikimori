using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UniRx;

public class RankingPresenter : MonoBehaviour
{
    [SerializeField]
    private RankingItem _rankItemPrefab;

    [SerializeField]
    private Transform _rankingScrollParent;

    [SerializeField]
    private Button _returnButton;

    [SerializeField]
    private RankingItem _playerRankItem;

    private List<RankingUserDataParameter> _playerRankParamList = new List<RankingUserDataParameter>();
    private List<RankingItem> _rankingItemList = new List<RankingItem>();

    void Start()
    {
        // GetUserData().Forget();

        Initialize();
    }

    private void Initialize()
    {
        var sort = new SortMaxClearCount();
        CreateRanking(sort).Forget();

        _returnButton.OnClickAsObservable()
            .Subscribe(_ => SceneLoadManager.Instance.LoadScene(Scenes.Home))
            .AddTo(this);
    }

    /// <summary>
    /// ランキングを作成
    /// </summary>
    /// <param name="sort"></param>
    private async UniTask CreateRanking(ISort sort){
        ResetRanking();
        await SetSortedUserData(sort);
        SetPlayerRankingData();
    }

    /// <summary>
    /// ユーザーデータをＤＢから取得
    /// </summary>
    /// <returns></returns>
    private async UniTask<List<UserData>> GetUserData()
    {
        var list = await AppApi.GetRankUserData();

        foreach (var d in list)
        {
            Debug.Log($"Name:{d.Name} 最大勝利数:{d.MaxClearCount}");
        }

        return list;
    }

    /// <summary>
    /// ソートさせる
    /// </summary>
    /// <param name="sort"></param>
    private async UniTask SetSortedUserData(ISort sort)
    {
        var list = await GetUserData();
        var sortedList = sort.GetSortedUserData(list);

        foreach (var (user, index) in sortedList.Select((user, index) => (user, index)))
        {
            var param = new RankingUserDataParameter(){
                // 「0」スタートのため「+1」する
                Rank = index + 1,
                Name = user.Name,
                Value = user.MaxClearCount,
                UserId = user.UserId
            };
            _playerRankParamList.Add(param);
            
            CreateRankingItem(param);
        }
    }

    /// <summary>
    /// ランキングアイテムの作成
    /// </summary>
    /// <param name="userData">表示するアイテム</param>
    /// <param name="rank">ランキング順位</param>
    private void CreateRankingItem(RankingUserDataParameter param)
    {
        var rankItem = _rankingItemList.FirstOrDefault(item => !item.gameObject.activeSelf);
        if (rankItem == null)
        {
            rankItem = Instantiate(_rankItemPrefab, Vector3.zero, Quaternion.identity, _rankingScrollParent) as RankingItem;
            _rankingItemList.Add(rankItem);
        }

        rankItem.SetRankingItem(param);
        rankItem.gameObject.SetActive(true);
    }

    /// <summary>
    /// ランキングの状態をリセット
    /// </summary>
    private void ResetRanking()
    {
        _rankingItemList.ForEach(item => item.gameObject.SetActive(false));
        _playerRankParamList.Clear();
    }

    private void SetPlayerRankingData()
    {
        var playerData = RunTimeData.PlayerData;
        var userRank = _playerRankParamList.FirstOrDefault(param => param.UserId == playerData.UserId);

        if(userRank == null){
            Debug.LogError("ランキングにユーザーデータがありません。");
        }

        _playerRankItem.SetRankingItem(userRank);
        _playerRankItem.gameObject.SetActive(true);
    }
}
