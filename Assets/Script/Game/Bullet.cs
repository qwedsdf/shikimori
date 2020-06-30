using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class Bullet : MonoBehaviour
{
    private static readonly float BASE_SPEED = -5f;
    private static readonly float DEFENCE_POWER = 1000f;
    private bool isCut = false;
    private Subject<bool> _settlementSubject = new Subject<bool>();
    public IObservable<bool> OnSettlement => _settlementSubject.AsObservable();
    

    [SerializeField]
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (isCut) return;
        var force = new Vector2(BASE_SPEED, 0);
        rb.AddForce(force);
    }

    public void OnTap()
    {
        var tapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Camera.main.transform.forward);
        var direction = GameManager.Instance.CacheTapPos.y - transform.position.y;
        isCut = true;

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

        _settlementSubject.OnNext(true);
    }
}
