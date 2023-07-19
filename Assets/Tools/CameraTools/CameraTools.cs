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
    public float CalculateCameraOrthographicSize(float viewWidth, float viewHeight, float targetWidth, float targetHeight, float offsetRate = 0.1f)
    {
        // 计算视口宽高比和目标宽高比
        float viewAspect = viewWidth / viewHeight;
        float targetAspect = targetWidth / targetHeight;

        // 计算目标宽高中的较大值
        float length = Mathf.Max(targetWidth, targetHeight);

        // 计算临时尺寸
        float tmpSize = length * 0.5f;
        float retSize;

        // 判断是否为横屏
        if (IsLandscape(viewAspect, targetAspect))
        {
            // 根据横屏情况计算摄像机正交大小
            retSize = CalculateSizeForLandscape(viewAspect, targetAspect, targetHeight, tmpSize, viewWidth, viewHeight);
        }
        else
        {
            // 根据竖屏情况计算摄像机正交大小
            retSize = CalculateSizeForPortrait(targetAspect, viewAspect, targetWidth, tmpSize, viewWidth, viewHeight);
        }

        // 根据偏移比例调整摄像机正交大小
        retSize *= (1 + offsetRate);

        return retSize;
    }

    // 判断是否为横屏
    private bool IsLandscape(float viewAspect, float targetAspect)
    {
        // 当视口宽高比大于等于1且目标宽高比大于1时，视为横屏
        return viewAspect >= 1 && targetAspect > 1;
    }

    // 根据横屏情况计算摄像机正交大小
    private float CalculateSizeForLandscape(float viewAspect, float targetAspect, float targetHeight, float tmpSize, float viewWidth, float viewHeight)
    {
        // 如果视口宽高比减去目标宽高比大于0，则将目标高度的一半作为摄像机正交大小，否则调用 CalcSize 计算
        return viewAspect - targetAspect > 0 ? targetHeight * 0.5f : CalcSize(tmpSize, viewWidth, viewHeight);
    }

    // 根据竖屏情况计算摄像机正交大小
    private float CalculateSizeForPortrait(float targetAspect, float viewAspect, float targetWidth, float tmpSize, float viewWidth, float viewHeight)
    {
        // 如果目标宽高比减去视口宽高比大于0，则调用 CalcSize 计算，否则将临时尺寸作为摄像机正交大小
        return targetAspect - viewAspect > 0 ? CalcSize(targetWidth / 2, viewWidth, viewHeight) : tmpSize;
    }

    // 根据视口宽高比和目标宽高比计算摄像机正交大小
    private static float CalcSize(float tmpSize, float viewWidth, float viewHeight)
    {
        // 通过将临时尺寸乘以视口高度除以视口宽度，计算摄像机正交大小
        return tmpSize * viewHeight / viewWidth;
    }

}

