using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


public class ShikimoriButton : MonoBehaviour
{
    [SerializeField]
    private string NORMAL_SE = "Decision";
    private AudioSource _audioSource;
    private Button _button;

    void Start()
    {
        _button = GetComponent<Button>();
        _button.OnClickAsObservable().Subscribe(_ => SoundManager.Instance.PlaySE(NORMAL_SE)).AddTo(this);
    }
}
