using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//contains list of events for turtorial
public class Turtorial5 : SceneMan
{
    public Turret turret;
    public MovingObject door1;
    public MovingObject door2;
    public MovingObject door3;
    public PressurePad throughWall;

    void Start() {
        nextScene = "Scenes/Menu";
        isNextSceneGameMusic = false;
        //level = "T";
    }

    override protected void Loop() {
        switch (helpIndex) {
            case 0:
                if (Input.GetKeyUp(KeyCode.Q)) {
                    Increment(true);
                }
                break;
            case 1:
                if (Input.GetKeyUp(KeyCode.Q)) {
                    door1.commands.Clear();
                    door1.CalculateCommands();
                    door2.commands.Clear();
                    door2.CalculateCommands();
                    Increment(true);
                }
                break;
            case 2:
                if (!turret.alive) {
                    Increment(true);
                }
                break;
            case 3:
                if (throughWall.activated) {
                    Increment(true);
                }
                break;
            case 4:
                if (turret.alive) {
                    Increment(true);
                }
                break;
            case 5:
                if (!turret.alive) {
                    Increment(true);
                    Increment();
                    door3.commands.Clear();
                    door3.CalculateCommands();
                    door3.InvertRewind();
                }
                break;
            case 7:
                if (end.activated) {
                    Increment();
                }
                break;
        }
    }
}
