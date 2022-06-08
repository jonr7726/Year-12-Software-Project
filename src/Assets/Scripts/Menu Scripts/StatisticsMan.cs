using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//will handle loading statistics from user log and displaying them
public class StatisticsMan : MonoBehaviour
{
    public GameObject textBox;
    private string[] statNames = { "Bullets Fired:		", "Time Reversed:	",  "Enemies Killed:    ", "Death Count:		" };

    void Start() {
        int[] originalStats = ReadLog();
        int[,] sortedStats = SortLog(ReadLog());

        string text = "";
        for (int i = 0; i < originalStats.Length; i++) {
            text += statNames[sortedStats[i,1]] + sortedStats[i,0] + "\n";
        }

        textBox.GetComponent<Text>().text = text;
    }

    public void ChangeScene(string scene) { //change scene (for going back)
        SceneManager.LoadScene(scene);
    }

    private int[,] SortLog(int[] log) { //sorts log by highest number (selection sort)
        int[] indexes = { 0, 1, 2, 3 };
        for (int i = 0; i < log.Length - 1; i++) { //repeat size - 1 times:
            int highest = i; //set highest to frist element (in unsorted list)
            for (int ii = i+1; ii < log.Length; ii++) { //find highest value in unsorted array
                if (log[ii] > log[highest]) {
                    highest = ii;
                }
            }
            if (i != highest) { //swap current position with highest in unsorted list
                int temp = log[i];
                log[i] = log[highest];
                log[highest] = temp;

                temp = indexes[i]; //(copy operation to indexes list)
                indexes[i] = indexes[highest];
                indexes[highest] = temp; 
            }
        }
        int[,] sortedLog = new int[4,2];
        for(int i = 0; i < log.Length; i++) { //constuct 2D array
            sortedLog[i, 0] = log[i];
            sortedLog[i, 1] = indexes[i];
        }
        return sortedLog;
    }

    private int[] ReadLog() {
        List<string> stats = Util.Instance.ReadFile(); //read log
        int[] log = new int[4];
        foreach (string line in stats) {
            if (line.Length < 2 || line.Substring(0, 2) == "//") { //if line begins with '//' skip line
                continue;
            }
            string[] logString = line.Split(' ');
            for(int i = 0; i < 4; i++) {
                log[i] = int.Parse(logString[i]); //makes array of strings, an array of integers
            }
            break; //ignore lines after (there should be none)
        }
        return log;
    }
}