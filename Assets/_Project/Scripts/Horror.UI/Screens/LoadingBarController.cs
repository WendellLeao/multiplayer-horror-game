using Horror.ServiceLocator;
using System.Collections;
using Horror.Networking;
using UnityEngine.UI;
using UnityEngine;

namespace Horror.UI.Screens
{
    public sealed class LoadingBarController : MonoBehaviour
    {
        [SerializeField] private Image _progressBar;
        
        private float _sceneProgress;

        public void Initialize()
        {
            _sceneProgress = 0f;

            _progressBar.fillAmount = 0f;
            
            INetworkService networkService = GameServices.GetService<INetworkService>();

            if (networkService.Operation == null)
            {
                Debug.Log("Thres no operation active");
                
                return;
            }
            
            StartCoroutine(GetSceneLoadProgressRoutine(networkService.Operation));
        }
        
        private IEnumerator GetSceneLoadProgressRoutine(AsyncOperation operation)
        {
            while (!operation.isDone)
            {
                _sceneProgress = Mathf.Clamp01(operation.progress / 0.9f);
            
                _progressBar.fillAmount = _sceneProgress;

                yield return null;
            }
        }
    }
}