using FantasticLog;
using UnityEngine;
namespace FantasticLog
{
    public class LogController : MonoBehaviour
    {
        private void Awake()
        {
            Debug.unityLogger.logHandler = LocalOrNetLogHandler.Instance;
            Debug.unityLogger.logEnabled = true;
        }

        // private void Update()
        // {
        //     Debug.Log("test log");
        // }
    }
}
