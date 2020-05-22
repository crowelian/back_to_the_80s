using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System;

public class MainMenuManager : MonoBehaviour
{

    public GameObject mainMenuPanel;
    public GameObject infoPanel;
    public GameObject infoCreditsPanel;
    public GameObject loginPanel;

    public Text loggedInUserText;
    public Text loginText; 
    private string serverUrl = "https://softa.site/backtothe80s/";
    private string loginUrl = "try_login.php";
    private string registerUrl = "try_register.php";
    public InputField logEmail;
    public InputField logPassword;
    public InputField regEmail;
    public InputField regPassword1;
    public InputField regPassword2;
    public InputField regUsername;

    WWWForm form;
    WWWForm formRegister;

    bool networkIsConnected = false;
    bool firstTimeRun = true;


    //DEBUG
    public User loggedInUser;

    // Start is called before the first frame update
    void Start()
    {


        infoPanel.SetActive(false);
        infoCreditsPanel.SetActive(false);
        loggedInUserText.text = "";
        loginText.text = "";
        PlayerPrefs.SetInt("userIsLoggedIn", 0); // set this always to 0 at main menu start because the user cannot be logged in at start!
        CheckNetworkStatus();
    }

    public void LoadMainGame() {
        SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
    }

    public void QuitGame() {
        Application.Quit();
    }

    void CheckNetworkStatus()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            networkIsConnected = false;

            // Tell user this error
            loginText.text = "<color=red>NO INTERNET CONNECTION!</color> \nConnect to the INTERNET then try again! \nOr if you already connected try again now!";

            if (GameManager.debugIsOn)
            {
                Debug.Log("Network Error: check your connection!");
            }

        }
        else
        {
            networkIsConnected = true;

            if (PlayerPrefs.HasKey("Registering Offered"))
            {
                firstTimeRun = false;
            } else
            {
                loginPanel.SetActive(true);
                PlayerPrefs.SetInt("Registering Offered", 1);
                loginText.text = "<color=orange>You can register or login here!</color> But this is NOT mandatory, you can go BACK from the back button.This message is shown only once!";
            }
        }
    }


    public void Login()
    {
        loginText.text = "<color=orange>Loggin in...</color>";
        if (!string.IsNullOrEmpty(logEmail.text) || !string.IsNullOrEmpty(logPassword.text))
        {
            StartCoroutine(LoginUser());
        } else
        {
            loginText.text = "<color=red>Fill the email & password!!!</color> \nCannot be blank!";
        }
    }

    public void Register()
    {
        loginText.text = "<color=orange>Registering new user..</color>";
        if (!string.IsNullOrEmpty(regEmail.text) || !string.IsNullOrEmpty(regPassword1.text) || !string.IsNullOrEmpty(regPassword2.text))
        {
            StartCoroutine(RegisterUser());
        }
        else
        {
            loginText.text = "<color=red>Fill in the email, password and the confirmation password!</color> \nCannot be blank!";
        }
    }

    IEnumerator LoginUser()
    {
        string email = logEmail.text;
        
        string password = logPassword.text;

        form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("game", "BackToThe80s"); // Check this in PHP so not everybody could check the scores

        WWW www = new WWW(serverUrl + loginUrl, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            User user = JsonUtility.FromJson<User>(www.text);
            

            if (user.success == true)
            {

                // ALWAYS REMEMBER TO USE THE SAME "null" string in php. example: != " " and in php 'error' => ' '   SO IT IS NOT "" it is " "...
                if (user.error != " " /*|| user.error != ""*/)
                {
                    loginText.text = "error " + user.error + www.error;
                }
                else
                {


                    loginText.text = "Logged in!";
                    // Set the current client user to playerprefs
                    // And also set the userIsLoggedIn to 1 (true)
                    PlayerPrefs.SetInt("userIsLoggedIn", 1);
                    PlayerPrefs.SetString("clientEmail", email);
                    PlayerPrefs.SetString("clientUsername", user.username);
                    PlayerPrefs.SetInt("clientScore", user.score); // last score on db
                    PlayerPrefs.SetInt("clinetId", user.id);

                    loggedInUser = user;

                    loggedInUserText.text = "Welcome "+user.username;
                    if (user.score > 0)
                    {
                        loggedInUserText.text = "Welcome back "+user.username;
                    }

                    if (GameManager.debugIsOn)
                    {
                        Debug.Log("DATA:\n" + www.text);
                    }

                }
            }
            else
            {
                loginText.text = "Error occured: " + user.error /*+ " --- " + www.error + " ||| \n" + www.text*/;
            }


        }


        else
        {
            //textErrorMessages.text = "Error" + www.error;
        }
    }

    IEnumerator RegisterUser()
    {
        string email = regEmail.text;
        string password = regPassword1.text;
        string reenterpassword = regPassword2.text;
        string username = regUsername.text;


        if (password.Length < 8)
        {
            loginText.text = "Password must be at least 8 characters!";
            //yield return false;
            yield break;
        }
        if (password != reenterpassword)
        {
            loginText.text = "Passwords does not match!";
            yield break;
        }
        if (email == " " || email == null || email.Length < 5)
        {
            loginText.text = "Email must be over 5 characters!";
            yield break;
        }
        if (username == " " || username == null || username.Length < 4)
        {
            loginText.text = "Username must be over 4 characters!";
            yield break;
        }
        if (IsValidEmailAddress(email) == false)
        {
            loginText.text = "Use a valid email address!";
            yield break;
        }
        if (email == password)
        {
            loginText.text = "Email and password cannot be the same!";
            yield break;
        }



        formRegister = new WWWForm();
        formRegister.AddField("email", email);
        formRegister.AddField("password", password);
        formRegister.AddField("username", username);
        formRegister.AddField("game", "BackToThe80s"); // Check this in PHP so not everybody could check the scores

        //Debug.Log("EMail:" + email);

        WWW www = new WWW(serverUrl + registerUrl, formRegister);
        yield return www;


        // Register the user php
        if (string.IsNullOrEmpty(www.error))
        {


            User user = JsonUtility.FromJson<User>(www.text);

            if (user.success == true)
            {

                // ALWAYS REMEMBER TO USE THE SAME "null" string in php. example: != " " and in php 'error' => ' '   SO IT IS NOT "" it is " "...
                if (user.error != " " /*|| user.error != ""*/)
                {
                    loginText.text = "error " + user.error + www.error;

                    if (GameManager.debugIsOn)
                    {
                        Debug.Log("NULL ERROR: " + user.error + " --- " + www.text);
                    }
                }
                else
                {
                    loginText.text = "Registration was a success! Now you can login.";

                    loggedInUser = user;

                    // Set the current client user to playerprefs
                    PlayerPrefs.SetString("clientUser", email);


                }
            }
            else
            {
                loginText.text = "Error occured: " + user.error;

                if (GameManager.debugIsOn)
                {
                    Debug.Log("ERROR: " + user.error + " --- " + www.text);
                }
            }



        }
        // if www.text contains php error:
        else
        {
            loginText.text = "Error: " + www.error;

            if (GameManager.debugIsOn)
            {
                Debug.Log("ERROR: " + www.text);
            }
        }
    }



    public bool IsValidEmailAddress(string s)
    {
        var regex = new Regex(@"[a-z0-9!#$%&amp;'*+/=?^_`{|}~-]+(?:.[a-z0-9!#$%&amp;'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
        return regex.IsMatch(s);
    }

}


[Serializable]
public class User
{

    public bool success;
    public string error;
    public string email;
    public string username;
    public int id;
    public int score;

}