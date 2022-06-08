using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//will control audio and reset user save from options menu
public class OptionsMan : MonoBehaviour
{
    public GameObject warningText; //text for warning message

    public void ChangeScene(string scene) { //change scene (for going back)
        SceneManager.LoadScene(scene);
    }

    public void ChangeAudio(float volume) { //change audio volume
        FindObjectsOfType<AudioPlayer>()[0].SetVolume(volume);
    }

    public void ResetLog() {
        if(warningText.GetComponent<Text>().text == "Reset Progress") { //warning message
            warningText.GetComponent<Text>().text = "Are you sure?\n(Click again to continue)";
        }
        else {
            warningText.GetComponent<Text>().text = "Reset Progress"; //reset message
            List<string> stats = Util.Instance.ReadFile(); //reset log
            string preserve = "";
            foreach (string line in stats) {
                if (line.Length < 2 || line.Substring(0, 2) == "//") { //if line begins with '//' skip line
                    preserve += line + "\n";
                    continue;
                }
                break; //ignore lines after (there should be none)
            }
            Util.Instance.WriteFile(preserve + "0 0 0 0 []");
        }
    }
}