using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    
    public GameObject player;
    public Vector3 offset = new Vector3(0,5,-7);
    public float smoothness = 13f;
    

    /*
    public Transform player;
    public float distance = 3.0f;
    public float height = 3.0f;
    public float damping = 5.0f;
    public bool smoothRotation = true;
    public bool followBehind = true;
    public float rotationDamping = 10.0f;
    */

    // Update is called once per frame
    void Update()
    {


        




        /*
        // DID NOT WORK! was veeeeeeeery jerky!
        //
        if (player != null) {
            if (GetComponent<CameraShake>().isShaking) {
                // Camera is shaking.
            } 
            else {
                Vector3 wantedPosition;
                if(followBehind) {
                    wantedPosition = player.TransformPoint(0, height, -distance);
                }
                            
                else {
                    wantedPosition = player.TransformPoint(0, height, distance);
                }
                            
            
                transform.position = Vector3.Lerp (transform.position, wantedPosition, Time.deltaTime * damping);

                if (smoothRotation) {
                    Quaternion wantedRotation = Quaternion.LookRotation(player.position - transform.position, player.up);
                    transform.rotation = Quaternion.Slerp (transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
                }
                else {
                    transform.LookAt (player, player.up);
                }
                
            }
        }

        */


        //transform.position = player.transform.position + offset;

        /*
        // Compute our exponential smoothing factor.
        float blend = 1f - Mathf.Pow(1f - smoothness, Time.deltaTime * 30f);

        transform.position = Vector3.Lerp(
            transform.position,
            player.transform.position + offset,
            blend);
        */
    }

    void LateUpdate()
    {






            
            if (player != null) {
                if (GetComponent<CameraShake>().isShaking) {
                    
                } else {
                    Vector3 desiredPosition = player.transform.position + offset;
                    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothness * 2f);
                    transform.position = smoothedPosition;
                }
                
            }
            

            
    }





}
