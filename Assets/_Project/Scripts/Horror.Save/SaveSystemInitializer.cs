using Horror.Gameplay.SceneLoader;
using UnityEngine;

namespace Horror.SaveSystem
{
	public sealed class SaveSystemInitializer : MonoBehaviour
	{
		[Header("Async Scene Handler")]
		[SerializeField] private AsyncSceneHandler _asyncSceneHandler;

		private void Awake()
		{
			InitializeSaveSystem();
	
			LoadAsyncScene();
		}

		private void InitializeSaveSystem()
		{
			SaveSystem.LoadGameData();
		}
	
		private void LoadAsyncScene()
		{
			_asyncSceneHandler.LoadAdditiveSceneAsync(SceneEnum.MAIN_MENU);
		}
	}
}
