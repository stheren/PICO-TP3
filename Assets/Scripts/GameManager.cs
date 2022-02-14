using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int MAX_LIFE = 300;

    public Text scoreUI;
    private int score;
    //public int increment = 100;

    public Color hurt;
    public Color badlyHurt;
    public Color verybadlyHurt;

    public Animator animator;

    private void Start()
    {
        score = MAX_LIFE;
    }

    void Update()
    {
        scoreUI.text = score.ToString();

        if(score <= 0)
        {
            animator.SetTrigger("dropDead");
            scoreUI.enabled = false;
        }
        else if (score < MAX_LIFE / 5)
            scoreUI.color = verybadlyHurt;
        else if (score < MAX_LIFE / 3)
            scoreUI.color = badlyHurt;
        else if (score < MAX_LIFE / 2)
            scoreUI.color = hurt;

    }

    public void UpdateScore(int increment)
    {
        score -= increment;
    }

    public void ResetScore()
    {
        score = MAX_LIFE;
    }
}
