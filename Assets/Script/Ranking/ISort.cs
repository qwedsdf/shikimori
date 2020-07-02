using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISort
{
    string GetRankingTitle();
    List<UserData> GetSortedUserData(List<UserData> userDataList);
}
