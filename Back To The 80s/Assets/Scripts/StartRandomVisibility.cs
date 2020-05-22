using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRandomVisibility : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        if (Random.value >= 0.5)
        {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }

}
