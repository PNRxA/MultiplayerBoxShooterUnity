using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MainMenu : NetworkBehaviour
{

    float scrW, scrH;
    public bool inMainMenu;
    public bool inSingleGame;
    public bool inMultiGame;
    public bool startedGame;
    public GameObject singlePlayer;
    private float audioSlider;
    public bool showOptionsMenu;
    private bool showResOptions;
    private string buttonName;
    private bool fullscreenToggle;
    private Vector2 resScrollPosition;
    public AudioSource audi;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        // Set toggle to current fullscreen status
        fullscreenToggle = Screen.fullScreen;
        // If there are player prefs load them in
        if (PlayerPrefs.HasKey("volume"))
        {
            // Set audio
            audioSlider = PlayerPrefs.GetFloat("volume");
        }
    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (audi == null)
        {
            audi = FindObjectOfType<AudioSource>();
        }
        else
        {

            if (audioSlider != audi.volume)
            {
                audi.volume = audioSlider;
            }

        }

        if (Input.GetKeyDown(KeyCode.Escape) && inSingleGame || Input.GetKeyDown(KeyCode.Escape) && inMultiGame)
        {
            SaveOptions();
            showOptionsMenu = !showOptionsMenu;
        }
    }

    void OnGUI()
    {
        scrH = Screen.height / 9;
        scrW = Screen.width / 16;
        //In the options menu  
        if (showOptionsMenu)
        {
            OptionsMenu();
            if (showResOptions)
            {
                ResOptionsFunc();
            }
        }
        //In a single player game
        else if (inSingleGame)
        {

        }
        //In a multi-player game
        else if (inMultiGame)
        {

        }
        //In the main menu
        else
        {
            HomeMenu();
        }

    }

    void HomeMenu()
    {
        GUI.Box(new Rect(scrW * 7, scrH * 2, scrW * 3, scrH), "Box Shooter");

        if (GUI.Button(new Rect(scrW * 6, scrH * 4, scrW * 2, scrH), "Start Single Player"))
        {
            inSingleGame = true;
            SceneManager.LoadSceneAsync("SP");
            Instantiate(singlePlayer);
        }

        if (GUI.Button(new Rect(scrW * 9, scrH * 4, scrW * 2, scrH), "Start Multiplayer"))
        {
            inMultiGame = true;
            SceneManager.LoadSceneAsync("Offline");
            Instantiate(singlePlayer);
        }

        if (GUI.Button(new Rect(scrW * 7.5f, scrH * 5.5f, scrW * 2, scrH), "Options"))
        {
            showOptionsMenu = true;
        }

        if (GUI.Button(new Rect(scrW * 7.5f, scrH * 7, scrW * 2, scrH), "Quit"))
        {
            // Exit the game
            Application.Quit();
        }
    }

    //Function for the options menu
    //Show options that the player can change
    void OptionsMenu()
    {
        GUI.Box(new Rect(scrW, scrH, scrW * 14, scrH * 6), "Options");

        GUI.Box(new Rect(scrW * 2, scrH * 2, scrW * 4, scrH * 4), "Graphics");

        GUI.Box(new Rect(scrW * 6, scrH * 2, scrW * 4, scrH * 4), "Sound");
        //Used to have more options... R.I.P
        GUI.BeginGroup(new Rect(scrW * 6.5f, scrH * 2.5f, scrW * 4, scrH * 4));
        audioSlider = GUI.HorizontalSlider(new Rect(0, scrH, 3 * scrW, .5f * scrH), audioSlider, 0f, 1f);
        GUI.EndGroup();

        GUI.Box(new Rect(scrW * 10, scrH * 2, scrW * 4, scrH * 4), "Screen");

        GUI.BeginGroup(new Rect(scrW * 10.5f, scrH * 2.5f, scrW * 3, scrH * 5f));

        if (GUI.Button(new Rect(0, 0, scrW * 3, scrH * .5f), buttonName))
        {
            // Show res dropdown menu
            showResOptions = !showResOptions;
        }

        // Only show fullscreen toggle when resolution dropdown isn't shown (to avoid accidental toggling when in dropdown)
        if (!showResOptions)
        {
            fullscreenToggle = GUI.Toggle(new Rect(0, scrH, scrW * 3, scrH * .5f), fullscreenToggle, "Toggle Fullscreen");

            Screen.fullScreen = fullscreenToggle;
        }

        GUI.EndGroup();

        if (GUI.Button(new Rect(scrW * 7f, scrH * 8, scrW * 2, scrH), "Save & Back"))
        {
            // Go back to previous menu and also save options
            SaveOptions();
            showOptionsMenu = false;
            showResOptions = false;
        }

        if (GUI.Button(new Rect(scrW * 12f, scrH * 8, scrW * 2, scrH), "Quit"))
        {
            // // Go back to previous menu and also save options
            SaveOptions();
            // showOptionsMenu = false;
            // showResOptions = false;
            // if (FindObjectOfType<NetworkManager>())
            // {
            //     List<NetworkManager> managers = new List<NetworkManager>();
            //     foreach (var manager in managers)
            //     {
            //         manager.StopHost();
            //         manager.StopMatchMaker();
            //         manager.StopServer();
            //         Destroy(manager.gameObject);
            //     }
            // }
            // Destroy(GameObject.Find("PlayerLobby(Clone)"));
            // SceneManager.LoadScene("Menu");
            // Destroy(gameObject);
            Application.Quit();
        }
    }

    void ResOptionsFunc()
    {
        // Set up resolutions for button labels
        string[] res = new string[] { "1024×576", "1152×648", "1280×720", "1280×800", "1366×768", "1440×900", "1600×900", "1680×1050", "1920×1080", "1920×1200", "2560×1440", "2560×1600", "3840×2160" };

        // Set up resolution values to set (TODO could be improved)
        int[] resW = new int[] { 1024, 1152, 1280, 1280, 1366, 1440, 1600, 1680, 1920, 1920, 2560, 2560, 3840 };
        int[] resH = new int[] { 576, 648, 720, 800, 768, 900, 900, 1050, 1080, 1200, 1440, 1600, 2160 };

        // Create GUI style solid black (kek) for scrollable resolutions
        Texture2D black = new Texture2D(1, 1);
        black.SetPixel(1, 1, Color.black);
        GUIStyle solidBlack = new GUIStyle();
        solidBlack.normal.background = black;


        // Group for the drop down menu
        GUI.BeginGroup(new Rect(scrW * 10.5f, scrH * 3, scrW * 3, scrH * 4));

        resScrollPosition = GUI.BeginScrollView(new Rect(0, 0, scrW * 3, scrH * 4), resScrollPosition, new Rect(0, 0, scrW * 2.6f, scrH * 13));

        GUI.Box(new Rect(0, 0, scrW * 3, scrH * 13), "", solidBlack);

        for (int i = 0; i < 13; i++)
        {
            if (GUI.Button(new Rect(0, scrH * i, scrW * 2.7f, scrH), res[i]))
            {
                // Set resolution based on which button was pressed (array[i] name, array[i] width, array[i] height)
                Screen.SetResolution(resW[i], resH[i], Screen.fullScreen);
                buttonName = res[i];
                showResOptions = false;
            }
        }

        GUI.EndScrollView();

        GUI.EndGroup();
    }

    // Save all options
    void SaveOptions()
    {
        PlayerPrefs.SetFloat("volume", audioSlider);
    }
}
