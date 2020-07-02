using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortMaxClearCount : ISort
{

    public string GetRankingTitle(){
        return "最大クリア数ランキング";
    }

    public List<UserData> GetSortedUserData(List<UserData> userDataList){
        userDataList.Sort((a,b) => b.MaxClearCount - a.MaxClearCount);
        return userDataList;
    }
}
