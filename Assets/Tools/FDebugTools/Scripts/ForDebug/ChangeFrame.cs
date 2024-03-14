using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FDebugTools
{
    public class ChangeFrame : MonoBehaviour
    {
        // ProcessStateManager processStateManager;
        private void Awake()
        {
            // processStateManager = GetComponent<ProcessStateManager>();
        }


        public void SetFrame(int targetFrameRate)
        {
            Application.targetFrameRate = targetFrameRate;
            Debug.Log($"Application.targetFrameRate={Application.targetFrameRate}");
        }


    }
}
