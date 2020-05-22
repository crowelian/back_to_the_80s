using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// PARTS of this code came from the unity technologies UNIT 1,2,3 tutorials (what I learned).
public class PlayerController : MonoBehaviour
{
    
    // Private Player variables
    private float health = 100f;
    private float speed = 45.0f;
    private float origSpeed;
    private float boostSpeed = 50f;
    private float currSpeed;
    private float currDistance;
    private float turnSpeed = 55f;
    private float horizontalInput;
    private float forwardInput;

    private float nitroAmount = 100f;


    private MusicPlayer mPlayer; // music player script

    [Header("Player Audio")]
    // Audio to instantiate (because cannot stop when player dies)
    public GameObject explosionAudio;
    public GameObject collectPowerUpAudio1; // Health
    public GameObject collectPowerUpAudio2; // Speed / Nitro
    public GameObject collectPowerUpAudio3; // Score

    // Audio to use in car audio source (can stop when player dies)
    public AudioSource engineAudio;
    public AudioSource carAudio;
    public AudioClip nitroAudio;
    public AudioClip alarmAudio; // e.g. if health is low!

    


    [Header("Car Configuration")]
    public static float xRange = 60f; // static so a road prefab can easily tell the PlayerController the new xRange!

    private float minSpeed = 3.0f; // increase this to make it harder...
    private float minSpeedIncrementTime = 30f;
    private Vector3 prevPos;
    private Vector3 currVel;

    public Text speedOMeter;
    public Text distanceOMeter;
    public string disColor = "orange";
    public Text scoreOMeter;
    public string scoColor = "#00C4FF";
    public string scoreInfoColor = "#00C4FF"; // color which tells player that this is score!
    public Text healthOMeter;
    public string hmColor = "blue";
    public string healthInfoColor = "#00FF03"; // color which tells player that this is health 
    public Slider nitroAmountSlider;
    public float nitroConsumption = 45f;
    public Text powerUpCollectedOMeter;
    
    [Header("Car Power")]
    // GameObjects that need power
    public GameObject lights;
    public GameObject SpeedOMeterScreen;
    public GameObject ScoreScreen;
    public bool powerOn = true;

    // Check if crashed
    private bool crashed = false;

    private float crashWait;

    [Header("Misc config")]
    public GameObject cameraGameObject;
    public GameObject skyStuff;


    private int lastScoreForScreen; // some bug shows the same score as last although it should be new...

    // Start is called before the first frame update
    void Start()
    {
        origSpeed = speed;
        StartCoroutine(CalcVelocity());

        Init();

        InvokeRepeating("AddToMinSpeed", 60f, minSpeedIncrementTime);

        lastScoreForScreen = PlayerPrefs.GetInt("last_score");

        mPlayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();

        StartCoroutine(PowerUpText(999));
    }

    // Things to initialize
    void Init() {
        crashed = false;
        healthOMeter.text = "Health:<color="+hmColor+"><b>"+health+"%</b></color>";
    }



    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGameRunning) {

            GameManager.score += 1;
            scoreOMeter.text = "<color="+scoreInfoColor+">Score:</color><color="+scoColor+">" + GameManager.score.ToString()+"</color>";

            if(powerOn) {
                horizontalInput = Input.GetAxis("Horizontal");
                forwardInput = Input.GetAxis("Vertical");
            }
            // Check the player bounds
            CheckPlayerBounds();

            // Check if player car starts to rotate
            if (crashed) {
                Crash();
            } else {
                CheckCrash();
            }
            
            // Change the engine audio pitch by the speed
            engineAudio.pitch = 0.4f + (Mathf.Abs(currVel.z) * 0.001f);

            // Move the vehicle forward with Vertical Input (-forwardInput because the 3D-model was backwards... could correct it with empty GameObject etc...)
            // Added also a minSpeed so the player must go forward all the time...
            // Also Shift add speed
            if (Input.GetButton("SHIFT") && nitroAmount > 0f) {
                if (GameManager.debugIsOn) {
                    Debug.Log("SHIFT PRESSED!");
                    
                }

                horizontalInput = 0; // To prevent steering while in nitro boost!
                nitroAmount -= 1f * Time.deltaTime * nitroConsumption;

                if (speed < (origSpeed+boostSpeed)) {
                    speed = speed + boostSpeed;
                    carAudio.PlayOneShot(nitroAudio, 1f);
                    
                }
                
            } else {
                speed = origSpeed;
            }

            if (forwardInput < 0) {
                forwardInput = 0; // stops player going backwards in game!
            }


            transform.Translate(Vector3.forward * Time.deltaTime * speed * -(forwardInput+minSpeed));
            // Rotate the vehicle with Horizontal Input
            //transform.Rotate(Vector3.up * turnSpeed * horizontalInput * Time.deltaTime);

            // Slide the car left / right (-horizontalInput 'cos 3D model backwards)
            transform.Translate(Vector3.right * Time.deltaTime * turnSpeed * -horizontalInput);
            

            // If something funky happens end the game
            if (transform.position.y < -50) {
                Die();
            }
            if (transform.position.y > 50) {
                Die();
            }
            
            nitroAmountSlider.value = nitroAmount;


            if (Input.GetButtonUp("PowerToggle")) {
                PowerToggle();
            }


            // rotate the sky...
            skyStuff.transform.Rotate(0,speed*Time.deltaTime*0.01f,0);

        } // eof isGameRunning...
        
        

        
    }


    void AddToMinSpeed() {
        float add = 0.5f;
        minSpeed += add;
        if (GameManager.debugIsOn) {
            Debug.Log("Added "+add+" more to car minSpeed!");
        }
    }

    void FixedUpdate() {
        // USE IF OBJECT IS MOVED WITH RIGIDBODY
    }


    void Die() {

        // TODO! 
        // Gameover screen
        // Die explosion
        // Maybe external camera showing the explosion?
        Debug.LogError("THIS IS DEBUG GAMEOVER! TODO! FIX THIS! Then remove this message!");
        //TODO scores this is a temp last score
        GameManager.gameOver = true;
        GameManager.debugGameOverString = "GAME OVER - Score:" + GameManager.score.ToString() 
        + " | Last score was: " + lastScoreForScreen
        + " ------ Press the 'spacebar' tp try again! ------";
        
        GameManager.End();
        
        Destroy(this.gameObject);
    }


    IEnumerator CalcVelocity()
    {
        while(Application.isPlaying)
        {
            // Position at frame start
            prevPos = transform.position;
            // Wait till it the end of the frame
            yield return new WaitForEndOfFrame();
            // Calculate velocity: Velocity = DeltaPosition / DeltaTime
            currVel = (prevPos - transform.position) / Time.deltaTime;
            currDistance += Mathf.Abs(currVel.z) * 0.10f;
            // Use Mathf.Abs to remove the minus (because again my 3D Model is going the wrong way!)
            if (!crashed) {
                speedOMeter.text = (Mathf.Abs(currVel.z)).ToString("00.0");
                distanceOMeter.text = "<color="+disColor+">Dist:"+currDistance.ToString("00.0")+"</color>";
            } else {
                speedOMeter.text = "CRASHED!!!";
                distanceOMeter.text = "<color=red>CRASHED!!!</color>";
            }
            
        }
    }

    void CheckCrash() {

        //Debug.Log("CAR ROT:" + transform.rotation);


        //Debug.Log("Checking crash... ROT:"+transform.rotation);
        if (transform.rotation.x > 0.5f || transform.rotation.x < -0.5f) {
            if (!crashed) {
                //Debug.Log("CAR X rot");
                crashed = true;
                crashWait = Time.time + 4f;
                GameManager.debugCrashString = "CRASHED!!!";
                if (GameManager.debugIsOn) {
                    Debug.Log("Car Crashed!");
                }
            }
        }
        if (transform.rotation.y < 0.2f || transform.rotation.y > 1.8f) {
            if (!crashed) {
                //Debug.Log("CAR Y rot");
                crashed = true;
                crashWait = Time.time + 4f;
                GameManager.debugCrashString = "CRASHED!!!";
                if (GameManager.debugIsOn) {
                    Debug.Log("Car Crashed!");
                }
            }
            
        }
        if (transform.rotation.w < -0.1f || transform.rotation.w > 0.1f) {
            if (!crashed) {
                //Debug.Log("CAR w rot");
                crashed = true;
                crashWait = Time.time + 4f;
                GameManager.debugCrashString = "CRASHED!!!";
                if (GameManager.debugIsOn) {
                    Debug.Log("Car Crashed!");
                }
            }
        }

        
    }

    void Crash() {
        if (Time.time > crashWait) {
            Die();
        }
        
    }


    // Toggle car power --- for example power failure negative powerup?
    public void PowerToggle() {
        powerOn = !powerOn;
        if (powerOn) {
            lights.SetActive(true);
            SpeedOMeterScreen.SetActive(true);
            ScoreScreen.SetActive(true);
            
        }
        else {
            lights.SetActive(false);
            SpeedOMeterScreen.SetActive(false);
            ScoreScreen.SetActive(false);
        }
    }



    public void AddHealth(float amount) {

        if (health > 0 && GameManager.gameOver == false) {
            if ((health + amount) < 125f) {
                health += amount;
                healthOMeter.text = "<color="+healthInfoColor+">Health:</color><color=orange><b>"+health+"%</b></color>";
                if(GameManager.debugIsOn) {
                    Debug.Log("health added:" +amount);
                }
                if (mPlayer.musicMoodChangeOn == false) {
                    mPlayer.NormalSetup();
                }   
            } else {
                health = 125f;
                healthOMeter.text = "<color="+healthInfoColor+">Health:</color><color=orange><b>"+health+"%</b></color>";
                if(GameManager.debugIsOn) {
                    Debug.Log("health added:" +amount);
                }
                if (mPlayer.musicMoodChangeOn == false) {
                    mPlayer.NormalSetup();
                }  
            }

            if (health >= 26f && health <= 49f) {
                
                healthOMeter.text = "<color="+healthInfoColor+">Health:</color><color="+hmColor+"><b>"+health+"%</b></color>";
                if (mPlayer.musicMoodChangeOn == false) {
                    mPlayer.NormalSetup();
                }  
                
            } else if (health <= 75f && health >= 50f) {
                healthOMeter.text = "Health:<color=orange><b>"+health+"%</b></color>";
                
            } 
            else if (health < 26f){
                healthOMeter.text = "Health:<color=red><b>"+health+"%</b></color>";
                carAudio.PlayOneShot(alarmAudio, 1f);
            }
        }

        
    }

    public void LoseHealth(float amount) {
        health -= amount;
        if (health >= 26f && health <= 49f) {
            
            healthOMeter.text = "Health:<color="+hmColor+"><b>"+health+"%</b></color>";
            
            
        } else if (health <= 75f && health >= 50f) {
            healthOMeter.text = "Health:<color=orange><b>"+health+"%</b></color>";
            
            
        } 
        else if (health < 26f){
            healthOMeter.text = "Health:<color=red><b>"+health+"%</b></color>";
            carAudio.PlayOneShot(alarmAudio, 1f);
            mPlayer.EnergyAlmosGone();
        } 

        if (health < 1f) {
            Die();
        }
        
    }


    void AddScore(float amount) {
        GameManager.score += (int)amount;
    }

    void AddNitro() {
        nitroAmount = 100f;
    }

    void CheckPlayerBounds() {
        // Keep the player inside the bounds!
        if (transform.position.x < -xRange) {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }
        if (transform.position.x > xRange) {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
    }
    

    public void ShakePlayerCamera(float d,float m) {
        if (cameraGameObject.GetComponent<CameraShake>()) {
            // shake camera (duration, magnitude)
            if(GameManager.debugIsOn) {
                Debug.Log("ShakeCamera!");
            }
            StartCoroutine(cameraGameObject.GetComponent<CameraShake>().Shake(d,m));
        }
    }


    public void AddHealthPowerUp(float addition) {
        if(GameManager.debugIsOn) {
                Debug.Log("POWERUP: health");
        }
        AddHealth(addition);
        Instantiate(collectPowerUpAudio1, transform.position, Quaternion.identity);
        StartCoroutine(PowerUpText(0));
    }

    public void AddSpeedPowerUp() {
        if(GameManager.debugIsOn) {
                Debug.Log("POWERUP: more nitro");
        }
        AddNitro();
        Instantiate(collectPowerUpAudio2, transform.position, Quaternion.identity);
        StartCoroutine(PowerUpText(1));
    }
    
    public void AddScorePowerUp(float addition) {
        if(GameManager.debugIsOn) {
                Debug.Log("POWERUP: add score");
        }
        AddScore(addition);
        Instantiate(collectPowerUpAudio3, transform.position, Quaternion.identity);
        StartCoroutine(PowerUpText(2));
    }



    private void OnTriggerEnter(Collider other) {
        
        if (other.CompareTag("AudioMoodChanger")) {
            
            
        }

        /*
        if (other.CompareTag("DangerSign")) {
            if (gameObject.CompareTag("ActivateStuffTag")) {
                // NOTHING...
            }
        }
        */
        
    }


    IEnumerator PowerUpText(int pIndex) {
        string pText;
        string pColor;
        if (pIndex == 0) {
            pText = "HEALTH";
            pColor = healthInfoColor;
        } else if (pIndex == 1) {
            pText = "NITRO";
            pColor = "yellow";
        } 
        else if (pIndex == 999) {
            pText = "TEST";
            pColor = "#c5072a";
        } else if (pIndex == 2) {
            pText = "SCORE";
            pColor = scoreInfoColor;
        }
        
        else {
            pText = "ERROR";
            pColor = "red";
        }

        powerUpCollectedOMeter.text = "<color="+pColor+">POWERUP COLLECTED</color>";
        yield return new WaitForSeconds(0.5f);
        powerUpCollectedOMeter.text = "";
        yield return new WaitForSeconds(0.3f);
        powerUpCollectedOMeter.text = "<color="+pColor+">POWERUP COLLECTED</color>";
        yield return new WaitForSeconds(0.5f);
        powerUpCollectedOMeter.text = "";
        yield return new WaitForSeconds(0.3f);
        powerUpCollectedOMeter.text = "<color="+pColor+">POWERUP COLLECTED</color>";
        yield return new WaitForSeconds(0.5f);
        powerUpCollectedOMeter.text = "";
        yield return new WaitForSeconds(0.3f);
        powerUpCollectedOMeter.text = "<color="+pColor+">POWERUP COLLECTED</color>";
        yield return new WaitForSeconds(1f);

        powerUpCollectedOMeter.text = "<color="+pColor+">"+pText+"</color>";
        yield return new WaitForSeconds(0.5f);
        powerUpCollectedOMeter.text = "";
        yield return new WaitForSeconds(0.3f);
        powerUpCollectedOMeter.text = "<color="+pColor+">"+pText+"</color>";
        yield return new WaitForSeconds(0.5f);
        powerUpCollectedOMeter.text = "";
        yield return new WaitForSeconds(0.3f);
        powerUpCollectedOMeter.text = "<color="+pColor+">"+pText+"</color>";
        yield return new WaitForSeconds(1f);
        
        powerUpCollectedOMeter.text = "";

    }

}
