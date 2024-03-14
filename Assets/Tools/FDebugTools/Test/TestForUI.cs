using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestForUI : MonoBehaviour
{
    public TMPro.TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnReceiveMsg(string msg)
    {
        if (text != null)
            text.text = msg;
    }
}
