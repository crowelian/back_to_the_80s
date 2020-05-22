using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheel : MonoBehaviour
{

    private float horizontalInput;
    private Vector3 offset = new Vector3(-60,-180,0);
    private float turnSpeed = 2000.0f;
    private float steeringWheelAngle = -30f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        // NOT WORKING!!! The 3d model scaling is wrong and the rotation is not working right!
        transform.localRotation = Quaternion.Euler(steeringWheelAngle,0, offset.z + ((horizontalInput * 4) * turnSpeed * Time.deltaTime));


    }
}
