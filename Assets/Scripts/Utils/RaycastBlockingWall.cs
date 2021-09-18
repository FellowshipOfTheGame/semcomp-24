using System;
using UnityEngine;
using UnityEngine.UI;

public class RaycastBlockingWall : MonoBehaviour
{
    private Image raycastBlockImg;

    private void Awake()
    {
        raycastBlockImg = GetComponent<Image>();
    }

    private void OnEnable()
    {
        RaycastBlockEvent.Subscribe(BlockRaycast);
    }

    private void OnDisable()
    {
        RaycastBlockEvent.Unsubscribe(BlockRaycast);
    }

    private void BlockRaycast(bool shouldBlock)
    {
        raycastBlockImg.enabled = shouldBlock;
    }
}

public static class RaycastBlockEvent
{
    private static event Action<bool> BlockRaycast;

    public static void Subscribe(Action<bool> action)
    {
        BlockRaycast += action;
    }

    public static void Unsubscribe(Action<bool> action)
    {
        BlockRaycast -= action;
    }

    public static void Invoke(bool shouldBlock)
    {
        BlockRaycast?.Invoke(shouldBlock);
    }
}