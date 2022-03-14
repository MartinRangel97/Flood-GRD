using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Credits : MonoBehaviour
{
    public void RemoveCredits(float amount)
    {
        float money = float.Parse(gameObject.GetComponent<Text>().text);
        money = money - amount;
        gameObject.GetComponent<Text>().text = money.ToString();
    }

    public void AddCredits(float amount)
    {
        float money = float.Parse(gameObject.GetComponent<Text>().text);
        money = money + amount;
        gameObject.GetComponent<Text>().text = money.ToString();
    }
}
