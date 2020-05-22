using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{

    public GameObject currentPrefab;
    public GameObject[] roadPrefabs;
    public int randomBlock; // change this to create random roadblock everytime.

    private Transform spawnHere;
    private Transform originPoint;
    // Start is called before the first frame update
    void Start()
    {
        // Generate the starting roads!
        spawnHere = currentPrefab.GetComponent<Road_Prefab>().spawnPoint.transform;
        originPoint = currentPrefab.GetComponent<Road_Prefab>().instantiatePoint.transform;
        if (currentPrefab.GetComponent<Road_Prefab>().hasGenNew == false) {
            currentPrefab.GetComponent<Road_Prefab>().hasGenNew = true;
            GameObject newObj = Instantiate(roadPrefabs[randomBlock], spawnHere.position, Quaternion.identity);
            // This below need to be fixed!!! It it glued... Should've done the prefab originpoint to the "end".
            newObj.transform.position = newObj.transform.position + new Vector3(0,0,(newObj.transform.localScale.z*4f));
            currentPrefab = newObj;
            
        } else {
            if (GameManager.debugIsOn) {
                Debug.Log("Error: tried to create second roadblock! name:" +currentPrefab.gameObject.name);
            }
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallCreateNewBlock() {
        CreateNewBlock();
    }

    private void CreateNewBlock() {
        // Generate the starting roads!
        spawnHere = currentPrefab.GetComponent<Road_Prefab>().spawnPoint.transform;
        originPoint = currentPrefab.GetComponent<Road_Prefab>().instantiatePoint.transform;
        if (currentPrefab.GetComponent<Road_Prefab>().hasGenNew == false) {
            currentPrefab.GetComponent<Road_Prefab>().hasGenNew = true;
            randomBlock = GenerateRandomNumber();
            GameObject newObj = Instantiate(roadPrefabs[randomBlock], spawnHere.position, Quaternion.identity);
            // This below need to be fixed!!! It it glued... Should've done the prefab originpoint to the "end".
            newObj.transform.position = newObj.transform.position + new Vector3(0,0,(newObj.transform.localScale.z*4f));
            newObj.GetComponent<Road_Prefab>().spawnEnemies = true;
            newObj.GetComponent<Road_Prefab>().SpawnEnemies();
            currentPrefab = newObj;
        } else {
            if (GameManager.debugIsOn) {
                Debug.Log("Error: tried to create second roadblock! name:" +currentPrefab.gameObject.name);
            }
        }
    }


    int GenerateRandomNumber() {
        int random = (int)Random.Range(0, roadPrefabs.Length);
        return random;
    }

}
