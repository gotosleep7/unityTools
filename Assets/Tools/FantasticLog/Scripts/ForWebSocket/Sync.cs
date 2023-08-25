using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FantasticLog
{
    public enum ValueType
    {
        Float = 0, Color
    }
    public class Sync : MonoBehaviour
    {
        public string targetUser;
        public string gameobject;
        public ValueType valueType;
        public string fieldName;
        public string value;
        public Color color;
        public float updateRate;
        public bool isSync;

        private string messageType = "1";
        public float timer;
        private void Update()
        {
            if (isSync)
            {
                timer += Time.deltaTime;
                if (timer >= updateRate)
                {

                    WsLogLogic.Instance.SendWsMessage(ConvertInfo());
                    timer = 0;
                }
            }

        }
        public string ConvertInfo()
        {
            if (targetUser.Equals("")) { return null; }
            string dataStr = $"{targetUser}|{messageType}|{gameobject}|{(int)valueType}|{fieldName}|{value}|{color.r},{color.g},{color.b},{color.a}";
            return dataStr;
        }
    }
}