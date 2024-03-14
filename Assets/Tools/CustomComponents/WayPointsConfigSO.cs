using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WayPointsConfigSO", menuName = "Tools/WayPointsConfig")]
public class WayPointsConfigSO : ScriptableObject
{
    public string wayName;
    public WayPoints way;
    public int numberOfPoints;
    public float duration;
    public AnimationCurve animationCurve;
    private List<Vector3> points;
    public int StepAmout { get; set; }
    public float StepSize { get; set; }
    public int PointStepAmout { get; set; }
    public float PointStepSize { get; set; }
    public void InitPoints()
    {
        points = new List<Vector3>();
        var wayPoints = way.GetWayPoints();
        for (int i = 0; i < wayPoints.Count - 1; i++)
        {
            points.AddRange(BezierUtils.GetCubicBeizerList(
                    wayPoints[i].position,
                    BezierUtils.AutoCalculateCubicBezierPoint(wayPoints[i].position, wayPoints[i + 1].position),
                    wayPoints[i + 1].position,
                    numberOfPoints
                  ));
        }

        PointStepAmout = points.Count - 1;
        PointStepSize = 1f / PointStepAmout;
        StepAmout = wayPoints.Count - 1;
        StepSize = 1f / StepAmout;
    }

    public List<Vector3> GetWayPoints()
    {
        if (points == null || points.Count == 0)
        {
            InitPoints();
        }
        return points;
    }

}
