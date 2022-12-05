using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float PlayerSpeed;
    public float JumpPowr;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
   
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move(); // �÷��̾� �̵� �Լ�
        Jump(); // �÷��̾� ���� �Լ�
        
    }

    private void FixedUpdate()
    {
        // Platfrom�� �÷��̾ �������, ������ �����ϰ� ���ִ� ���
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if(rayHit.collider != null)
            {
                if(rayHit.distance < 0.5f)
                {
                    anim.SetBool("IsJumping", false);
                }
            }
        }
    }
    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;
            spriteRenderer.flipX = true;
            anim.SetBool("IsWalking", true);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;
            spriteRenderer.flipX = false;
            anim.SetBool("IsWalking", true);
        }

        else if (Input.GetAxisRaw("Horizontal") == 0)
        {
            anim.SetBool("IsWalking", false);
        }

        transform.position += moveVelocity * PlayerSpeed * Time.deltaTime;
    }

    void Jump(){

        if (Input.GetKeyDown(KeyCode.Space)&&!anim.GetBool("IsJumping"))
        {
            rigid.AddForce(Vector2.up * JumpPowr, ForceMode2D.Impulse);
            anim.SetBool("IsJumping", true);
        }

    }

}
