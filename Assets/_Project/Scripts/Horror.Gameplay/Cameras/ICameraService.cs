using UnityEngine;

namespace Horror.Gameplay.Cameras
{
    public interface ICameraService
    {
        public Camera MainCamera { get; }
        public Transform ItemContainer { get; }
        FirstPersonCamera CreateFirstPersonCamera();
    }
}