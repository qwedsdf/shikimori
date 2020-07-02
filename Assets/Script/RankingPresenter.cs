using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using System.Linq;

public class RankingPresenter : MonoBehaviour
{
    [SerializeField]
    private RankingItem _rankItemPrefab;

    [SerializeField]
    private Transform _rankingScrollParent;

    private List<RankingItem> _rankingItemList = new List<RankingItem>();

    void Start()
    {
        // GetUserData().Forget();
        var sort = new SortMaxClearCount();
        SetSortedUserData(sort).Forget();
    }

    private async UniTask<List<UserData>> GetUserData()
    {
        var list = await AppApi.GetRankUserData();

        foreach (var d in list)
        {
            Debug.Log($"Name:{d.Name} 最大勝利数:{d.MaxClearCount}");
        }

        return list;
    }

    private async UniTask SetSortedUserData(ISort sort)
    {
        ResetItem();
        var list = await GetUserData();
        var sortedList = sort.GetSortedUserData(list);

        foreach (var (user, index) in sortedList.Select((user, index) => (user, index))){
            // 「0」スタートのため「+1」する
            CreateRankingItem(user,index + 1);
        }
    }

    private void CreateRankingItem(UserData userData,int rank){
        var rankItem = _rankingItemList.FirstOrDefault(item => !item.gameObject.activeSelf);
        if(rankItem == null){
            rankItem = Instantiate(_rankItemPrefab, Vector3.zero, Quaternion.identity, _rankingScrollParent) as RankingItem;
            _rankingItemList.Add(rankItem);
        }
        var param = new RankingUserDataParameter(){
            Rank = rank,
            Name = userData.Name,
            Value = userData.MaxClearCount,
        };

        rankItem.SetRankingItem(param);
        rankItem.gameObject.SetActive(true);
    }

    private void ResetItem(){
        _rankingItemList.ForEach(item => item.gameObject.SetActive(false));
    }
}
