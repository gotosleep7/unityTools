using System.Collections.Generic;
using UnityEngine;

public class TestSnakeSkill : MonoBehaviour
{
    public float speed = 5f; // 蛇的移动速度  
    public float bodyBend = 0.5f; // 身体的弯曲程度  

    public Transform head; // 蛇的头部Transform  
    List<Transform> segments; // 蛇的身体部分列表  
    public Transform segmentsPre; // 蛇的身体部分列表  
    public Transform tailPre; // 蛇的身体部分列表  
    public float segmentSpacing = 0.2f;
    public int maxLength = 5;
    float timer;
    private Vector3 direction = Vector3.up; // 蛇的移动方向  

    void Start()
    {
        segments.Add(Instantiate(head));
        for (int i = 1; i < segments.Count; i++)
        {
            segments[i].position = segments[i - 1].position - new Vector3(0, 0, 0);
        }
        // moveDis = bodyBend;
    }
    float moveDis;
    void Update()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 newdir = new Vector2(h, v).normalized;
        if (newdir != Vector2.zero) direction = newdir;

        for (int i = 0; i < segments.Count; i++)
        {
            if (i == 0)
            {
                segments[i].position += direction * speed * Time.deltaTime;
            }
            else
            {
                // 计算当前段和前一段之间的方向差异  
                Vector3 directionDiff = (segments[i - 1].position - segments[i].position).normalized;

                // 根据方向差异和前一段的位置计算目标位置  
                Vector3 targetPosition = segments[i - 1].position - (directionDiff * bodyBend);

                // 使用MoveTowards平滑地移动当前段到目标位置  
                segments[i].position = Vector2.MoveTowards(segments[i].position, targetPosition, speed * Time.deltaTime);
            }
        }
        moveDis += Time.deltaTime * speed;
        if (moveDis > bodyBend && segments.Count < maxLength)
        {
            if (segments.Count == maxLength - 1)
            {
                AddTail();
            }
            else
            {

                AddBody();
            }
            moveDis = 0;
        }
    }


    public void AddBody()
    {
        var newBody = Instantiate(segmentsPre).transform;
        Vector3 dir;
        if (segments.Count < 2)
        {
            dir = -direction;
        }
        else
        {
            dir = segments[segments.Count - 1].position - segments[segments.Count - 2].position;
        }

        newBody.position = segments[segments.Count - 1].position + dir.normalized * bodyBend;
        segments.Add(newBody);
    }
    public void AddTail()
    {
        var newBody = Instantiate(tailPre).transform;
        Vector3 dir = segments[segments.Count - 1].position - segments[segments.Count - 2].position;
        newBody.position = segments[segments.Count - 1].position + dir.normalized * bodyBend;
        segments.Add(newBody);
    }
}