using UnityEngine;

public class GoRotate : MonoBehaviour
{
    [Header("Attributes")] public bool ignoreTimeScale;
    public float speed = 1f;
    public bool rotateX;
    public bool rotateY;
    public bool rotateZ;
    public bool isReverse;

    public void FixedUpdate()
    {
        var transformTemp = transform;
        if (rotateX)
        {
            if (!isReverse)
            {
                transform.RotateAround(transform.position, transform.right, Time.deltaTime * 90f * speed);
            }
            else
            {
                transform.RotateAround(transform.position, transform.right, Time.deltaTime * 90f * -speed);
            }
        }

        if (rotateY)
        {
            if (!isReverse)
            {
                transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f * speed);
            }
            else
            {
                transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f * -speed);
            }
        }

        if (rotateZ)
        {
            if (!isReverse)
            {
                transform.RotateAround(transform.position, transform.forward, Time.deltaTime * 90f * speed * 1);
            }
            else
            {
                transform.RotateAround(transform.position, transform.forward, Time.deltaTime * 90f * speed * -1);
            }
        }
    }
}