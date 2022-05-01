using UnityEngine;

namespace Horror.Gameplay.Cameras
{
    public interface ICamera
    {
        Transform CameraTarget { get; } 
        void SetTarget(Transform target);
    }
}