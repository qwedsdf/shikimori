using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;
using UniRx.Triggers;

public class Bullet : MonoBehaviour
{
    private static readonly float DEFENCE_POWER = 1000f;
    private static readonly string CUT_SE_NAME = "hajiku_01";
    private static readonly string THROW_SE_NAME = "nageru";
    private bool _isCut = false;
    public bool IsCut => _isCut;
    public bool IsOutFrame => _spriteRenderer.isVisible;
    public bool IsStart = false;
    private Subject<bool> _settlementSubject = new Subject<bool>();
    public IObservable<bool> OnSettlement => _settlementSubject.AsObservable();
    public bool IsUseObj { set; get; } = false;

    public IObservable<string> OnTriggerHome => this.OnTriggerEnter2DAsObservable()
            .Select(collision => collision.tag)
            .Where(tag => tag == "Home");

    public CompositeDisposable CompositeDisposable = new CompositeDisposable();



    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        SoundManager.Instance.PlaySE(THROW_SE_NAME);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_isCut || CheckStop() || !IsStart) return;
        var speed = GameManager.Instance.Speed;
        var force = new Vector2(speed, 0);
        rb.velocity = force;
    }

    private bool CheckStop()
    {
        if (GameManager.Instance.IsBreakHome)
        {
            rb.velocity = Vector2.zero;
            return true;
        }

        return false;
    }

    public void Initialize()
    {
        _isCut = false;
        IsUseObj = true;
        IsStart = false;
    }

    public void Reset()
    {
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        IsUseObj = false;
    }

    public void OnTap()
    {
        if (_isCut)
        {
            return;
        }
        var tapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Camera.main.transform.forward);
        var direction = GameManager.Instance.CacheTapPos.y - transform.position.y;
        _isCut = true;

        Vector2 force = new Vector2();

        if (direction > 0)
        {
            force = new Vector2(1, -1);
        }
        else
        {
            force = new Vector2(1, 1);
        }


        rb.AddForceAtPosition(force.normalized * DEFENCE_POWER, tapPos);

        SoundManager.Instance.PlaySE(CUT_SE_NAME);

        _settlementSubject.OnNext(true);
    }
}
