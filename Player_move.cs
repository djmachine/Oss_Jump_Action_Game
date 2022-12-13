using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_move : MonoBehaviour
{
    public GameManager gamemanager;
    public float maxspeed;
    public float jumppower;
    Rigidbody2D rigid;
    SpriteRenderer spriterenderer;
    Animator animator;
    CapsuleCollider2D capsulecollider;
    public float depopower;

    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;

    AudioSource audioSource;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriterenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        capsulecollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //점프
        if (Input.GetButtonDown("Jump") && !animator.GetBool("is_jump"))
        {
            //점프 동력
            rigid.AddForce(Vector2.up * jumppower, ForceMode2D.Impulse);
            //점프 애니메이션
            animator.SetBool("is_jump", true);

            Play_Sound("JUMP");

        }

        //멈추는 속도
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.01f, rigid.velocity.y);
        }

        //방향 전환
        if (Input.GetButton("Horizontal"))
        {
            spriterenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        //멈춤과 움직임 애니메이션
        if (Mathf.Abs(rigid.velocity.x) < 0.3f)
        {
            animator.SetBool("is_walk", false); //속도가 0.3보다 작으면 is_walk OFF
        }
        else
        {
            animator.SetBool("is_walk", true); //속도가 0.3보다 크면 is_walk ON
        }
    }
    void FixedUpdate()
    {
        //움직임 속도
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //최대 속도 설정

        if (rigid.velocity.x > maxspeed) //오른쪽 최대 속도
        {
            rigid.velocity = new Vector2(maxspeed, rigid.velocity.y);
        }

        else if (rigid.velocity.x < maxspeed * (-1)) //왼쪽 최대 속도
        {
            rigid.velocity = new Vector2(maxspeed * (-1), rigid.velocity.y);
        }

        //착지
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 2));

            RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));


            if (rayhit.collider != null)
            {
                if (rayhit.distance < 0.5f)
                {
                    animator.SetBool("is_jump", false);
                }
            }

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {   //몬스터 공격
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                On_Attack(collision.transform);
            }
            else //데미지 입음
            {
                On_Damaged(collision.transform.position);
            }
        }

        else if (collision.gameObject.tag == "Spike")
        {
            On_Damaged(collision.transform.position);
        }

        else if (collision.gameObject.tag == "Depoal")
        {
            on_Back(collision.transform.position);
        }
    }


    void OnTriggerEnter2D(Collider2D collision) //동전, 깃발 인스펙터에서 is_trigger 체크
    {
        if (collision.gameObject.tag == "Item")
        {
            Play_Sound("ITEM");
            //점수

            bool is_gold = collision.gameObject.name.Contains("gold");
            if (is_gold)
                gamemanager.stagepoint += 200;

            bool is_silver = collision.gameObject.name.Contains("silver");
            if (is_silver)
                gamemanager.stagepoint += 100;

            bool is_bronze = collision.gameObject.name.Contains("bronze");
            if (is_bronze)
                gamemanager.stagepoint += 50;

            //동전 사라지게
            collision.gameObject.SetActive(false);
        }

        else if (collision.gameObject.tag == "Finish")
        {
            //다음 단계로 넘어감
            gamemanager.Next_Stage();

            Play_Sound("FINISH");
        }

    }


    void On_Attack(Transform enemy)
    {
        //점수
        gamemanager.stagepoint += 100;

        //플레이어 점프 물리
        rigid.AddForce(Vector2.up * 15, ForceMode2D.Impulse);

        Enemy_move enemy_Move = enemy.GetComponent<Enemy_move>();
        enemy_Move.Enemy_Damaged();

        Play_Sound("ATTACK");
    }

    void on_Back(Vector2 targetpos)
    {
        int direct = transform.position.x - targetpos.x > 0 ? 1 : -1;

        //충돌물리 구현
        rigid.AddForce(new Vector2(direct, 0) * 15, ForceMode2D.Impulse);
    }



    void On_Damaged(Vector2 targetpos)
    {
        Play_Sound("DAMAGED");

        //생명력
        gamemanager.Health_Down();
        //layer 바꾸기 
        gameObject.layer = 8;

        //렌더러 색깔바꾸기
        spriterenderer.color = new Color(1, 1, 1, 0.4f);

        //튕기는 방향 설정
        int direct = transform.position.x - targetpos.x > 0 ? 1 : -1;

        //충돌물리 구현
        rigid.AddForce(new Vector2(direct, 1) * 15, ForceMode2D.Impulse);

        //애니메이션 구현
        animator.SetTrigger("damaged");

        //2초뒤 무적 해제
        Invoke("Off_Damaged", 2);


    }
    void Off_Damaged()
    {
        gameObject.layer = 7;
        spriterenderer.color = new Color(1, 1, 1, 1);
    }

 
    public void On_Die()
    {   
        //콜라이더 끄기
        capsulecollider.enabled = false;
        //색깔 바꾸기
        spriterenderer.color = new Color(1, 1, 1, 0.4f);
        //뒤집기
        spriterenderer.flipY = true;
        //점프 모션
        rigid.AddForce(Vector2.up * 3, ForceMode2D.Impulse);

        Play_Sound("DIE");
    }

    public void Velocity_Zero()
    {
        rigid.velocity = Vector2.zero;
    }

    void Play_Sound(string action)
    {
        switch (action)
        {
            case "JUMP": //
                audioSource.clip = audioJump;
                break;
            case "ATTACK": //
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED": //
                audioSource.clip = audioDamaged;
                break;
            case "ITEM": //
                audioSource.clip = audioItem;
                break;
            case "DIE": //
                audioSource.clip = audioDie;
                break;
            case "FINISH": //
                audioSource.clip = audioFinish;
                break;
        }

        audioSource.Play();
    }
}
