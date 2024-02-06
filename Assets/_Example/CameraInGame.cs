using System;
using UnityEngine;
using VirtueSky.Core;

public class CameraInGame : BaseMono
{
    public GameObject player;
    private Vector3 offset;

    private void Awake()
    {
        offset = transform.position - player.transform.position;
    }

    public override void LateTick()
    {
        base.LateTick();
        transform.position = player.transform.position + offset;
    }
}