﻿using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

    public CanvasGroup pauseGroup; 

	private ShowPanels showPanels;						//Reference to the ShowPanels script used to hide and show UI panels
	private bool isPaused;								//Boolean to check if the game is paused or not
	private StartOptions startScript;                   //Reference to the StartButton script
    private CanvasGroup mainCanvasGroup;

	//Awake is called before Start()
	void Awake()
	{
		//Get a component reference to ShowPanels attached to this object, store in showPanels variable
		showPanels = GetComponent<ShowPanels> ();
		//Get a component reference to StartButton attached to this object, store in startScript variable
		startScript = GetComponent<StartOptions> ();

        mainCanvasGroup = GetComponent<CanvasGroup>(); 
	}

	// Update is called once per frame
	void Update () {

        

    }

    public void PauseGame()
    {
        //Check if the Cancel button in Input Manager is down this frame (default is Escape key) and that game is not paused, and that we're not in main menu
        if (!isPaused && !startScript.inMainMenu)
        {
            Debug.Log("pausing");
            //Call the DoPause function to pause the game
            DoPause();
        }
        
    }

    public void UnpauseGame()
    {
        //If the button is pressed and the game is paused and not in main menu
        if (isPaused && !startScript.inMainMenu)
        {
            //Call the UnPause function to unpause the game
            UnPause();
        }
    }


    public void DoPause()
	{
		//Set isPaused to true
		isPaused = true;
		//Set time.timescale to 0, this will cause animations and physics to stop updating
		Time.timeScale = 0;


        mainCanvasGroup.alpha = 1f;
        //pauseGroup.alpha = 1f; 
        //call the ShowPausePanel function of the ShowPanels script
        showPanels.ShowPausePanel ();
	}


	public void UnPause()
	{
		//Set isPaused to false
		isPaused = false;
		//Set time.timescale to 1, this will cause animations and physics to continue updating at regular speed
		Time.timeScale = 1;
		//call the HidePausePanel function of the ShowPanels script
		showPanels.HidePausePanel ();
        Jam.GameManager.Instance.UnpauseGame(); 
	}


}
