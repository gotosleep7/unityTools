using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ThreeDButton : MonoBehaviour
{
    public Action OnClick;

    public void Click()
    {
        OnClick?.Invoke();
    }

    public void AddListener(Action fun)
    {
        OnClick = fun;
    }
    public void RemoveListener()
    {
        OnClick = null;
    }

}