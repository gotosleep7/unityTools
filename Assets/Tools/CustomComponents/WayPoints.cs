using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    [SerializeField] private List<Transform> wayPoints;
    public List<Transform> GetWayPoints()
    {
        return wayPoints;
    }
}
