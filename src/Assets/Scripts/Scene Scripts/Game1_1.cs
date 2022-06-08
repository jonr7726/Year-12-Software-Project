using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//contains list of events for level 1
public class Game1_1 : SceneMan
{

    void Start() {
        nextScene = "Scenes/Game_2_1";
        level = "1";
    }

    override protected void Loop() {
        switch (helpIndex) {
            case 0:
                if (end.activated) {
                    Increment();
                }
                break;
        }
    }
}
