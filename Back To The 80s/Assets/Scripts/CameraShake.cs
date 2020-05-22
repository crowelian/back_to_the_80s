using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public bool isShaking = false;
    public GameObject player;

    public IEnumerator Shake(float duration, float magnitude)
    {
        isShaking = true;
        Vector3 orignalPosition = transform.position;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(transform.position.x + x,transform.position.y + y, player.transform.position.z);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        isShaking = false;
        transform.position = orignalPosition;
    }
}
