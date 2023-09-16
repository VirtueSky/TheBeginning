using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBoxWithObject : MonoBehaviour
{
    [Range(0.1f, 20f)] public float Length = 1.0f;
    public Transform TargetTransform;
    public Transform _head;
    public Transform _body;
    public Transform _tail;
    public Transform _archor_head;
    public Transform _archor_tail;
    public Transform _archor_left;
    public Transform _archor_right;
    public BoxCollider Target_Box;

    private void Awake()
    {
        UpdateBox();
    }

    public void UpdateBox()
    {
        Vector3 tempTransform = TargetTransform.eulerAngles;
        TargetTransform.eulerAngles = Vector3.zero;
        // Set pos for tail

        _body.localScale = new Vector3(Length, _body.localScale.y, _body.localScale.z);
        _tail.position = _archor_tail.position;
        _head.position = _archor_head.position;

        // Find center
        Vector3 centerX = (_archor_left.position + _archor_right.position) / 2;
        Target_Box.center = new Vector3(centerX.x - TargetTransform.position.x, 0, 0);

        // Set size of box collider
        Target_Box.size = new Vector3(Mathf.Abs(_archor_right.position.x - _archor_left.position.x),
            Target_Box.size.y, Target_Box.size.z);

        TargetTransform.eulerAngles = tempTransform;
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        UpdateBox();
    }
#endif
}