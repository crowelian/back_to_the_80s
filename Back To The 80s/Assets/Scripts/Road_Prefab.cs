using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road_Prefab : MonoBehaviour
{

    [TextArea]
    public string TODO = "Here some todo stuff if needed";

    public bool hasGenNew = false; // Has this roadprefab generated a new block of road
    public GameObject spawnPoint; // Where the new block will appear
    public GameObject instantiatePoint; // The point to align this when created
    // At this moment the spawnpoints are array 0 - middle-center, 1 - right, 2 - left, 3 - far center etc...
    public GameObject[] spawnPoints; // points to spawn enemy / powerups / collectables
    public GameObject[] enemies; // Simple way for now... now enemy manager yet...
    public GameObject[] powerUps;
    public int difficulty = 3;
    public bool spawnEnemies = false;
    private bool spawnerDebugIsOn = true;

    public bool fixStopEnemySpawnOnThisBlock = false;

    [Header("grid row")]
    public int xSpawnSize = 0;
    [Header("grid column")]
    public int ySpawnSize = 0;
    private float fixStartX = -60f;

    public GameObject gameManager;

    void Start() {
        gameManager = GameObject.FindWithTag("GameManager");
        //StartCoroutine(SpawnGridDelayed());
        //GenerateSpawnMatrix();
        SpawnEnemies();
    }


    public void SpawnEnemies() {

        if (fixStopEnemySpawnOnThisBlock) {
            if (GameManager.debugIsOn) {
                Debug.Log("<color=green>DO NOT SPAWN ON THIS BLOCK!</color>"+gameObject.name);
            }
        } else {
            if (spawnEnemies) {
                if (difficulty < 3 && difficulty >= 1) {difficulty = 3;}
                if (difficulty == 0){
                    // This is a enemy free zone
                } else {
                    for (int i=0; i < (difficulty/3); i++) {
                        

                    if (spawnPoints.Length < 1) {
                        Debug.LogError("HEY! MORE SPAWNPOINTS!");
                    }

                        int randomn = (int)Random.Range(0,2.2f);
                        int randomEn = (int)Random.Range(0,enemies.Length);
                        
                        // spawnline 1 is (in array) 0,1,2
                        Instantiate(enemies[randomEn], spawnPoints[randomn].transform.position, Quaternion.identity);
                        
                        // spawnline 2 is 3,4,5
                        randomn = (int)Random.Range(3,5.2f);
                        randomEn = (int)Random.Range(0,enemies.Length);
                        Instantiate(enemies[randomEn], spawnPoints[randomn].transform.position, Quaternion.identity);
                        
                        // spawnline 3 is 6,7,8
                        randomn = (int)Random.Range(6,8.2f);
                        randomEn = (int)Random.Range(0,enemies.Length-1);
                        Instantiate(enemies[randomEn], spawnPoints[randomn].transform.position, Quaternion.identity);
                    }
                }

                if (GameManager.canSpawnPowerUp) {
                    int randomPU = (int)Random.Range(0,powerUps.Length);
                    int randomPoint = (int)Random.Range(0,8.2f);
                    GameObject powerU = Instantiate(powerUps[randomPU], spawnPoints[randomPoint].transform.position, Quaternion.identity);
                    
                }
                    
            }
        }

        
        
    }

     void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            if (GameManager.debugIsOn) {
                Debug.Log("Player hit road!");
            }
            

        }

        if (other.gameObject.tag == "RoadSpawner") {
            Debug.Log("Try to create block!!!");
            if (!hasGenNew) {
                if (GameManager.debugIsOn) {
                    Debug.Log("Road Prefab tries to generate a new block!");
                }
                
                gameManager.GetComponent<RoadGenerator>().CallCreateNewBlock();
            }
        }

        if (other.gameObject.tag == "RoadDestroyer") {
            if (GameManager.debugIsOn) {
                Debug.Log("Destroy Block!");        
            }
            
            
            Destroy(this.gameObject);
        }

        
            
        
    }

    void GenerateSpawnMatrix() {
        /*
        for (int i = 0; i < xSpawnSize; i++) {
            for (int x = 0; x < ySpawnSize; x++){
                GameObject spawnCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                spawnCube.transform.localScale = Vector3.one * 20;
                spawnCube.transform.position = new Vector3( 
                    fixStartX + i+i * spawnCube.transform.localScale.x, 
                    1, 
                    x+x * spawnCube.transform.localScale.z
                    );
            }
        }
        */
    }

    IEnumerator SpawnGridDelayed()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.2f);


        for (int row = 0; row < xSpawnSize; row++) {
            yield return new WaitForSeconds(0.2f);
            for (int col = 0; col < ySpawnSize; col++){
                
                
                    GameObject spawnCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    spawnCube.GetComponent<BoxCollider>().enabled = false;
                    spawnCube.transform.localScale = Vector3.one * 20;
                    spawnCube.GetComponent<Renderer>().material.color = new Color(255-(row*10),col*15,0);
                    spawnCube.transform.position = new Vector3( 
                        fixStartX + row * spawnCube.transform.localScale.x, 
                        0, 
                        col * spawnCube.transform.localScale.z
                        );
                
                    Debug.Log("Row:"+row+" Column:"+col); 
                
                   
            }
        }
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }


}
