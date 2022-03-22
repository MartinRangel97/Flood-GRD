using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprites : MonoBehaviour {

    public static Sprites instance;

    public List<Sprite> Grasses;
    public List<Sprite> Waters;

    private void Start() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

    public Sprite GetRandomWaterSprite() {
        int random = Random.Range(0, 4);
        if (random == 3) {
            return Waters[1];
        } else {
            return Waters[0];
        }
    }

    public Sprite GetWaterSprite(int index) {
        
        return Waters[index];
    }

    public Sprite GetGrassSprite() {

        int max = 16;
        int random = Random.Range(0, max);
        if (random <= max / 2) {
            return Grasses[0];
        }else if (random > max / 2 && random < max - 2) {
            return Grasses[1];
        } else {
            return Grasses[2];
        }
        
        
    }


}
