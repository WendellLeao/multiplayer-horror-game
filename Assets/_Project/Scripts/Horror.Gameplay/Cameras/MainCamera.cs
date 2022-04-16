using UnityEngine;

namespace Horror.Gameplay.Cameras
{
    public sealed class MainCamera : MonoBehaviour
    {
        [SerializeField] private Transform _itemContainer;

        public Transform ItemContainer => _itemContainer;
    }
}