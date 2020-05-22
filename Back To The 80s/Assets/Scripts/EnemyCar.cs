using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCar : Enemy
{

    public float speed = 60f;
    public float xRange = 60f;

    public float sideSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = transform.position + offset; // set the starting position offset
        transform.rotation = Quaternion.Euler(startRot.x, startRot.y, startRot.z); // Set the starting rotation (if needed)
        sideSpeed += GameManager.enemySpeedIncrement;

        if (!GameManager.enemyCarCanSpawn) {
            Destroy(gameObject);
        } else {
            GameManager.enemyCarCanSpawn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        transform.Translate(Vector3.forward * Time.deltaTime * speed);
           
        transform.Translate(Vector3.right * Time.deltaTime * sideSpeed);

        if (transform.position.x < -xRange) {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
            sideSpeed = sideSpeed -(sideSpeed * 2);
        }
        if (transform.position.x > xRange) {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
            sideSpeed = sideSpeed -(sideSpeed * 2);
        }
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
    }
}
