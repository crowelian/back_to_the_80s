using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumAnalyzer : MonoBehaviour
{

    /********** ############################

        THIS code is from I great book I bought!
                Unity 2018 Cookbook
            Helped me MANY MANY MANY TIMES!


            NOTICE: This code was
            intended in game, but due
            to some problems I could not
            use this... BUT wanted to use
            this in the main menu instead.

    ################## *************######*/

    const int NUM_SAMPLES = 256; // was 512
    public Color displayColor;
    public float multiplier = 5000;
    public float startY;
    public float maxHeight = 50;
    public AudioSource audioSource;
    private float[] spectrum = new float[NUM_SAMPLES];
    private GameObject[] cubes = new GameObject[NUM_SAMPLES];
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        CreateCubes();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.GetSpectrumData(spectrum, 0,
        FFTWindow.BlackmanHarris);
        UpdateCubeHeights();
    }


    private void UpdateCubeHeights() {
        for (int i = 0; i < NUM_SAMPLES; i++)
        {
            Vector3 oldScale = cubes[i].transform.localScale;
            Vector3 scaler = new Vector3(oldScale.x,
            HeightFromSample(spectrum[i]), oldScale.z);
            cubes[i].transform.localScale = scaler;
            Vector3 oldPosition = cubes[i].transform.position;
            float newY = startY +
            cubes[i].transform.localScale.y / 2;
            Vector3 newPosition = new Vector3(oldPosition.x,
            newY, oldPosition.z);
            cubes[i].transform.position = newPosition;
        }
    }


    private float HeightFromSample(float sample) {
        float height = 2 + (sample * multiplier);
        return Mathf.Clamp(height, 0, maxHeight);
    }

   private void CreateCubes() {
        for (int i = 0; i < NUM_SAMPLES; i++) {
            GameObject cube = Instantiate(prefab, transform.position, Quaternion.identity);
            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = transform;
            cube.name = "SampleCube" + i;
            Renderer cubeRenderer =
            cube.GetComponent<Renderer>();
            cubeRenderer.material = new Material(Shader.Find("Specular"));
            cubeRenderer.sharedMaterial.color = displayColor;
            float x = transform.position.x + 0.9f * i;
            float y = transform.position.y + startY;
            float z = transform.position.z + 0;
            cube.transform.position = new Vector3(x, y, z);
            cube.transform.rotation = transform.rotation;
            cubes[i] = cube;
    }
   }
}
