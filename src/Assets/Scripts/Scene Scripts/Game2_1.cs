using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//contains list of events for level 2
public class Game2_1 : SceneMan
{

    void Start() {
        nextScene = "Scenes/Menu";
        isNextSceneGameMusic = false;
        level = "2";
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
