using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingUserDataParameter{
    public int UserId;
    public int Rank;
    public string Name;
    public int Value;
}

public class RankingItem : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _rankText;
    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private TextMeshProUGUI _valueText;

    public void SetRankingItem(RankingUserDataParameter param)
    {
        _rankText.text = param.Rank.ToString();
        _nameText.text = param.Name;
        _valueText.text = param.Value.ToString();
    }    
}
