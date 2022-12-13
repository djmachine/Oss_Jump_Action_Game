using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class depoal : MonoBehaviour
{
    public float delta = 0.1f;
    public float max;
    private float a = 0;
    private float zerox;
    private float zeroy;
    BoxCollider2D collider;
    void Start()
    {
        zerox = transform.localPosition.x;
        zeroy = transform.localPosition.y;
        collider = GetComponent<BoxCollider2D>();
        a = delta;
    }

    // Update is called once per frame
    void Update()
    {
        float newXPosition = transform.localPosition.x + a;
        transform.localPosition = new Vector2(newXPosition, transform.localPosition.y);


        if (delta < 0f)
        {
            if (transform.localPosition.x < max)
            {
                collider.enabled = false;
                transform.localPosition = new Vector2(zerox, zeroy);
                a = 0;
                Invoke("Move", 0.5f);
            }
        }
        else
        {
            if (transform.localPosition.x > max)
            {
                collider.enabled = false;
                transform.localPosition = new Vector2(zerox, zeroy);
                a = 0;
                Invoke("Move", 0.5f);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D Circle)//공과 장애물 충돌
    {
        if (Circle.gameObject.tag == "Player")
        {
            collider.enabled = false;
            transform.localPosition = new Vector2(zerox, zeroy);
            a = 0;
            Invoke("Move", 0.5f);
        }
    }
    void Move()
    {
        collider.enabled = true;
        a = delta;
    }
}
