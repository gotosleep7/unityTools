using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniPad : MonoBehaviour
{

    public Button userBtn;
    public Button addressBtn;
    public Button portBtn;
    public Button serverPortBtn;
    public TMPro.TMP_Text inputText;
    public TMPro.TMP_InputField user;
    public TMPro.TMP_InputField address;
    public TMPro.TMP_InputField port;
    public TMPro.TMP_InputField serverPort;

    public
    // Start is called before the first frame update
    void Start()
    {
        userBtn.onClick.AddListener(OnClickUser);
        addressBtn.onClick.AddListener(OnClickIp);
        portBtn.onClick.AddListener(OnClickPort);
        serverPortBtn.onClick.AddListener(OnClickServerPort);
    }

    private void OnClickServerPort()
    {
        serverPort.text = inputText.text;
    }
    private void OnClickPort()
    {
        port.text = inputText.text;
    }

    private void OnClickIp()
    {
        address.text = inputText.text;
    }

    private void OnClickUser()
    {
        user.text = inputText.text;
    }

    public void OnClick(int n)
    {
        string str = inputText.text;
        if (n <= 9)
        {

            inputText.text += n;
            return;

        }
        else if (n == 11)
        {
            inputText.text += ".";
            return;
        }
        else if (n == 12)
        {
            if (str.Length <= 1) str = "";
            else
                str = inputText.text.Substring(0, str.Length - 1);
        }
        else if (n == 13)
        {
            str = "";
        }

        inputText.text = str;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
