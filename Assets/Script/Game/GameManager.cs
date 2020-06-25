using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Left,
    Right,
    Down,
    Up,
}

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public static Direction CrossDirection;
    public Vector2 CacheTapPos { private set; get; }
    public bool IsTapmiddle { private set; get; } = false;

    private void LateUpdate()
    {
        SetCuurentTapPos();
    }

    private void SetCuurentTapPos()
    {
#if !UNITY_EDITOR
        if (Input.touchCount > 0)
        {
            CacheTapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Camera.main.transform.forward);
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                IsTapmiddle = true;
                return;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                IsTapmiddle = false;
                return;
            }
        }
#else

        CacheTapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Camera.main.transform.forward);
#endif

    }
}
