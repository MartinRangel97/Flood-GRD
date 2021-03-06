using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WinLoss : MonoBehaviour
{
    public Credits Credits;
    public GameObject GameOverUI;
    public GameObject ManagerObject;
    public GameObject Stars;

    private bool GameIsOver;
    private bool DidPlayerWin;
    private List<string[]> rowData = new List<string[]>();

    public int UID;

    private void Start()
    {
        ReadLogResults();
        try
        {
            if(rowData[rowData.Count - 1][0] != null)
            {
                rowData.Remove(rowData[rowData.Count - 1]);
                UID = int.Parse(rowData[rowData.Count - 1][0]) + 1;
               
            }
        }
        catch
        {
            UID = 1;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkIfPlayerWon();
    }

    private void checkIfPlayerWon()
    {
        if(GameIsOver == false)
        {
            if (Credits.CurrentCreds <= 0)
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
        
        yield return new WaitForSeconds(1f);
        LogGameResults();
        float money = Credits.CurrentCreds;
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
            ShowRank(Credits.CalculateRank());
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
        Credits.ResetCreds();

        for (var i = 0; i < Credits.CalculateRank(); i++)
        {
            Stars.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
        Time.timeScale = 1;
    }

    private void ReadLogResults()
    {
        String FileData = File.ReadAllText(Path.Combine(Application.dataPath, "Log.csv"));
        
        String[] lines = FileData.Split('\n');

        for(var i = 0; i < lines.Length; i++)
        {
            String[] lineData = lines[i].Split(',');
            string[] rowDataTemp = new string[6];
            for (var j = 0; j < lineData.Length; j++)
            {
                rowDataTemp[j] = lineData[j];
            }

            rowData.Add(rowDataTemp);

        }
    } 

    private void LogGameResults()
    {
        FileStream filePath = File.Open(Path.Combine(Application.dataPath, "Log.csv"), FileMode.OpenOrCreate);
        
        string[] rowDataTemp = new string[6];
        rowDataTemp[0] = UID.ToString();
        rowDataTemp[1] = WorldManager.level.ToString();
        rowDataTemp[2] = Credits.CalculateRank().ToString();
        rowDataTemp[3] = Credits.StartingCreds.ToString();
        rowDataTemp[4] = Credits.SpentCreds.ToString();
        rowDataTemp[5] = Credits.CurrentCreds.ToString();
        rowData.Add(rowDataTemp);

        Debug.Log(rowData.Count);

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < rowData.Count; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            if(index != 0)
            {
                sb.Append("\n" + string.Join(delimiter, output[index]));
            } else
            {
                sb.Append(string.Join(delimiter, output[index]));
            }
            
        }

        StreamWriter writer = new StreamWriter(filePath);
        writer.Write(sb);
        writer.Close();
    }

   
}
