using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//functionality for help menu interface
public class HelpMenu : MonoBehaviour
{
    public Canvas ui;
    public Canvas help;

    void OnMouseDown() { //when clicked
        if (help.gameObject.activeSelf) {
            Close();
        }
        else {
            Open();
        }
    }

    public void Open() {
        WorldScript.paused = true;

        help.gameObject.SetActive(true); //disyplay help
        ui.gameObject.SetActive(false); //turn off text

    }

    public void Close() {
        WorldScript.paused = false;

        help.gameObject.SetActive(false); //close help
        ui.gameObject.SetActive(true); //turn on text
    }
}