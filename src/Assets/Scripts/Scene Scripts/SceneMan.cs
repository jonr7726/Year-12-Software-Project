using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

//generic scene manager that will increment adn deincrement between stages (for text displayed) which will end wiht moving to the next scene
public class SceneMan : MonoBehaviour {
    
    protected int helpIndex = 0;
    public GameObject[] helpText;
    public PressurePad end;
    protected string nextScene;
    protected bool isNextSceneGameMusic = true;
    protected string level = null;

    virtual protected void Loop() {} //implemented in subclasses

    void Update() {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            NextLevel(true); //return to menu
        }
        Loop();
    }

    protected void DeIncrement(bool createPrevious = false) { //move to previous stage
    	helpText[helpIndex].SetActive(false);
    	helpIndex --;
        if(createPrevious) {
            helpText[helpIndex].SetActive(true);
        }
    }

    protected void Increment(bool destroyPrevious = false) { //move to next stage
        if (destroyPrevious) {
            helpText[helpIndex].SetActive(false);
        }
        helpIndex ++;
        if(helpText.Length - 1 < helpIndex) { //if level completed
            NextLevel();
        }
        else {
            helpText[helpIndex].SetActive(true);
        }
    }

    private void NextLevel(bool returnToMenu = false) { //go to next level (or return to menu)
        if(returnToMenu) {
            level = null;
            isNextSceneGameMusic = false;
            nextScene = "Menu";
        }

        FindObjectsOfType<Protagonist>()[0].UpdateStats(false, level);

        AudioPlayer player = FindObjectsOfType<AudioPlayer>().First();
        if(player == null) {
            SceneManager.LoadScene(nextScene);
            return;
        }

        if (isNextSceneGameMusic != player.IsGameMusic()) {
            player.PlayNew(isNextSceneGameMusic);
        }
        player.Play();
        SceneManager.LoadScene(nextScene);
    }
}
