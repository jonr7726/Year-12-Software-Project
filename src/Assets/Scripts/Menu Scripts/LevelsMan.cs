using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//will determine which buttons should be displayed from the levels menu based on user log, and handle moving back to title screen
public class LevelsMan : MonoBehaviour
{
    public GameObject[] levels;

    private Dictionary<string, int> levelsMap = new Dictionary<string, int>(){
        {"1", 0},
        {"2", 1}
    };

    void Start() {
        string[] completed = ReadLog();
        for (int i = 0; i < completed.Length-1; i++) {
            if (levelsMap[completed[i]] + 1 < levels.Length) {
                levels[levelsMap[completed[i]] + 1].SetActive(true); //set next level as accessible
            }
        }
    }

    private string[] ReadLog() { //return array of strings for each level completed
        List<string> stats = Util.Instance.ReadFile(); //read log
        foreach (string line in stats) {
            if (line.Length < 2 || line.Substring(0, 2) == "//") { //if line begins with '//' skip line
                continue;
            }
            string[] logString = line.Split(' ');
            logString = logString[4].Substring(1, logString[4].Length - 2).Split(',');
            return logString;
        }
        return new string[0];
    }

    public void ChangeScene(string scene) {
        if (scene != "Menu") {
            FindObjectsOfType<AudioPlayer>()[0].PlayNew(true);
        }
        SceneManager.LoadScene(scene);
    }
}
