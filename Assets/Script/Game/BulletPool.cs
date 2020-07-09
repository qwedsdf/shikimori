using System.Collections;
using System.Collections.Generic;
using UniRx.Toolkit;
using UnityEngine;

public class BulletPool : ObjectPool<Bullet>
{
    private readonly Bullet _prefab;
    private readonly Transform _parentTransform;

    public BulletPool(Transform parentTransform, Bullet prefab)
    {
        _prefab = prefab;
        _parentTransform = parentTransform;
    }

    /// <summary>
    /// オブジェクトの追加生成時に実行される
    /// </summary>
    protected override Bullet CreateInstance()
    {
        //新しく生成
        var obj = GameObject.Instantiate(_prefab);

        //ヒエラルキーが散らからないように一箇所にまとめる
        obj.transform.SetParent(_parentTransform);

        return obj;
    }

    protected override void OnBeforeReturn(Bullet instance)
    {
        instance.Reset();
        base.OnBeforeReturn(instance);
    }
}
