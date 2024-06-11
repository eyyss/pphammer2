using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Platform : MonoBehaviour
{
    public float moveDelta = 2f;
    public Transform targetTransform;
    private Vector3 startPos;
    private Vector3 targetPos;
    public AreaEffector2D effector;
    private void Start()
    {
        startPos = transform.position;
        targetPos = targetTransform.position;
    }

    private void Update()
    {
        Vector2 dir = (transform.position - targetPos).normalized;
        effector.forceMagnitude = -dir.x * 10;

        transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * moveDelta);
        float distance = Vector2.Distance(transform.position, targetPos);
        if (distance < 0.1f)
        {
            if (targetPos == startPos)
            {
                targetPos = targetTransform.position;
                return;
            }
            if (targetPos == targetTransform.position)
            {
                targetPos = startPos;
                return;
            }
        }
    }
}
