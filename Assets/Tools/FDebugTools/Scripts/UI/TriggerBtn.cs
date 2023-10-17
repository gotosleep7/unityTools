using UnityEngine;
using UnityEngine.UI;
namespace FDebugTools
{
    public class TriggerBtn : MonoBehaviour
    {
        [SerializeField] GameObject logGo;
        [SerializeField] Button btn;
        [SerializeField] Button closeBtn;
        int count;
        int timer = 3;
        float startTime;
        // Start is called before the first frame update
        void Start()
        {
            btn.onClick.AddListener(OnClick);
            closeBtn.onClick.AddListener(OnCloseClick);
        }

        private void OnCloseClick()
        {
            logGo.SetActive(false);
            closeBtn.gameObject.SetActive(false);
            count = 0;
        }

        private void OnClick()
        {
            if (count == 0)
            {
                startTime = Time.time;
            }
            count++;
            if (count > 2)
            {
                var endTime = Time.time - startTime;
                if (endTime < timer)
                {
                    logGo.SetActive(true);
                    closeBtn.gameObject.SetActive(true);
                }
                count = 0;
            }
        }
    }
}
