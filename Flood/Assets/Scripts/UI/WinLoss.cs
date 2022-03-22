using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WinLoss : MonoBehaviour
{
    public GameObject Credits;
    public GameObject GameOverUI;
    public GameObject WinUI;
    public GameObject WinMessage;
    public GameObject ManagerObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Win();
        gameOver();
    }

    private void gameOver()
    {
        float money = float.Parse(Credits.GetComponent<Text>().text);
        if (money <= 0)
        {
            GameOverUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void Win()
    {
        if (ManagerObject.GetComponent<WorldManager>().simFinished)
        {
            
            WinUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResetGame()
    {
        Time.timeScale = 1;
    }
}
