using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class GameManager : MonoBehaviour
{
    //점수와 스테이지 관리
    public int totalpoint;
    public int totalpoint2;
    public int stagepoint;
    public int stagepoint2;
    public int stageindex;
    public int health;
    public int change;
    int tmp_point;
    public Player_move player_move;
    public GameObject[] stages;
    public GameObject[] stages2;
    public GameObject[] UI_game;
    public GameObject Game;
    public GameObject Store;
    public GameObject Main_Menu;
    public Image[] UI_health;
    public Image[] UI_health2;
    public Text UI_point;
    public Text UI_point2;
    public Text UI_stage;
    public Text UI_stage2;
    public GameObject UI_restart_button;
    public GameObject UI_restart_button2;
    public GameObject UI_main_button;
    public GameObject UI_main_button2;
    public GameObject[] Buy_Button;
  
    

    //상점
    public GameObject[] store_page;
    public Text UI_money;
    public Text UI_money2;
    public int[] item_price;
    AudioSource audiosource;
    public AudioClip[] audiobuy;
    public GameObject money1;
    public GameObject money2;

    private void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }

    private void Update()
    {   
        UI_point.text = (totalpoint + stagepoint).ToString();
        UI_point2.text = (totalpoint2 + stagepoint2).ToString();
        UI_money.text = totalpoint.ToString();
        UI_money2.text = totalpoint2.ToString();
    }

    public void Next_Stage()
    {
        

        if (change == 0)
        {
            money1.SetActive(true);
            money2.SetActive(false);
            //스테이지 전환
            if (stageindex < stages.Length - 1) //스테이지 렝스가 총 3
            {   
                stages[stageindex].SetActive(false);
                stageindex++;
                stages[stageindex].SetActive(true);
                Player_Respawn();

                UI_stage.text = "STAGE" + (stageindex + 1);
            }
            else //게임 클리어
            {
                //시간 정지
                Time.timeScale = 0;

                Text button_TEXT = UI_restart_button.GetComponentInChildren<Text>();
                button_TEXT.text = "Clear";
                veiw_button();
            }

            totalpoint += stagepoint;
            stagepoint = 0;
        }

        else if(change == 1)
        {   
          
            //스테이지 전환
            if (stageindex < stages2.Length - 1) //스테이지 렝스가 총 3
            {
                stages2[stageindex].SetActive(false);
                stageindex++;
                stages2[stageindex].SetActive(true);
                Player_Respawn();

                UI_stage2.text = "STAGE" + (stageindex + 1);
            }
            else //게임 클리어
            {
                //시간 정지
                Time.timeScale = 0;

                Text button_TEXT = UI_restart_button2.GetComponentInChildren<Text>();
                button_TEXT.text = "Clear";
                veiw_button();
            }
            totalpoint2 += stagepoint2;
            stagepoint2 = 0;
        }


    }

    public void Health_Down()
    {
        if (change == 0)
        {
            if (health > 1)
            {
                health--;
                UI_health[health].color = new Color(1, 0, 0, 0.4f);
            }

            //health가 0일 때
            else
            {
                //모든 생명이 다했을 때
                UI_health[0].color = new Color(1, 0, 0, 0.4f);
                //플레이어 죽음 효과
                player_move.On_Die();
                //죽음 UI

                //재시도 UI
                veiw_button();
            }
        }

        else if(change == 1)
        {
            if (health > 1)
            {
                health--;
                UI_health2[health].color = new Color(1, 0, 0, 0.4f);
            }

            //health가 0일 때
            else
            {
                //모든 생명이 다했을 때
                UI_health2[0].color = new Color(1, 0, 0, 0.4f);
                //플레이어 죽음 효과
                player_move.On_Die();
                //죽음 UI

                //재시도 UI
                veiw_button();
            }
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {   
        //낙하시 생명력 감소 
        if(collision.gameObject.tag == "Player")
        {   
            //리스폰
            if (health > 1)
            {
                Player_Respawn();
            }

            //생명력 감소
            Health_Down();
        }
    }

    void Player_Respawn()
    {
        player_move.transform.position = new Vector3(0, 2, -1);
        player_move.Velocity_Zero();
    }
    void veiw_button()
    {
        if (change == 0)
        {
            UI_restart_button.SetActive(true);
            UI_main_button.SetActive(true);
        }

        else if (change == 1)
        {
            UI_restart_button2.SetActive(true);
            UI_main_button2.SetActive(true);
        }

    }
    public void Restart()
    {
        Time.timeScale = 1;
        Game.SetActive(true);
        Store.SetActive(false);
        Main_Menu.SetActive(true);
    }

    public void Main_button()
    {
       SceneManager.LoadScene("Main Menu");
    }

    public void Start_Game()
    {
        Game.SetActive(true);
        Store.SetActive(false);
        Main_Menu.SetActive(false);
        Player_Respawn();
        
    }

    public void Stop_Game()
    {
        Game.SetActive(false);
        Store.SetActive(false);
        Main_Menu.SetActive(true);
    }

    public void Start_Store()
    {
        Store.SetActive(true);
        Game.SetActive(false);
        Main_Menu.SetActive(false);
    }

    public void Stop_Store()
    {
        Store.SetActive(false);
        Game.SetActive(false);
        Main_Menu.SetActive(true);
    }

    public void buy(int index)
    {

        int price = item_price[index];
       
        if (price > totalpoint)
        {
            Debug.Log("실행됨.");
            Play_Sound(1);
        }

        else if (price <= totalpoint)
        {
            //구입 완료 창 UI

            if (index == 0)
            {
                Buy_Button[0].SetActive(true);
                Play_Sound(0);
                //구현
                change_UI();
                health += 1;
                change += 1;
               totalpoint = totalpoint - item_price[0];
                totalpoint2 = totalpoint;
                money1.SetActive(false);
                money2.SetActive(true);
            }
            else if (index == 1)
            {
                Buy_Button[1].SetActive(true);
                Play_Sound(0);
                player_move.maxspeed += 2;
                totalpoint = totalpoint - item_price[1];
            }
            else if (index == 2)
            {
                Buy_Button[2].SetActive(true);
                Play_Sound(0); 
                player_move.jumppower += 2;
                totalpoint = totalpoint - item_price[2];
            }
            else if (index == 3)//과잠
            {
                Buy_Button[3].SetActive(true);
                Play_Sound(0);
                totalpoint = totalpoint - item_price[3];
            }
            else if(index == 4)
            {
                Buy_Button[4].SetActive(true);
                Play_Sound(0);
                totalpoint = totalpoint - item_price[4];
            }
     
        }
          
           
  
    }
    void Play_Sound(int flag) 
    {
        if (flag == 0)
        {
            audiosource.clip = audiobuy[0];
            audiosource.Play();
        }
        else if(flag == 1)
        {
            audiosource.clip = audiobuy[1];
            audiosource.Play();
        }
    }

    public void Next_Page_Btn()
    {
        store_page[0].SetActive(false);
        store_page[1].SetActive(true);
    }

    public void Return_Page_Btn()
    {
        store_page[1].SetActive(false);
        store_page[0].SetActive(true);
    }

    void change_UI()
    {
        UI_game[0].SetActive(false);
        UI_game[1].SetActive(true);
    }

    public void Close_Buy_UI(int index)
    {
        if (index == 0)
            Buy_Button[0].SetActive(false);
        if (index == 1)
            Buy_Button[1].SetActive(false);
        if (index == 2)
            Buy_Button[2].SetActive(false);
        if (index == 3)
            Buy_Button[3].SetActive(false);
        if (index == 4)
            Buy_Button[4].SetActive(false);
    }

}
