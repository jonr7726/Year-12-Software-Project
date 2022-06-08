using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//contains list of events for turtorial
public class Turtorial3 : SceneMan
{

    public ReversableBody dummy;
    public MovingObject door1;

    void Start() {
        nextScene = "Scenes/Turtorial_4";
    }

    override protected void Loop() {
        switch (helpIndex) {
            case 0:
                if (!dummy.alive) {
                    door1.commands.Clear();
                    door1.CalculateCommands();
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
