using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;



public class ThreeDScroll : MonoBehaviour
{

    public static ThreeDScroll Instance { get; private set; }
    [SerializeField] private Transform defaultPosition;
    [SerializeField] private Transform targetPosition;
    // For open songlist flag;
    private bool openState;
    [SerializeField] private bool needToMove;
    [SerializeField] private MusicCenter.MusicCenterState currentState;
    [SerializeField] private float lerpMinDistance = 0.1f;
    [SerializeField] private float backLerpMinDistance = 0.1f;
    [SerializeField] private AnimationCurve inCurve;
    [SerializeField] private AnimationCurve outCurve;
    [SerializeField] private bool isUseCurve;
    [SerializeField] private float inTimeMax = 2.0f; // 设定期望的时间
    [SerializeField] private float outTimeMax = 2.0f; // 设定期望的时间
    [Header("Debug")]
    [SerializeField] private bool enableDeug;
    private float inTimer = 0;
    private float outTimer = 0;

    private void Awake()
    {
        Instance = this;
        openState = true;
        if (inCurve == null) inCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, inTimeMax);
        if (outCurve == null) outCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, outTimer);
    }
    // Start is called before the first frame update
    void Start()
    {
        MusicCenter.Instance.OnStateChange += MusicCenter_OnStateChange;
    }

    private void MusicCenter_OnStateChange(object sender, MusicCenter.OnStateChangeEventArgs e)
    {
        if (currentState == e.musicCenterState)
        {
            return;
        }
        if (e.musicCenterState == MusicCenter.MusicCenterState.GROUP_LIST)
        {
            openState = true;
            // box.gameObject.SetActive(false);
        }
        else
        {
            openState = false;
            // box.gameObject.SetActive(true);
        }
        needToMove = true;
        currentState = e.musicCenterState;
    }


    public bool IsMoveing()
    {
        return needToMove;
    }
    private void ForDebug()
    {
        inCurve = AnimationCurve.EaseInOut(0f, 0f, inTimeMax, 1f);
        outCurve = AnimationCurve.EaseInOut(0f, 0f, outTimeMax, 1f);
    }
    private void Update()
    {
        if (enableDeug) ForDebug();
        if (isUseCurve) HandleMoveByAnimationCurve();
        else HandleMove();
    }
    private void HandleMoveByAnimationCurve()
    {
        if (needToMove)
        {
            if (!openState)
            {
                if ((inTimer < inTimeMax) && (Vector3.Distance(transform.position, targetPosition.position) > lerpMinDistance))
                {
                    inTimer += Time.deltaTime;
                    // 使用 easeOutExpo 插值
                    float t = inCurve.Evaluate(inTimer / inTimeMax);
                    transform.position = Vector3.Lerp(defaultPosition.position, targetPosition.position, t);
                    transform.rotation = Quaternion.Lerp(defaultPosition.rotation, targetPosition.rotation, t);
                }
                else
                {
                    transform.position = targetPosition.position;
                    transform.rotation = targetPosition.rotation;
                    inTimer = 0;
                    outTimer = 0;
                    // 移动完成后的逻辑
                    needToMove = false;
                }
            }
            else
            {
                if ((outTimer < outTimeMax) && (Vector3.Distance(transform.position, defaultPosition.position) > backLerpMinDistance))
                {
                    outTimer += Time.deltaTime;
                    float t = outCurve.Evaluate(outTimer / outTimeMax);
                    transform.position = Vector3.Lerp(targetPosition.position, defaultPosition.position, t);
                    transform.rotation = Quaternion.Lerp(targetPosition.rotation, defaultPosition.rotation, t);
                }
                else
                {
                    transform.position = defaultPosition.position;
                    transform.rotation = defaultPosition.rotation;
                    outTimer = 0;
                    inTimer = 0;
                    needToMove = false;
                }
            }
        }
    }
    private void HandleMove()
    {
        if (needToMove)
        {
            if (!openState)
            {
                if (Vector3.Distance(transform.position, targetPosition.position) > lerpMinDistance)
                {
                    inTimer += Time.deltaTime;
                    // 使用 easeOutExpo 插值
                    float t = EaseInOutCirc(inTimer, outTimeMax);
                    transform.position = Vector3.Lerp(transform.position, targetPosition.position, t);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetPosition.rotation, t);
                }
                else
                {
                    transform.position = targetPosition.position;
                    transform.rotation = targetPosition.rotation;
                    inTimer = 0;
                    outTimer = 0;
                    // 移动完成后的逻辑
                    needToMove = false;
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, defaultPosition.position) > backLerpMinDistance)
                {
                    outTimer += Time.deltaTime;
                    float t = EaseInOutCirc(outTimer, outTimeMax);
                    transform.position = Vector3.Lerp(transform.position, defaultPosition.position, t);
                    transform.rotation = Quaternion.Lerp(transform.rotation, defaultPosition.rotation, t);
                }
                else
                {
                    transform.position = defaultPosition.position;
                    transform.rotation = defaultPosition.rotation;
                    outTimer = 0;
                    inTimer = 0;
                    needToMove = false;
                }
            }
        }
    }


    public bool IsOpen()
    {
        return openState;
    }

    private float EaseInOutCirc(float cTime, float tTime)
    {
        float t = cTime / tTime;
        t = t < 0.5f ? -0.5f * (Mathf.Sqrt(1 - 4 * t * t) - 1) : 0.5f * (Mathf.Sqrt(1 - 4 * (t - 1) * (t - 1)) + 1);
        return t;
    }
    private float EaseInOutQuart(float cTime, float tTime)
    {
        float t = cTime / tTime;
        t = (t < 0.5f) ? 2 * t * t * t * t : -2 * Mathf.Pow(t - 1, 4) + 1;

        return t;
    }
    private float EaseInOutExpo(float cTime, float tTime)
    {
        float t = cTime / tTime;
        t = (t < 0.5f) ? 0.5f * Mathf.Pow(2, 10 * (2 * t - 1)) : -0.5f * Mathf.Pow(2, -10 * (2 * t - 1)) + 1;
        return t;
    }
}
