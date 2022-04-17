using UnityEngine;

namespace Horror.Gameplay.Cameras
{
    public interface ICameraService
    {
        Camera MainCamera { get; }
        Transform ItemContainer { get; }
        FirstPersonCamera LocalFirstPersonCamera { get; }
        FirstPersonCamera CreateFirstPersonCamera();
    }
}