using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MainMenu : NetworkBehaviour
{

    float scrW, scrH;
    bool inSingleGame;
    bool inMultiGame;
    public GameObject singlePlayer;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        scrH = Screen.height / 9;
        scrW = Screen.width / 16;
        if (inSingleGame)
        {

        }
        else
        {
            if (GUI.Button(new Rect(scrW * 14, scrH, scrW, scrH), "Single"))
            {
                inSingleGame = true;
                SceneManager.LoadSceneAsync("Online");
                Instantiate(singlePlayer);
            }
        }

    }
}
