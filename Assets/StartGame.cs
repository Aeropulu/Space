﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    public MenuNavigator navigator;
    public MenuInputManager manager;
    public Animator CameraAnimator;

    public PlayerInput LeftInput, Rightinput;
    public InputSchemeSelector LeftSelector, RightSelector;
    public GameObject MainMenu;
    public MenuInputManager menuInput;

    public AudioSource StartGameSound;
    public GameObject GameMusic;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (manager.confirm && navigator.isSelected)
        {
            CameraAnimator.SetTrigger("StartGame");
            LeftInput.inputScheme = LeftSelector.schemes[LeftSelector.SelectedScheme];
            Rightinput.inputScheme = RightSelector.schemes[RightSelector.SelectedScheme];
            LeftInput.gameObject.SetActive(true);
            Rightinput.gameObject.SetActive(true);
            GameState.GameSpeed = 1.0f;
            StartGameSound.Play();
            GameMusic.SetActive(true);
            MainMenu.SetActive(false);
            menuInput.gameObject.SetActive(false);
        }

    }
}
