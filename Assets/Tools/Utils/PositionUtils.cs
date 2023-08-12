using UnityEngine;

public static class PositionUtils
{
    public static Vector3 GenerateRandomPosiion(Vector3 center, Vector2 minAndMaxArea)
    {
        float minRadius = minAndMaxArea.x;
        float maxRadius = minAndMaxArea.y;

        // 在 minRadius 到 maxRadius 范围内生成随机半径
        float randomRadius = UnityEngine.Random.Range(minRadius, maxRadius);

        // 在 0 到 2*pi 之间生成随机角度
        float randomAngle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);

        // 计算随机位置的 x 和 y 分量
        float x = randomRadius * Mathf.Cos(randomAngle);
        float y = randomRadius * Mathf.Sin(randomAngle);

        // 创建随机位置向量
        Vector3 pos = new(x, y, 0f);

        return pos + center;

    }
}