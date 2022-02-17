using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHide : MonoBehaviour
{
    public GameObject panel;
    public bool isShowing = true;
    private float speed = 800;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isShowing)
        {
            panel.GetComponent<RectTransform>().localPosition = Vector2.MoveTowards(panel.transform.localPosition, new Vector2(745.25f, 0), speed * Time.deltaTime);
        }
        else
        {
            panel.GetComponent<RectTransform>().localPosition = Vector2.MoveTowards(panel.transform.localPosition, new Vector2(1179, 0), speed * Time.deltaTime);
        }
    }

    public void togglePanel()
    {
        if (isShowing)
        {
            isShowing = false;
            gameObject.transform.GetChild(0).GetComponent<Text>().text = "<<<";
        } 
        else
        {
            isShowing = true;
            gameObject.transform.GetChild(0).GetComponent<Text>().text = ">>>";
        }

    }
}
