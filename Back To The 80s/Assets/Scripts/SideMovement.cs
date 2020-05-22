using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMovement : MonoBehaviour
{

        private float xRange = 50f;

        public float sideSpeed = 10f;


    // Start is called before the first frame update
    void Start()
    {
        
        sideSpeed += GameManager.boxSideSpeedIncrement;

        int randStartSpeed = Random.Range(0, 4);
        if (GameManager.debugIsOn) {
            Debug.Log("New rand n" +randStartSpeed);
        }
        if (randStartSpeed > 2) {
            sideSpeed = sideSpeed -(sideSpeed * 2);
        } else {
            // Nothing at the moment...
        }


        
    }

    // Update is called once per frame
    void Update()
    {
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
}
