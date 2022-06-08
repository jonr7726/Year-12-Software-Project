using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//contains list of events for turtorial
public class Turtorial1 : SceneMan {

    void Start() {
        nextScene = "Scenes/Turtorial_2";
    }

    override protected void Loop() {
        switch (helpIndex) {
            case 0:
                if (Input.GetAxis("Vertical") != 0 | Input.GetAxis("Horizontal") != 0) {
                    Increment();
                    Increment();
                }
                break;
            case 2:
                if (end.activated) {
                    Increment();
                }
                break;
        }
    }
}
