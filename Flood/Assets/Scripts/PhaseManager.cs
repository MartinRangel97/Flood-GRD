using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phase { MapEditor, DefenceSetup, Simulation}

public class PhaseManager : MonoBehaviour {

    public Phase currentPhase;
    public static PhaseManager instance;

    private void Start() {
        if (instance != null) {
            Destroy(this);
        } else {
            instance = this;
        }
        currentPhase = Phase.MapEditor;
    }

    public static void NextPhase() {
        switch (instance.currentPhase) {
            case Phase.MapEditor:
                instance.currentPhase = Phase.DefenceSetup;
                break;

            case Phase.DefenceSetup:
                instance.currentPhase = Phase.Simulation;
                break;

            default:
                Debug.Log("Nah, doesnt work like that");
                break;
        }
    }


}
