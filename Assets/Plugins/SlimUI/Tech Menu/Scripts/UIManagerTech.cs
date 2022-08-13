using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class UIManagerTech : MonoBehaviour
{
	[Header("Simple Panels")]
	[Tooltip("The UI Panel holding the Home Screen elements")]
	public GameObject homeScreen;
	[Tooltip("The UI Panel holding the credits")]
	public GameObject creditsScreen;
	[Tooltip("The UI Panel holding the settings")]
	public GameObject systemScreen;
	[Tooltip("The UI Panel holding the CANCEL or ACCEPT Options for New Game")]
	public GameObject newGameScreen;
	[Tooltip("The UI Panel holding the YES or NO Options for Load Game")]
	public GameObject loadGameScreen;
	[Tooltip("The Loading Screen holding loading bar")]
	public GameObject loadingScreen;
	[Tooltip("The UI Menu Bar at the edge of the screen")]
	public GameObject menuBar;

	[Header("COLORS - Tint")]
	public Image[] panelGraphics;
	public Image[] blurs;
	public Color tint;

	[Header("ADVANCED - UI Elements & User Data")]
	[Tooltip("The Main Canvas Gameobject")]
	public CanvasScaler mainCanvas;
	[Tooltip("The dropdown menu containing all the resolutions that your game can adapt to")]
	public TMP_Dropdown ResolutionDropDown;
	private Resolution[] resolutions;
	[Tooltip("The text object in the Settings Panel displaying the current quality setting enabled")]
	public TMP_Text qualityText; // text displaying current selected quality
	[Tooltip("The icon showing the current quality selected in the Settings Panels")]
	public Animator qualityDisplay;
	private string[] qualityNames;
	private int tempQualityLevel;// store it for start up text update
	[Tooltip("The volume slider UI element in the Settings Screen")]
	public Slider audioSlider;

	[Tooltip("If a message is displaying indiciating FAILURE, this is the color of that error text")]
	public Color errorColor;
	[Tooltip("If a message is displaying indiciating SUCCESS, this is the color of that success text")]
	public Color successColor;
	public float messageDisplayLength = 2.0f;
	public Slider uiScaleSlider;
	float xScale = 0f;
	float yScale = 0f;

	[Header("Menu Bar")]
	public bool showMenuBar = true;
	[Tooltip("The Arrow at the corner of the screen activating and de-activating the menu bar")]
	public GameObject menuBarButton;
	[Tooltip("The date and time display text at the bottom of the screen")]
	public TMP_Text dateDisplay;
	public TMP_Text timeDisplay;
	public bool showDate = true;
	public bool showTime = true;

	[Header("Loading Screen Elements")]
	public ToggleGroup toggleGroup;
	[Tooltip("The name of the scene loaded when a 'NEW GAME' is started")]
	public string newSceneName;
	[Tooltip("The loading bar Slider UI element in the Loading Screen")]
	public Slider loadingBar;
	private string loadSceneName; // scene name is defined when the load game data is retrieved

	[Header("Debug")]
	[Tooltip("If this is true, pressing 'R' will reload the scene.")]
	public bool reloadSceneButton = true;
	Transform tempParent;

	public void MoveToFront(GameObject currentObj){
		//tempParent = currentObj.transform.parent;
		tempParent = currentObj.transform;
		tempParent.SetAsLastSibling();
	}

	void Start(){
		// By default, starts on the home screen, disables others
		homeScreen.SetActive(true);
		if(creditsScreen != null)
		creditsScreen.SetActive(false);
		if(systemScreen != null)
		systemScreen.SetActive(false);
		if(loadingScreen != null)
		loadingScreen.SetActive(false);
		if(loadGameScreen != null)
		loadGameScreen.SetActive(false);
		if(newGameScreen != null)
		newGameScreen.SetActive(false);

		if(menuBar != null){
			if(!showMenuBar){
				menuBar.gameObject.SetActive(false);
				menuBarButton.gameObject.SetActive(false);
			}
		}

		// Set Colors if the user didn't before play
		for(int i = 0; i < panelGraphics.Length; i++)
        {
           panelGraphics[i].color = tint;
        }
		for(int i = 0; i < blurs.Length; i++)
        {
           blurs[i].material.SetColor("_Color",tint);
        }

		// Get quality settings names
		qualityNames = QualitySettings.names;

		// Get screens possible resolutions
		resolutions = Screen.resolutions;

		// Set Drop Down resolution options according to possible screen resolutions of your monitor
		if(ResolutionDropDown != null){
		for (int i = 0; i < resolutions.Length; i++){
				ResolutionDropDown.options.Add (new TMP_Dropdown.OptionData (ResToString (resolutions [i])));
	
				ResolutionDropDown.value = i;
	
				ResolutionDropDown.onValueChanged.AddListener(delegate { Screen.SetResolution(resolutions
				[ResolutionDropDown.value].width, resolutions[ResolutionDropDown.value].height, true);});
			}
		}
		 
		 // Check if first time so the volume can be set to MAX
		 if(PlayerPrefs.GetInt("firsttime")==0){
			 // it's the player's first time. Set to false now...
			 PlayerPrefs.SetInt("firsttime",1);
			 PlayerPrefs.SetFloat("volume",1);
		 }

		 // Check volume that was saved from last play
		 if(audioSlider != null)
		 audioSlider.value = PlayerPrefs.GetFloat("volume");

	}

	public void SetTint(){
		for(int i = 0; i < panelGraphics.Length; i++)
        {
           panelGraphics[i].color = tint;
        }
		for(int i = 0; i < blurs.Length; i++)
        {
           blurs[i].material.SetColor("_Color",tint);
        }
	}

	// Just for reloading the scene! You can delete this function entirely if you want to
	void Update(){
		if(reloadSceneButton){
			if(Input.GetKeyDown(KeyCode.Delete)){
				SceneManager.LoadScene("Tech Demo Scene");
			}
		}

		SetTint();

		if(showMenuBar){
			// Menu Bar and Clock/Date Elements
			//DateTime time = DateTime.Now;
			if(showTime){timeDisplay.text = DateTime.Now.ToString("HH:mm:ss");}else if(!showTime){timeDisplay.text = "";}
			if(showDate){dateDisplay.text = System.DateTime.Now.ToString("yyyy/MM/dd");}else if(!showDate){dateDisplay.text = "";}
		}
	}

	// called when returned back to the database menu, confirmation message displays temporarily
	public void MessageDisplayDatabase(string message, Color col){
		StartCoroutine(MessageDisplay(message, col));
	}

	IEnumerator MessageDisplay(string message, Color col){ // Display and then clear
		yield return new WaitForSeconds(messageDisplayLength);
	}

	public void UIScaler(){
		xScale = 1920 * uiScaleSlider.value;
		yScale = 1080 * uiScaleSlider.value;
		mainCanvas.referenceResolution = new Vector2 (xScale,yScale);
	}

	// Make sure all the settings panel text are displaying current quality settings properly and updating UI
	public void CheckSettings(){
		tempQualityLevel = QualitySettings.GetQualityLevel(); 
		if(tempQualityLevel == 0){
			qualityText.text = qualityNames[0];
			qualityDisplay.SetTrigger("Low");
		}else if(tempQualityLevel == 1){
			qualityText.text = qualityNames[1];
			qualityDisplay.SetTrigger("Medium");
		}else if(tempQualityLevel == 2){
			qualityText.text = qualityNames[2];
			qualityDisplay.SetTrigger("High");
		}else if(tempQualityLevel == 3){
			qualityText.text = qualityNames[3];
			qualityDisplay.SetTrigger("Ultra");
		}
	}

	// Converts the resolution into a string form that is then used in the dropdown list as the options
	string ResToString(Resolution res)
	{
		return res.width + " x " + res.height;
	}

	// Whenever a value on the audio slider in the settings panel is changed, this 
	// function is called and updated the overall game volume
	/*public void AudioSlider(){
		AudioListener.volume = audioSlider.value;
		PlayerPrefs.SetFloat("volume",audioSlider.value);
	}*/

	// When accepting the QUIT question, the application will close 
	// (Only works in Executable. Disabled in Editor)
	public void Quit(){
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	} 

	// Changes the current quality settings by taking the number passed in from the UI 
	// element and matching it to the index of the Quality Settings
	public void QualityChange(int x){
		if(x == 0){
			QualitySettings.SetQualityLevel(x, true);	
			qualityText.text = qualityNames[0];
		}else if(x == 1){
			QualitySettings.SetQualityLevel(x, true);
			qualityText.text = qualityNames[1];
		}else if(x == 2){
			QualitySettings.SetQualityLevel(x, true);
			qualityText.text = qualityNames[2];
		}if(x == 3){
			QualitySettings.SetQualityLevel(x, true);
			qualityText.text = qualityNames[3];
		}
	}

	// Called when loading new game scene
	public void LoadNewLevel (){
		Toggle toggle = toggleGroup.GetFirstActiveToggle();
		switch (toggle.name)
        {
			case "BeginnerToggle":
				newSceneName = "KolaySeviyeSahne";
				break;
			case "MediumToggle":
				newSceneName = "MidLevelScene";
				break;
			case "ExpertToggle":
				newSceneName = "HardLevelScene";
				break;
		}
		if(newSceneName != ""){
			StartCoroutine(LoadAsynchronously(newSceneName));
		}
	}

	// Load Bar synching animation
	IEnumerator LoadAsynchronously (string sceneName){ // scene name is just the name of the current scene being loaded
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

		while (!operation.isDone){
			float progress = Mathf.Clamp01(operation.progress / .9f);
			
			loadingBar.value = progress;

			yield return null;
		}
	}


}
