using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDScrollController : MonoBehaviour
{
    public event EventHandler<float> ProgressStateChange;
    public event EventHandler<ThreeDScrollItem> OnAutoAlignComplateEvent;
    [Header("总进度")]
    [SerializeField]
    private float currentProgress;
    [Header("滑动速度")]
    [SerializeField]
    private float slideSpeed = 2f;
    [Header("自动归位时间")]
    [SerializeField]
    private float autoAlginTimeMax = 2f;
    [Header("补帧文件")]
    [SerializeField]
    private KeyDataSO frameFillerSO;
    [Header("动画关键帧")]
    [SerializeField]
    private KeyDataConfigSO keyDataConfigSO;

    [Header("滑动翻转")]
    [SerializeField]
    private bool scrollFlipdirection = false;
    [Header("循环列表")]
    [SerializeField]
    private bool loop;
    [Header("启用滑动")]
    [SerializeField] private bool enableDrag = true;
    private float stepSize;

    private float lastProgress;
    private float halfStepSize;

    private Vector3 startPosiion;
    private Vector3 lastPosiion;

    private bool draging;
    private bool needToAlign;
    private float needToStart;
    private float needToTarget;
    private float autoAlignTimer;
    private List<KeyDataSO> keyDatas = new List<KeyDataSO>();

    private ThreeDScrollItem currentThreeDScrollItem;

    public ThreeDScrollItem GetCurrentThreeDScrollItem()
    {
        return currentThreeDScrollItem;
    }
    public virtual void Awake()
    {
        HandleKeyData();
        InitData();
    }
    public bool Draging()
    {
        return draging;
    }
    public bool Alignment()
    {
        return needToAlign;
    }
    public void EnableDrag()
    {
        enableDrag = true;
    }
    public void DisableDrag()
    {
        enableDrag = false;
    }

    private void Update()
    {
        // AddProgress(Time.deltaTime * 0.5f);
        if (currentProgress != lastProgress)
        {
            lastProgress = currentProgress;
            ProgressStateChange?.Invoke(this, lastProgress);
        }
        if (needToAlign)
        {
            HandleAutoAlign();
        }
        else
        {
            if (enableDrag)
                HandleDrag();
        }

    }

    public void ClickAutoAlign(ThreeDScrollItem threeDScrollItem)
    {
        currentThreeDScrollItem = threeDScrollItem;
        if (currentProgress == 0) needToStart = 1;
        else needToStart = currentProgress;
        needToTarget = needToStart - threeDScrollItem.CurrentProgress;

        if (needToTarget != needToStart) needToAlign = true;
    }

    private void InitData()
    {
        var childCount = transform.childCount;
        stepSize = 1 / ((float)childCount);
        halfStepSize = stepSize / 2;
        // for (int i = 0; i < childCount; i++)
        // {
        //     Instantiate(transform.GetChild(i), transform);
        // }
        for (int i = 0; i < transform.childCount; i++)
        {
            var srollItem = transform.GetChild(i).GetComponent<ThreeDScrollItem>();
            if (i == 0) currentThreeDScrollItem = srollItem;
            srollItem.SetInitProgress(i * stepSize);
        }
    }

    private void HandleKeyData()
    {
        keyDatas.AddRange(keyDataConfigSO.list);
        // 如果子物体的数量小于keydata的数量，显示会出问题，
        FrameFiller();
    }
    private void HandleAutoAlign()
    {
        autoAlignTimer += Time.deltaTime;
        if (autoAlignTimer < autoAlginTimeMax)
        {
            currentProgress = Mathf.Lerp(needToStart, needToTarget, autoAlignTimer / autoAlginTimeMax);
        }
        else
        {
            currentProgress = needToTarget;
            if (currentProgress == 1 && loop) currentProgress = 0;
            autoAlignTimer = 0;
            needToAlign = false;
            OnAutoAlignComplateEvent?.Invoke(this, currentThreeDScrollItem);
        }
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosiion = Input.mousePosition;
            lastPosiion = Input.mousePosition;
            draging = true;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 dragX = (Input.mousePosition - lastPosiion).normalized * slideSpeed * Time.deltaTime;
            // 屏幕左右滑动的时候是否翻转滑动效果
            int scrollFlipdirectionFlag = scrollFlipdirection ? -1 : 1;
            AddProgress(dragX.x * scrollFlipdirectionFlag);
            lastPosiion = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            draging = false;

            if (lastPosiion != startPosiion)
            {
                CheckAutoAlign();
            }
            else
            {
                if (!draging) OnMouseButtonUp();
            }
        }
    }
    private void CheckAutoAlign()
    {
        int step = (int)(currentProgress / stepSize);
        float diff = currentProgress - step * stepSize;
        int targetStep;
        if (diff >= halfStepSize)
        {
            targetStep = step + 1;
        }
        else
        {
            targetStep = step;
        }
        needToStart = currentProgress;
        needToTarget = targetStep * stepSize;
        needToAlign = true;
    }
    public virtual void OnMouseButtonUp()
    {
        Debug.Log("OnMouseButtonUp");
    }


    private void AddProgress(float dragX)
    {
        currentProgress += dragX;
        if (loop)
        {
            if (currentProgress >= 1) currentProgress -= 1;
            else if (currentProgress < 0) currentProgress += 1;
        }
        else
        {
            if (currentProgress >= 1) currentProgress = 1;
            else if (currentProgress < 0) currentProgress = 0;
        }
    }

    internal void GetDataByProgress(float currentProgress, out KeyDataSO currentData, out KeyDataSO nextData, out float lerpValue)
    {

        int step = (int)(currentProgress / stepSize);
        currentData = null;
        nextData = null;
        lerpValue = 0;
        try
        {
            float diff = currentProgress - step * stepSize;
            int nextStep;
            if (step >= keyDatas.Count - 1)
            {
                step = keyDatas.Count - 1;
                nextStep = 0;
            }
            else
            {
                nextStep = step + 1;
            }
            currentData = keyDatas[step];
            nextData = keyDatas[nextStep];
            lerpValue = diff / stepSize;
        }
        catch (Exception e)
        {
            Debug.LogError($"currentProgress={currentProgress}/stepSize={stepSize}=step=={step}");
        }

    }


    private void FrameFiller()
    {
        int diff = transform.childCount - keyDatas.Count;
        for (int i = 0; i < diff; i++)
        {
            keyDatas.Add(frameFillerSO);
        }
    }

}
