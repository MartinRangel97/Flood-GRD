using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Credits : MonoBehaviour
{
    public float StartingCreds;
    public float CurrentCreds;

    private void Start()
    {
        StartingCreds = 1000;
        CurrentCreds = StartingCreds;
        gameObject.GetComponent<Text>().text = CurrentCreds.ToString();
    }

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
        gameObject.GetComponent<Text>().text = CurrentCreds.ToString();
    }

    public int CalculateRank()
    {
        int Stars = 0;

        if (CurrentCreds > CurrentCreds * 0.5) //over 50%
        {
            Stars = 3;
        } 
        else if(CurrentCreds > CurrentCreds * 0.25) //over 25%
        {
            Stars = 2;
        } else
        {
            Stars = 1;
        }

        return Stars;
    }
}
