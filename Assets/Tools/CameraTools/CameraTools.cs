using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTools : MonoBehaviour
{
    Camera mainCamera;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (target == null) return;
            float viewWidth = Screen.width;
            float viewHeight = Screen.height;
            float targetWidth = Mathf.Abs(target.localScale.x);
            float targetHeight = Mathf.Abs(target.localScale.y);

            var size = CalculateCameraOrthographicSize(viewWidth, viewHeight, targetWidth, targetHeight, 0.2f);
            mainCamera.orthographicSize = size;
            var cameraPos = mainCamera.transform.position;
            cameraPos.x = target.position.x;
            cameraPos.z = target.position.z;
            // mainCamera.transform.position = cameraPos;

        }
    }

    /// <summary>
    /// 根据传入参数，计算正交相机要在某个分辨率下，完整显示某个区域所需的size。
    /// </summary>
    /// <param name="viewWidth">分辨率的宽，或者比例</param>
    /// <param name="viewHeight">分辨率的高，或者比例</param>
    /// <param name="targetWidth">要显示区域的尺寸</param>
    /// <param name="targetHeight">要显示区域的尺寸</param>
    /// <param name="offsetRate">留白区域比例</param>
    public static float CalculateCameraOrthographicSize(float viewWidth, float viewHeight, float targetWidth, float targetHeight, float offsetRate = 0.1f)
    {
        float viewAspect = viewWidth / viewHeight;
        float targetAspect = targetWidth / targetHeight;
        float length = targetWidth > targetHeight ? targetWidth : targetHeight;
        float tmpSize = length * .5f;
        float retSize;
        // 是否横屏
        if (viewAspect >= 1)
        {
            if (targetAspect > 1)
            {
                if (viewAspect - targetAspect > 0)
                    retSize = targetHeight * 0.5f;
                else
                    retSize = CalcSize(tmpSize, viewWidth, viewHeight);
            }
            else
            {
                retSize = tmpSize;
            }
        }
        else
        {
            // 判断是否是纵向的
            if (targetAspect < 1)
            {
                // 如果屏幕的宽高比 小于物体的宽高比
                if (targetAspect - viewAspect > 0)
                {
                    tmpSize = targetWidth / 2;
                    retSize = CalcSize(tmpSize, viewWidth, viewHeight);
                }
                else
                    retSize = tmpSize;
            }
            else
            {
                retSize = CalcSize(tmpSize, viewWidth, viewHeight);
            }
        }
        retSize += retSize * offsetRate;
        return retSize;
    }

    private static float CalcSize(float tmpSize, float viewWidth, float viewHeight)
    {
        return tmpSize * viewHeight / viewWidth;
    }
}

