using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderManager : MonoBehaviour
{
    [SerializeField]
    private Text _nameText;

    private void Start(){
        _nameText.text = RunTimeData.PlayerData.Name;
    }
}
