using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOver : MonoBehaviour
{
    public GameObject Credits;
    public GameObject GameOverUI;
    public GameObject ManagerObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

    public void ResetGame()
    {
        Time.timeScale = 1;
    }
}
