using UnityEngine.SceneManagement;

namespace Horror.Gameplay.SceneLoader
{
	public static class SceneHandler
	{
		public static void LoadScene(int sceneIndex)
		{
			SceneManager.LoadScene(sceneIndex);
		}
    
		public static void LoadScene(SceneEnum sceneEnum)
		{
			int sceneEnumToInt = (int)sceneEnum;
        
			SceneManager.LoadScene(sceneEnumToInt);
		}

		public static void LoadNextScene()
		{
			LoadScene(GetNextSceneIndex());
		}
    
		public static void ReloadScene()
		{
			LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		public static void BackToMainMenu()
		{
			LoadScene(SceneEnum.PRELOAD);
		}

		public static bool NextSceneExists()
		{
			return GetNextSceneIndex() < SceneManager.sceneCountInBuildSettings;
		}

		public static int GetActiveSceneIndex()
		{
			return SceneManager.GetActiveScene().buildIndex;
		}
    
		public static int GetNextSceneIndex()
		{
			return SceneManager.GetActiveScene().buildIndex + 1;
		}
	}
}
