using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
public class GameManager : MonoBehaviour
{

    public static bool debugIsOn = true;
    public static int fps;
    private float fpsCheckTimer = 5f;
    public static int score;
    private int lastScore;
    public static bool gameOver = false; // To check if game started...
    public static bool isGameRunning = true; // TODO freeze stuff is game isn't running e.g. at start

    public static float enemySpeedIncrement = 1f;
    private float enemySpeedIncrementIncTime = 15f;
    public float enemyCarSpawnTimer = 20f;
    private float howSoonCanNewEnemyCarSpawn = 20f;
    public static bool enemyCarCanSpawn = true;
    public float powerUpTimer = 30f;
    private float powerUpTimerTime = 30f;
    public static bool canSpawnPowerUp = false;

    public GraphicsQualityManager gqm;
    public GameObject musicPlayer;
    public GameObject highScorePanel;
    public GameObject highscoreLocalNamePanel;
    public GameObject hScoreLocalNameConfigBtn;
    public GameObject wannaQuitPanel;
    public Text nowAndLastHighscoreInGameOver;

    public static string debugGameOverString = "";
    public static string debugCrashString = "";

    public HighscoreItem[] localHighscoreList;
    string newList;


    /// NETWORK STUFF ///
    public User user; // logged in user
    public static string playerName = "Unnamed Player";
    private bool playerMadeAHighScoreCheck = false;
    private int idForLocalPlayer;
    public InputField localPlayerNameInput;
    public static bool networkIsConnected = false;
    public static bool userIsLoggedIn = false;
    private string serverUrl = "https://softa.site/backtothe80s/";
    private string getHScores = "get_hscore.php";
    public HighscoreItemArray highscores;

    // Start is called before the first frame update
    void Start()
    {
        
        Init();
        StartNewGame();

        // Testing message system...
        // MsgManager.SetTheMsg("Welcome to the 80's game! This was a test message!", 5f);

    }

    void Init() {
        
        if (PlayerPrefs.GetInt("userIsLoggedIn") == 1)
        {
            userIsLoggedIn = true;

            /*
            PlayerPrefs.SetInt("userIsLoggedIn", 1);
            PlayerPrefs.SetString("clientEmail", email);
            PlayerPrefs.SetString("clientUsername", user.username);
            PlayerPrefs.SetInt("clientScore", user.score); // last score on db
            PlayerPrefs.SetInt("clinetId", user.id);
            */

            playerName = PlayerPrefs.GetString("clientUsername");
            lastScore = PlayerPrefs.GetInt("clientScore");

            if (GameManager.debugIsOn)
            {
                Debug.Log("User is <color=green>logged in</color>!");
            }

        } else
        {
            userIsLoggedIn = false;

            if (GameManager.debugIsOn)
            {
                Debug.Log("User is <color=red>NOT logged in</color>!");
            }
        }

        InvokeRepeating("AddToEnemyOverallSpeed", 25f, enemySpeedIncrementIncTime);
        InvokeRepeating("AddToEnemyBoxSideSpeed", 60f, 60f);
        playerMadeAHighScoreCheck = false;
        if (PlayerPrefs.HasKey("LocalPlayerName") && !userIsLoggedIn) {
            playerName = PlayerPrefs.GetString("LocalPlayerName");
        }
        highscoreLocalNamePanel.SetActive(false);
        hScoreLocalNameConfigBtn.SetActive(true);
        localPlayerNameInput.placeholder.GetComponent<Text>().text = "Change Your Name?";
        if (playerName == "Unnamed Player") {
            highscoreLocalNamePanel.SetActive(true);
            hScoreLocalNameConfigBtn.SetActive(false);
            localPlayerNameInput.placeholder.GetComponent<Text>().text = "Your name please?";
        }
        highScorePanel.SetActive(false);
        wannaQuitPanel.SetActive(false);
        CheckNetworkStatus();
        debugGameOverString = "";
        debugCrashString = "";
        powerUpTimer = powerUpTimerTime;
        enemyCarSpawnTimer = howSoonCanNewEnemyCarSpawn;

        LoadHighScoreAtStart();

        if(!userIsLoggedIn)
        {
            lastScore = PlayerPrefs.GetInt("last_score");
        }
        
        
    }

    void StartNewGame() {
        score = 0;
        gameOver = false;
        isGameRunning = true;
    }

    void CheckNetworkStatus() {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            networkIsConnected = false;
            
            // Tell user this error
            MsgManager.SetTheMsg("Error: No Internet connection! \nHighscores will only be local and you cannot login!", 7f);
            
            if (GameManager.debugIsOn) {
                Debug.Log("Network Error: check your connection!");
            }
            
        } else {
            networkIsConnected = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.fps = (int)(1f / Time.unscaledDeltaTime);
        if (GameManager.fps <= 20) {
            fpsCheckTimer -= 1f * Time.deltaTime;
            if (fpsCheckTimer <= 0) {
                fpsCheckTimer = 99f;
                MsgManager.SetTheMsg("WARNING: You should consider changing your graphics settings to med or low!"
                +"\nPress <color=blue>'ESC'</color> and select the setting...", 8f);
            }
        }

        powerUpTimer -= 1f * Time.deltaTime;
        if(powerUpTimer < 0 && !canSpawnPowerUp) {
            powerUpTimer = powerUpTimerTime; // reset the poweruptimer
            canSpawnPowerUp = true;
        }

        enemyCarSpawnTimer -= 1f * Time.deltaTime;
        if (enemyCarSpawnTimer <= 0f) {
            enemyCarCanSpawn = true;
            enemyCarSpawnTimer = howSoonCanNewEnemyCarSpawn;
        }


        if(Input.GetKeyUp(KeyCode.Space) && GameManager.gameOver) {
            if (localPlayerNameInput.isFocused) { 
                MsgManager.SetTheMsg("WARNING: Cannot restart the game before you stop editing your name! (The input is in focus)", 8f);
            } else {
                Application.LoadLevel(Application.loadedLevel);
            }
            
        }

        if(Input.GetKeyUp(KeyCode.Escape)) {
            
            if (wannaQuitPanel.activeInHierarchy) {
                wannaQuitPanel.SetActive(false);
            } else {
                wannaQuitPanel.SetActive(true);
            }
        }
    }

    public void RestartGame() {
        
            if (localPlayerNameInput.isFocused) { 
                MsgManager.SetTheMsg("WARNING: Cannot restart the game before you stop editing your name! (The input is in focus)", 8f);
            } else {
                Application.LoadLevel(Application.loadedLevel);
            }
            
        
    }

    public static void End() {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.GameOverNow();
    }
    public void GameOverNow() {

        
        
        if (networkIsConnected && userIsLoggedIn) {
            StartCoroutine(GetHighScoreFromWeb());
        } else {
            Debug.LogError("userIsLoggedIn must be used and implemented... the log in I mean...");
            highScorePanel.SetActive(true);
            GetLocalHighScore();
        }


        nowAndLastHighscoreInGameOver.text = "Your score was: " + score
        + " ||| Your last score: " + lastScore 
        + "\nTRY AGAIN!";


        PlayerPrefs.SetInt("last_score", GameManager.score); // save the score to last score
        
    }

    IEnumerator GetHighScoreFromWeb() {
        
        WWWForm form = new WWWForm();
        form.AddField("game", "BackToThe80s"); // Check this in PHP so not everybody could check the scores
    
        WWW www = new WWW(serverUrl + getHScores, form);


        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {

            // Fill the highscore class itemlist with the database jsondata
            highscores = JsonUtility.FromJson<HighscoreItemArray>(www.text);

            // populate the list
            string newList = "";
            for (int i = 0; i < highscores.highscores.Length; i++) {
                newList += (i+1).ToString() + " " 
                + highscores.highscores[i].name + " " 
                + highscores.highscores[i].score.ToString() + "\n"; 
            }
            // add the hscore list to the highscore panel
            highScorePanel.gameObject.GetComponent<HighscoreList>().highscoreList.text = newList;

            if (GameManager.debugIsOn)
            {
                Debug.Log("<color=green>User data got: </color>" + www.text);
            }
            // show the panel!
            highScorePanel.SetActive(true);
        }
        else
        {
            if (www.text.Contains("ERROR! No Access!")) {
                MsgManager.SetTheMsg("Error: There is something wrong with the PHP code!");
            } else {
                MsgManager.SetTheMsg("Error: Cannot get the highscores from the web!?!");
            }
            
            MsgManager.SetTheMsg("Error: Cannot get the highscores from the web!?!");
        }

        

    }

    void GetLocalHighScore() {
        // ************************ TODO add a check if anything changed and then do this again!
        bool playerMadeAHighScore = false;
        newList = "";
        for (int i = 0; i < localHighscoreList.Length; i++) {
            if (score > localHighscoreList[i].score && playerMadeAHighScore == false || score == localHighscoreList[i].score && playerMadeAHighScore == false) {
                playerMadeAHighScore = true;
                playerMadeAHighScoreCheck = true;
                localHighscoreList[i].score = score;
                localHighscoreList[i].name = playerName;
                idForLocalPlayer = i;
            }
            newList += localHighscoreList[i].id.ToString() + " " + localHighscoreList[i].name + " " 
            + localHighscoreList[i].score.ToString() + "\n"; 

            PlayerPrefs.SetInt("hsId_"+i.ToString(), localHighscoreList[i].id);
            PlayerPrefs.SetInt("hsScore_"+i.ToString(), localHighscoreList[i].score);
            PlayerPrefs.SetString("hsName_"+i.ToString(), localHighscoreList[i].name);
        }
        highScorePanel.gameObject.GetComponent<HighscoreList>().highscoreList.text = newList;
        
    }

    void LoadHighScoreAtStart() {
        if (PlayerPrefs.HasKey("hsId_0")) {

            for (int i = 0; i < localHighscoreList.Length; i++) {

                localHighscoreList[i].id = PlayerPrefs.GetInt("hsId_"+i.ToString());
                localHighscoreList[i].score = PlayerPrefs.GetInt("hsScore_"+i.ToString());
                localHighscoreList[i].name = PlayerPrefs.GetString("hsName_"+i.ToString());

                
            }
            if (GameManager.debugIsOn) {
                    Debug.Log("Local Highscore loaded to array from PlayerPrefs.");
            }

        } else {

            // No scores made, save the default list to playerprefs!
            for (int i = 0; i < localHighscoreList.Length; i++) {
                PlayerPrefs.SetInt("hsId_"+i.ToString(), localHighscoreList[i].id);
                PlayerPrefs.SetInt("hsScore_"+i.ToString(), localHighscoreList[i].score);
                PlayerPrefs.SetString("hsName_"+i.ToString(), localHighscoreList[i].name);
            }
            if (GameManager.debugIsOn) {
                    Debug.Log("Local DEFAULT Highscore saved to PlayerPrefs!");
            }

        }
    }

    public void SaveLocalPlayerName() {

        if (localPlayerNameInput.text == "") {
            MsgManager.SetTheMsg("ERROR: You have to give some NAME in the input field which says: "+localPlayerNameInput.placeholder.GetComponent<Text>().text, 8f);
        } else {

            if (idForLocalPlayer != null && playerMadeAHighScoreCheck) {
                playerName = localPlayerNameInput.text;
                localHighscoreList[idForLocalPlayer].name = playerName;
                GetLocalHighScore();
            } else {
                playerName = localPlayerNameInput.text;
            }

            PlayerPrefs.SetString("LocalPlayerName", playerName);


            highscoreLocalNamePanel.SetActive(false);
            hScoreLocalNameConfigBtn.SetActive(true);
        }
        

    }


    void OnGUI ()
    {
        if (GameManager.debugIsOn) {
            GUI.Label(new Rect(10,5,100,90), "Player:" + playerName);
            GUI.Label(new Rect(10,35,100,90), "FPS: "+fps.ToString());
            GUI.Label(new Rect(10,60,1000,90), debugGameOverString);

            GUI.Label(new Rect(10,110,1000,90), debugCrashString);
        }
        
        
    }

    void AddToEnemyOverallSpeed() {
        float addition = 11f;
        enemySpeedIncrement += addition;
        howSoonCanNewEnemyCarSpawn -= 2f; // enemy cars can spawn more often...
        if(GameManager.debugIsOn) {
            Debug.Log("<color=red>Added "+addition+" </color>to enemy overall speed, this is done every:"+enemySpeedIncrementIncTime+" secs.");
        } 
    }

    public static float boxSideSpeedIncrement = 1f;


    void AddToEnemyBoxSideSpeed() {
        
        float addition = 0.5f;
        boxSideSpeedIncrement += addition;
        if(GameManager.debugIsOn) {
            Debug.Log("<color=orange>Added "+addition+" </color>to enemy box overall side speed");
        }
    }






    public void ExitToMainMenu() {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    

}
