using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Menu : MonoBehaviour
{
    public GameObject menu;
    public SelectCells sc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key down");
            if (menu.activeInHierarchy)
            {
                menu.SetActive(false);
                sc.enabled = true;
            } else
            {
                menu.SetActive(true);
                sc.enabled = false;
            }
        }
    }
}
