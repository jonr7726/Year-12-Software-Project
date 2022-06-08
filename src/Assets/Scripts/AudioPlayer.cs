using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//audio player plays music in the game, this can play menu or game music, and will play music in reverse when player reverses time
public class AudioPlayer : MonoBehaviour {

    private AudioSource audioSource;
    public AudioClip game;
    public AudioClip menu;
    private float lastTimeScale;
    public int reversed = 1;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = reversed;
        audioSource.clip = menu;
        audioSource.Play();

        DontDestroyOnLoad(this.gameObject);
    }

    void Update() {
        if (Time.timeScale != lastTimeScale) {
            lastTimeScale = Time.timeScale;
            SetPitch();
        }
        if (Input.GetKeyUp(KeyCode.Q)) {
            reversed *= -1;
            SetPitch();
        }
    }

    public void Play() {
        reversed = 1;
        SetPitch();
    }

    public void PlayNew(bool isGameMusic) {
        audioSource.clip = isGameMusic ? game : menu;
        audioSource.Play();
    }

    public bool IsGameMusic() {
        return audioSource.clip == game;
    }

    private void SetPitch() { //updates pitch so that slower music is not deeper and vise-versa
    	audioSource.pitch = reversed * lastTimeScale;
    }

    public void SetVolume(float volume) {
        audioSource.volume = volume;
    }
}