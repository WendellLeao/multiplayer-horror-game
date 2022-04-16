using Horror.Gameplay.SceneLoader;
using UnityEngine.UI;
using UnityEngine;

namespace Horror.UI
{
	public sealed class LoadingBarUI : MonoBehaviour
	{
		[Header("Async Scene Handler")]
		[SerializeField] private AsyncSceneHandler _asyncSceneHandler;

		[Header("Loading Bar")]
		[SerializeField] private Slider _slider;

		private void Update()
		{
			UpdateLoadingBar();
		}

		private void UpdateLoadingBar()
		{
			_slider.value = _asyncSceneHandler.GetNormalizedOperationProgress();
		}
	}
}
