using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Residential : MonoBehaviour {

    public Vector2 position;
    public float Health { get; private set; }
    public bool IsDestroyed { get; private set; }
    public bool IsFloodProofed { get; set; }


    private GameObject Credits;

    public void Setup(Vector2 position) {
        this.position = position;
        Health = 100f;
        IsDestroyed = false;
        Credits = GameObject.Find("Money");
        IsFloodProofed = false;
    }



    public void ReduceHealth(float damageValue) {
        Health -= damageValue;
        if (Health <= 0) {
            IsDestroyed = true;
            gameObject.GetComponent<Cell>().ChangeColour(0, 0, 0);
        } else
        {
            if (IsFloodProofed)
            {
                Credits.GetComponent<Credits>().RemoveCredits(damageValue/2);
            }else
            {
                Credits.GetComponent<Credits>().RemoveCredits(damageValue);
            }
            
        }
    }

    public void RemoveResidential() {
        Destroy(this);
    }

}
