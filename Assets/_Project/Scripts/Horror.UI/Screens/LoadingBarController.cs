using Horror.ServiceLocator;
using System.Collections;
using Horror.Networking;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;

namespace Horror.UI.Screens
{
    public sealed class LoadingBarController : MonoBehaviour
    {
        [SerializeField] private Image _progressBar;
        [SerializeField] private float _fillDuration = 1f;
        
        private float _sceneProgress;

        public void Initialize()
        {
            _sceneProgress = 0f;

            _progressBar.fillAmount = 0f;
            
            INetworkService networkService = GameServices.GetService<INetworkService>();

            if (networkService.Operation == null)
            {
                Debug.LogWarning("There's no async operation active");
                
                return;
            }
            
            StartCoroutine(GetUpdateProgressBarRoutine(networkService.Operation));
        }
        
        private IEnumerator GetUpdateProgressBarRoutine(AsyncOperation operation)
        {
            while (!operation.isDone)
            {
                _sceneProgress = Mathf.Clamp01(operation.progress / 0.9f);
            
                _progressBar.DOFillAmount(_sceneProgress, _fillDuration);

                yield return null;
            }
        }
    }
}