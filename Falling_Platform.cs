using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Falling_Platform: MonoBehaviour
{
    [SerializeField] float fall_time = 0.5f, destory_time = 2f;
    Rigidbody2D rb;

    private void Start()
    {
        rb  = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject.tag == "Player" && collision.transform.position.y > transform.position.y)
        {
            Invoke("Fall_Platform", fall_time);
            Invoke("De_Active", destory_time);
        }
    }

    void Fall_Platform()
    {
        rb.isKinematic = false;
    }

    void De_Active()
    {
        gameObject.SetActive(false);
    }

}
