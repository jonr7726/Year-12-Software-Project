using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//contains list of events for turtorial
public class Turtorial4 : SceneMan
{

    public PressurePad doorTest;

    void Start() {
        nextScene = "Scenes/Turtorial_5";
    }

    override protected void Loop() {
        switch (helpIndex) {
            case 0:
                if (Input.GetKeyUp(KeyCode.Q)) {
                    Increment();
                    Increment();
                }
                break;
            case 2:
                if (doorTest.activated) {
                    Increment(true);
                }
                break;
            case 3:
                if (end.activated) {
                    Increment();
                }
                break;
        }
    }
}
