using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//contains list of events for turtorial
public class Turtorial2 : SceneMan {

    public PressurePad doorPad1;
    public MovingObject door1;

    void Start() {
        nextScene = "Scenes/Turtorial_3";
    }

    override protected void Loop() {
        switch (helpIndex) {
            case 0:
                if (doorPad1.activated) {
                    Increment();
                }
                break;
            case 1:
                if (end.activated) {
                    Increment();
                }
                break;
        }
    }
}
