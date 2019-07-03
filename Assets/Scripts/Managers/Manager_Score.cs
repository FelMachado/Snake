using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager_Score : MonoBehaviour
{
    [SerializeField][HideInInspector]
    List<Text> bestScores = null;

    public void SubmitScore(int score)
    {
        for(int i = 0; i < 5; i++)
        {
            string currentKey = "Score" + i.ToString();
            if (PlayerPrefs.HasKey(currentKey))
            {
                if(score >= PlayerPrefs.GetInt(currentKey))
                {
                    OverwriteScores(score, i);
                    return;
                }
            }
            else
                PlayerPrefs.SetInt(currentKey, score);
        }
    }

    void OverwriteScores(int score, int place)
    {
        int temp2 = score;
        int temp1 = 0;
        for (int i = place; i < 5; i++)
        {
            string currentKey = "Score" + i.ToString();
            temp1 = PlayerPrefs.GetInt(currentKey);
            PlayerPrefs.SetInt(currentKey, temp2);
            temp2 = temp1;
        }
    }

    public void PrintScores()
    {
        for (int i = 0; i < 5; i++)
            this.bestScores[i].text = PlayerPrefs.GetInt("Score" + i.ToString()).ToString();
    }

}
