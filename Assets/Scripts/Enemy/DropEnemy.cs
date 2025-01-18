using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEnemy : Enemy
{
    public float startSize;
    public Collider2D[] cs;

    public SpriteRenderer[] sprites;

    protected override void Awake()
    {
        base.Awake();
        foreach (var c in cs)
        {
            c.enabled = false;
        }
    }

    protected enum State
    {
        dropping,
        floating,
        stop,
    }

    protected State state;

    float timmer = 0;
    protected virtual void Update()
    {
        switch (state)
        {
            case State.dropping:
                timmer += Time.deltaTime;
                transform.localScale = Vector3.one * (Mathf.Cos(Mathf.PI * timmer) / (1f + Mathf.Pow(timmer, 3)) / 2 + 1);
                foreach (var s in sprites)
                {
                    s.color = new(0.2f+timmer/0.5f*0.8f , 0.2f+timmer/0.5f*0.8f ,0.2f+timmer/0.5f*0.8f );
                }
                if (timmer > 0.5f)
                {
                    state = State.floating;
                    foreach (var c in cs)
                    {
                        c.enabled = true;
                    }
                    foreach (var s in sprites)
                    {
                        s.color = new(1, 1, 1);
                    }
                }
                break;
            case State.floating:
                timmer += Time.deltaTime;
                transform.localScale = Vector3.one * (Mathf.Cos(Mathf.PI * timmer) / (1f + Mathf.Pow(timmer+0.5f, 3)) / 2 + 1);
                if (timmer > 4.5f)
                {
                    state = State.stop;
                }
                break;
            case State.stop:
                transform.localScale = Vector3.one;
                break;
        }
    }
}