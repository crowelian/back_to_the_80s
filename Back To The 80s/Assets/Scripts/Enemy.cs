using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float damageToPlayer = 25.0f;
    public Vector3 offset;
    public Quaternion startRot;

    public float playerCamShakeMaqnitude = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = transform.position + offset; // set the starting position offset
        transform.rotation = Quaternion.Euler(startRot.x, startRot.y, startRot.z); // Set the starting rotation (if needed)
    }


    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<PlayerController>().LoseHealth(damageToPlayer);
            Instantiate(other.gameObject.GetComponent<PlayerController>().explosionAudio, transform.position, Quaternion.identity);
            TryShake(other.gameObject);
            Destroy(this.gameObject);
        }    
    }

    void OnTriggerEnter(Collider other) {
        if((other.gameObject.tag == "Player" && other.gameObject.tag != "RoadSpawner") || (other.gameObject.tag == "Player" && other.gameObject.tag != "RoadDestroyer")) {
            other.gameObject.GetComponent<PlayerController>().LoseHealth(damageToPlayer);
            Debug.LogError("TODO: ADD Explosion to this! +enemyhit");
            Instantiate(other.gameObject.GetComponent<PlayerController>().explosionAudio, transform.position, Quaternion.identity);
            TryShake(other.gameObject);
            Destroy(this.gameObject);
        }

        // Destroy the enemy
        if (other.gameObject.tag == "RoadDestroyer") {
            if (GameManager.debugIsOn) {
                Debug.Log("Destroy Enemy Left <color=red>BEHIND</color>!");        
            }
            
            
            Destroy(this.gameObject);
        }
    }

    public void TryShake(GameObject target) {
        target.GetComponent<PlayerController>().ShakePlayerCamera(0.4f, playerCamShakeMaqnitude);
    }

    
}
