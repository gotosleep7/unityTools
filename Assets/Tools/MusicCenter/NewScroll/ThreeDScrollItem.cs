using System;
using UnityEngine;

public class ThreeDScrollItem : MonoBehaviour
{
    public event EventHandler OnCurrentProgressChange;
    [SerializeField]
    private float selfProgress;
    [SerializeField] private float currentProgress;
    [SerializeField] private ThreeDButton threeDButton;
    [SerializeField] private ThreeDScrollItemVisual visual;
    public float CurrentProgress => currentProgress;

    private void Awake()
    {
        threeDButton = GetComponent<ThreeDButton>();
        threeDButton.AddListener(() =>
        {
            ThreeDScrollControllerForMusic.Instance.ClickAutoAlign(this);
        });
    }
    // Start is called before the first frame update
    void Start()
    {
        ThreeDScrollControllerForMusic.Instance.ProgressStateChange += OnProgressStateChange;
    }
    public void SetInitProgress(float progress)
    {
        this.selfProgress = progress;
        currentProgress = selfProgress;
        visual.UpdateVisual();
    }
    private void OnProgressStateChange(object sender, float e)
    {
        currentProgress = selfProgress + e;
        if (currentProgress >= 1) currentProgress -= 1;
        else if (currentProgress < 0) currentProgress += 1;
        OnCurrentProgressChange?.Invoke(this, EventArgs.Empty);

    }
    public void SetInfoByProgress()
    {
        ThreeDScrollControllerForMusic.Instance.GetDataByProgress(currentProgress, out KeyDataSO currentData, out KeyDataSO nextData, out float lerpValue);
        transform.localPosition = Vector3.Lerp(currentData.position, nextData.position, lerpValue);
        transform.rotation = Quaternion.Euler(Vector3.Lerp(currentData.roatation, nextData.roatation, lerpValue));
        transform.localScale = Vector3.Lerp(currentData.scale, nextData.scale, lerpValue);
    }




}
