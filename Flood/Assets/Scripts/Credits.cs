using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Credits : MonoBehaviour
{
    public float StartingCreds = 1;
    public float CurrentCreds = 1;
    public float SpentCreds;

    public void RemoveCredits(float amount)
    {
        CurrentCreds -= amount;
        if (CurrentCreds <= 0)
            CurrentCreds = 0;
        gameObject.GetComponent<Text>().text = CurrentCreds.ToString();
    }

    public void AddCredits(float amount)
    {
        CurrentCreds += amount;
        SpentCreds -= amount;
        gameObject.GetComponent<Text>().text = CurrentCreds.ToString();
    }

    public int CalculateRank()
    {
        int Stars = 0;

        if (CurrentCreds > StartingCreds * 0.5) //over 50%
        {
            Stars = 3;
        } 
        else if(CurrentCreds > StartingCreds * 0.25) //over 25%
        {
            Stars = 2;
        } else if (CurrentCreds > 0)
        {
            Stars = 1;
        }

        return Stars;
    }

    public void ResetCreds()
    {
        CurrentCreds = StartingCreds;
        gameObject.GetComponent<Text>().text = CurrentCreds.ToString();
    }

   public void FloodDefenceCreds(float amount)
    {
        SpentCreds += amount;
    }

    public void SetCredits(int houses)
    {
        StartingCreds = houses * 100;
        CurrentCreds = StartingCreds;
        gameObject.GetComponent<Text>().text = CurrentCreds.ToString();
    }
    
}
