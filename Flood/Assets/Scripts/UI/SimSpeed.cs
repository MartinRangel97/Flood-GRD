using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class SimSpeed : MonoBehaviour
{
    public WorldManager ManagerObject;

    // Update is called once per frame
    void Update()
    {
        ManagerObject.timeBetweenSteps = gameObject.GetComponent<Slider>().value;
    }
}
