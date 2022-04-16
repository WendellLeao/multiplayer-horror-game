using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Horror.Gameplay.SceneLoader
{
	public sealed class AsyncSceneHandler : MonoBehaviour
	{
		private AsyncOperation _asyncOperation;

		private float _normalizedAsyncOperationProgress;
    
		public void LoadAdditiveSceneAsync(SceneEnum sceneEnum)
		{
			int sceneEnumToInt = (int)sceneEnum;
		
			StartCoroutine(LoadAsynchronously(sceneEnumToInt, LoadSceneMode.Additive));
		}

		public void LoadSingleSceneAsync(int sceneIndex)
		{
			StartCoroutine(LoadAsynchronously(sceneIndex, LoadSceneMode.Single));
		}
    
		public void LoadSingleSceneAsync(SceneEnum sceneEnum)
		{
			int sceneEnumToInt = (int)sceneEnum;
		
			StartCoroutine(LoadAsynchronously(sceneEnumToInt, LoadSceneMode.Single));
		}

		private IEnumerator LoadAsynchronously(int sceneIndex, LoadSceneMode loadSceneMode)
		{
			_asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, loadSceneMode);
		
			while (!_asyncOperation.isDone)
			{
				_normalizedAsyncOperationProgress = Mathf.Clamp01(_asyncOperation.progress / 0.9f);
            
				yield return null;
			}

			Destroy(gameObject);
		}
    
		public float GetNormalizedOperationProgress()
		{
			return _normalizedAsyncOperationProgress;
		}
	}
}
