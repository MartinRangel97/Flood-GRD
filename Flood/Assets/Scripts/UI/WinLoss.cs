using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WinLoss : MonoBehaviour
{
    public GameObject Credits;
    public GameObject GameOverUI;
    public GameObject ManagerObject;
    public GameObject Stars;

    private bool GameIsOver;
    private bool DidPlayerWin;

    // Update is called once per frame
    void Update()
    {
        checkIfPlayerWon();
    }

    private void checkIfPlayerWon()
    {
        float money = Credits.GetComponent<Credits>().CurrentCreds;
        if(GameIsOver == false)
        {
            if (money <= 0)
            {
                GameIsOver = true;
                DidPlayerWin = false;
                StartCoroutine("GameFinished");
            }
            if (ManagerObject.GetComponent<WorldManager>().simFinished)
            {
                GameIsOver = true;
                DidPlayerWin = true;
                StartCoroutine("GameFinished");
            }
        }
    }
    
    private IEnumerator GameFinished()
    {
        Debug.Log("Check");

        yield return new WaitForSeconds(5f);
        float money = Credits.GetComponent<Credits>().CurrentCreds;
        if (!DidPlayerWin) //Lose Game Screen
        {
            GameOverUI.SetActive(true);
            GameOverUI.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "Level Failed!";
            GameOverUI.transform.Find("Message").GetComponent<TextMeshProUGUI>().text = "You ran out of credits";
            Time.timeScale = 0;
        }
        else //Win Game Screen
        {
            GameOverUI.SetActive(true);
            GameOverUI.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "Level Cleared!";
            GameOverUI.transform.Find("Message").GetComponent<TextMeshProUGUI>().text = "You have " + money.ToString() + " credits remaining";
            ShowRank(Credits.GetComponent<Credits>().CalculateRank());
            Time.timeScale = 0;
        }
    }

    private void ShowRank(int stars)
    {
        for(var i = 0; i < stars; i++)
        {
            Stars.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
        }
    }

    public void ResetGame()
    {
        GameIsOver = false;
        Time.timeScale = 1;
    }
}
