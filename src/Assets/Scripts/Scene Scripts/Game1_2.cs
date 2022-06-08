using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//(not used in final code)
public class Game1_2 : SceneMan
{

    void Start() {
        nextScene = "Scenes/Game_2_1";
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
