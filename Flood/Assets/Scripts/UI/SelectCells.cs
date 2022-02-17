using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCells : MonoBehaviour
{
    public GameObject CellInformation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (Input.GetMouseButtonDown(0))
        {
            if (hit.collider)
            {
                foreach (Transform child in transform)
                {
                    if (hit.collider.name == child.name)
                    {
                        child.GetComponent<Cell>().isSelected = true;
                    }
                    else
                    {
                        child.GetComponent<Cell>().isSelected = false;
                    }
                }
            }
        }
    }
}
