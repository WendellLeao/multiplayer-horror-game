using System.Collections;
using UnityEngine;

namespace Horror.Gameplay.Scenary
{
    public sealed class Lamp : Entity, IIlluminable
    {
        [SerializeField] private Light _light = default;
        [SerializeField] private GameObject _lampView;
        [SerializeField] private float _delayInSeconds;

        public void SetIntensity(float endValue)
        {
            StartCoroutine(SetIntensityCoroutine(endValue));
        }

        private IEnumerator SetIntensityCoroutine(float endValue)
        {
            yield return new WaitForSeconds(_delayInSeconds);

            _light.intensity = endValue;

            if (endValue <= 0f)
            {
                _lampView.SetActive(false);
            }
        }
    }
}
