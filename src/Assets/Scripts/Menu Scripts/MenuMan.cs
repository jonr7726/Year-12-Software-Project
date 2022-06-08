using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//handles movement from start menu to other scenes and ending the application
public class MenuMan : MonoBehaviour
{
    public AudioPlayer audioPlayer;

    void Start() {
        if (FindObjectsOfType<AudioPlayer>().Length == 0) {
            Instantiate(audioPlayer);
        }
    }

    public void ChangeScene(string scene) {
        if (scene == "Turtorial_1") {
            FindObjectsOfType<AudioPlayer>()[0].PlayNew(true);
        }
        SceneManager.LoadScene(scene);
    }

    public void EndGame() {
        Application.Quit();
    }
}