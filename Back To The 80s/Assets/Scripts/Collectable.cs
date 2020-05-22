using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Collectable : MonoBehaviour
{

  
    public enum PowerUpType {Health, Nitro, Score};
    
    public PowerUpType selectPowerUp;
    public Vector3 offset;

    [Header("Health is this float, score is multiplied by 1000")]
    public float powerUpAmount = 25f;

    void Start() {
        transform.position = transform.position + offset; // set the starting position offset
        GameManager.canSpawnPowerUp = false; // if spawned cannot spawn another for some time...
    }

    void OnTriggerEnter(Collider other) {

        if((other.gameObject.tag == "Player" && other.gameObject.tag != "RoadSpawner") || (other.gameObject.tag == "Player" && other.gameObject.tag != "RoadDestroyer")) {
            
            
            Debug.Log("TODO: ADD COLLECT PARTICLE to this!");

            if (selectPowerUp == PowerUpType.Health) {
                // powerUpAmount will decide how much health player will get
                other.gameObject.GetComponent<PlayerController>().AddHealthPowerUp(powerUpAmount);
            } else if (selectPowerUp == PowerUpType.Nitro) {
                // The nitro will fill up 100% everytime!!!
                other.gameObject.GetComponent<PlayerController>().AddSpeedPowerUp(); 
            } else if (selectPowerUp == PowerUpType.Score) {
                // powerUpAmount will decide how much extra scrore player will get
                other.gameObject.GetComponent<PlayerController>().AddScorePowerUp((powerUpAmount*1000f));
            }
            

            if (GameManager.debugIsOn) {
                Debug.Log("Collected powerup: " + selectPowerUp.ToString());
            }
            Destroy(this.gameObject);
        }

        if (other.gameObject.tag == "Enemy") {
            Destroy(gameObject);
        }
    }


}



