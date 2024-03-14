using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongWayPointsExt : MonoBehaviour
{
    public event EventHandler<float> OnProcessChanged;
    [SerializeField] private List<Transform> wayPoints;
    [SerializeField] private Transform moveTarget;
    [SerializeField] private Transform lookTarget;
    [SerializeField] public int numberOfPoints = 100;
    [SerializeField] private float animationTimeMax = 2f;
    [SerializeField] AnimationCurve animationCurve;
    [Header("For Debug")]
    [SerializeField] bool drawPath;
    [SerializeField] List<WayPointsConfigSO> wayConfigList;
    private float process;
    private float currentProcess;
    private int currentStep;
    private int pointCurrentStep;
    int switchingFlag;
    int currentSwitchingFlag;
    bool breakAnima;
    float inTimer;
    private List<Vector3> retPoints;
    private void Start()
    {
        var c = wayConfigList[0];
        // c.InitPoints();
        transform.position = c.GetWayPoints()[0];
        // stepAmout = wayPoints.Count - 1;
        // stepSize = 1f / stepAmout;
        // GenerateBezierCurves();
        // SetDataByProcess(0, moveTarget);
        // OnProcessChanged?.Invoke(this, 0);
    }

    public void SetDataByProcess(float value, Transform trans)
    {
        SetPosition(value, trans);
        SetRotation(value, trans);
    }
    private void SetPosition(float value, Transform trans)
    {
        // pointCurrentStep = (int)(value / pointStepSize);
        // float lerpValue = value % pointStepSize / pointStepSize;
        // if (pointCurrentStep == pointStepAmout) return;
        // trans.localPosition = Vector3.Lerp(retPoints[pointCurrentStep], retPoints[pointCurrentStep + 1], lerpValue);
    }
    private void SetRotation(float value, Transform trans)
    {
        // if (lookTarget != null)
        // {
        //     trans.LookAt(lookTarget);
        //     return;
        // }
        // currentStep = (int)(value / stepSize);
        // float lerpValue = value % stepSize / stepSize;
        // if (currentStep == stepAmout) return;
        // if (lookTarget == null) trans.rotation = Quaternion.Lerp(wayPoints[currentStep].rotation, wayPoints[currentStep + 1].rotation, lerpValue);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("相机移动路径绘制调试信息");
            ZeroToOne();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("相机移动路径绘制调试信息2");
            OneToZero();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("相机移动路径生成调试信息");
            // GenerateBezierCurves();
        }
        if (currentProcess != process)
        {
            currentProcess = currentProcess > 1 ? 1 : currentProcess;
            currentProcess = currentProcess < 0 ? 0 : currentProcess;
            process = currentProcess;
            OnProcessChanged?.Invoke(this, process);
            SetDataByProcess(process, moveTarget);
        }
        if (switchingFlag != 0)
        {
            if (breakAnima)
            {
                // 如果要实现移动到一半要回去的话，就不能使用两条曲线分别控制
                if (inTimer != 0) inTimer = 2f - inTimer;
                breakAnima = false;
            }
            {
                if (switchingFlag == 1)
                {
                    if (inTimer < animationTimeMax)
                    {
                        inTimer += Time.deltaTime;
                        currentProcess = animationCurve.Evaluate(inTimer / animationTimeMax);
                    }
                    else
                    {
                        inTimer = 0;
                        switchingFlag = 0;
                    }
                }
                else if (switchingFlag == -1)
                {
                    if (inTimer < animationTimeMax)
                    {
                        inTimer += Time.deltaTime;
                        currentProcess = animationCurve.Evaluate(1 - (inTimer / animationTimeMax));
                    }
                    else
                    {
                        inTimer = 0;
                        switchingFlag = 0;
                    }
                }
            }


        }
    }

    // [ContextMenu("ZeroToOne")]
    private void ZeroToOne()
    {
        switchingFlag = 1;
        if (currentSwitchingFlag != switchingFlag)
        {
            breakAnima = true;
            currentSwitchingFlag = switchingFlag;
        }
    }
    // [ContextMenu("OneToZero")]
    private void OneToZero()
    {
        switchingFlag = -1;
        if (currentSwitchingFlag != switchingFlag)
        {
            breakAnima = true;
            currentSwitchingFlag = switchingFlag;
        }
    }


    #region For Debug
    void OnDrawGizmos()
    {
        if (drawPath)
            DrawBezierCurve();
    }

    void DrawBezierCurve()
    {
        for (int i = 0; i < retPoints.Count; i++)
        {
            Gizmos.DrawSphere(retPoints[i], 0.1f);
        }
    }
    #endregion


}
