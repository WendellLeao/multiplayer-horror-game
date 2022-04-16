using UnityEngine;

namespace Horror.Gameplay.Items
{
    public abstract class ItemView : MonoBehaviour
    {
        [SerializeField] private GameObject _handModelObject;

        public virtual void Begin()
        {
            _handModelObject.SetActive(true);
        }

        public virtual void Stop()
        {
            _handModelObject.SetActive(false);
        }
    }
}