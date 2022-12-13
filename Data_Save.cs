using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class Data_Save : MonoBehaviour
{

    private Text RankNameCurrent;
    private Text RankScoreCurrent;
    private Text[] RankScoreText;
    private float[] rankScore;
    private string[] rankName;
    private Text[] RankNameText;
    private Text[] RankText;

    private float[] bestScore = new float[5];
    private string[] bestName = new string[5];

    void ScoreSet(float currentScore, string currentName)
    {
        PlayerPrefs.SetString("CurrentPlayerName", currentName);
        PlayerPrefs.SetFloat("CurrentPlayerScore", currentScore);

        float tmpScore = 0f;
        string tmpName = "";

        for (int i = 0; i < 5; i++)
        {
            bestScore[i] = PlayerPrefs.GetFloat(i + "BestScore");
            bestName[i] = PlayerPrefs.GetString(i + "BestName");

            while (bestScore[i] < currentScore)
            {
                tmpScore = bestScore[i];
                tmpName = bestName[i];
                bestScore[i] = currentScore;
                bestName[i] = currentName;

                PlayerPrefs.SetFloat(i + "BestScore", currentScore);
                PlayerPrefs.SetString(i.ToString() + "BestName", currentName);

                currentScore = tmpScore;
                currentName = tmpName;



            }

        }

        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetFloat(i + "BestScore", bestScore[i]);
            PlayerPrefs.SetString(i.ToString() + "BestName", bestName[i]);
        }

        RankNameCurrent.text = PlayerPrefs.GetString("CurrentPlayerName");
        RankScoreCurrent.text = string.Format("{0:N3}", PlayerPrefs.GetFloat("CurrentPlayerScore"));

        for (int i = 0; i < 5; i++)
        {
            rankScore[i] = PlayerPrefs.GetFloat(i + "BestScore");
            RankScoreText[i].text = string.Format("{0:N3}", rankScore[i]);
            rankName[i] = PlayerPrefs.GetString(i.ToString() + "BestName");
            RankNameText[i].text = string.Format(rankName[i]);
            //1위 노란색으로 표시
            if (RankScoreCurrent.text == RankScoreText[i].text)
            {
                Color Rank = new Color(255, 225, 0);
                RankText[i].color = Rank;
                RankNameText[i].color = Rank;
                RankScoreText[i].color = Rank;
            }
        }
    }





}
