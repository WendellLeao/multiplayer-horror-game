using System.Collections;
using Horror.Gameplay;
using Horror.Gameplay.SceneLoader;
using UnityEngine;

namespace Horror._Project.Scripts.Horror.Gameplay
{
	public sealed class GameplayTimerHandler : MonoBehaviour
	{
		// [Header("Countdown Timer")]
		// private int _currentTime;
		//
		// private bool _gameHasStarted = false;
		//
		// protected override void SubscribeEvents()
		// { }
		//
		// protected override void UnsubscribeEvents()
		// { }
		//
		// private void Awake()
		// {
		// 	CheckPreload();
		// }
		//
		// protected override void Initialize()
		// {
		// 	base.Initialize();
		//
		// 	StopGame(GameState.STARTING);
		//
		// 	LockCursor();
		//
		// 	InitializeCountdownTimer();
		// }
		//
		// private void CheckPreload()
		// {
		// 	if (!SaveSystem.SaveSystem.GetWasLoaded())
		// 	{
		// 		if (!SaveSystem.SaveSystem.GetWasCreated())
		// 		{
		// 			SceneHandler.LoadScene(SceneEnum.PRELOAD);
		// 		}
		//
		// 		Debug.LogWarning("Save System has been loaded forcibly. Start the game from the Preload scene!");
		// 	}
		// }
		//
		// private void InitializeCountdownTimer()
		// {
		// 	int timeToStartGame = SaveSystem.SaveSystem.GetLocalGameData().TimeToStartGame;
		//
		// 	SetCurrentTime(timeToStartGame);
		//
		// 	if (timeToStartGame > 0)
		// 	{
		// 		StartCoroutine(CountdownTimer());
		// 	}
		// 	else
		// 	{
		// 		StartGame();
		// 	
		// 		StartMatchTimer();
		// 	}
		// }
		//
		// private void HandleGameTimer()
		// {
		// 	if (_currentTime <= 0)
		// 	{
		// 		if (!_gameHasStarted)
		// 		{
		// 			StartGame();
		// 		
		// 			StartMatchTimer();
		// 		}
		// 		else
		// 		{
		// 			FinishGame();
		// 		}
		// 	}
		// }
		//
		// private IEnumerator CountdownTimer()
		// {
		// 	while (_currentTime > 0)
		// 	{
		// 		_playerGameEvents.OnGameTimeChanged?.Invoke(_currentTime);
		// 	
		// 		yield return new WaitForSecondsRealtime(1f);
		//
		// 		_currentTime--;
		// 	}
		//
		// 	_playerGameEvents.OnGameTimeChanged?.Invoke(0);
		//
		// 	HandleGameTimer();
		// }
		//
		// private void StartGame()
		// {
		// 	_gameHasStarted = true;
		//
		// 	ResumeGame();
		// 	
		// 	_globalGameEvents.OnGameIsStarted?.Invoke();
		// }
		//
		// private void StartMatchTimer()
		// {
		// 	int gameDuration = SaveSystem.SaveSystem.GetLocalGameData().GameDuration;
		// 	
		// 	SetCurrentTime(gameDuration);
		//
		// 	StartCoroutine(CountdownTimer());
		// }
		//
		// private void FinishGame()
		// {
		// 	_globalGameEvents.OnGameIsFinished?.Invoke();
		//
		// 	StopGame(GameState.LEVEL_COMPLETED);
		// }
		//
		// private void SetCurrentTime(int time)
		// {
		// 	_currentTime = time;
		// }
	}
}
