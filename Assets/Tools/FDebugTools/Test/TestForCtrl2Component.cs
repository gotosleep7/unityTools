using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForCtrl2Component : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if (Time.frameCount % 100 == 0)
        // {
        //     Debug.Log("aaasf");
        // }
    }
    public void Log(int value)
    {
        Debug.Log($"form sync int value={value}");
    }
    public void Log(float value)
    {
        Debug.Log($"form sync float value={value}");
    }
    public void Log(string value)
    {
        Debug.Log($"form sync string value={value}");
    }
    public void Log(Vector3 value, float vvv)
    {
        transform.rotation *= Quaternion.Euler(value);
    }
}
