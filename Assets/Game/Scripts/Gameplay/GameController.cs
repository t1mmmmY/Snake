using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameController 
{
	//It will be fun to add a couple snakes at the same level. Like multiplayer snake game or contest with AI.
	private static List<SnakeInfo> allSnakes;

	private static bool allSnakesReady;
	private static bool levelReady;

	public static int startSnakeSize { get; private set; }

	public static System.Action onClearMap;
	public static System.Action onPrepareGame;
	public static System.Action onStartGame;
	public static System.Action onFinishGame;


	static GameController()
	{
		allSnakes = new List<SnakeInfo>();
		allSnakesReady = false;
		levelReady = false;
		startSnakeSize = 5;
	}

	public static bool AddSnake(Snake snake)
	{
		if (!ContainsSnake(snake))
		{
			//Add snake if it is not added yet
			allSnakes.Add(new SnakeInfo(snake));
			return true;
		}
		else
		{
			//Snake already added
			return false;
		}
	}

	public static bool SnakeReady(Snake snake)
	{
		if (ContainsSnake(snake))
		{
			allSnakes[GetSnakeIndex(snake)].Ready();

			if (IsGameReady())
			{
				StartGame();
			}

			return true;
		}
		else
		{
			return false;
		}
	}

	public static void LevelReady()
	{
		levelReady = true;

		if (IsGameReady())
		{
			StartGame();
		}
	}

	public static void SetStartSnakeSize(int size)
	{
		startSnakeSize = size;
	}


	public static bool IsGameReady()
	{
		//Check is all snakes ready
		allSnakesReady = IsAllSnakesReady();

		//if all snakes ready and level ready than true
		return allSnakesReady && levelReady;
	}

	public static void PrepareGame()
	{
//		ClearMap();

		if (onPrepareGame != null)
		{
			onPrepareGame();
		}
	}

	private static void ClearMap()
	{
		if (onClearMap != null)
		{
			onClearMap();
		}
	}

	public static void GameOver()
	{
		Debug.Log("GAME OVER");
		if (onFinishGame != null)
		{
			onFinishGame();
		}

		ClearMap();
	}

	private static void StartGame()
	{
		if (onStartGame != null)
		{
			onStartGame();
		}
	}

	private static bool ContainsSnake(Snake snake)
	{
		//If index == -1, than snake does not exist
		return GetSnakeIndex(snake) == -1 ? false : true;
	}

	private static int GetSnakeIndex(Snake snake)
	{
		for (int i = 0; i < allSnakes.Count; i++)
		{
			if (allSnakes[i].snake == snake)
			{
				return i;
			}
		}

		return -1;
	}

	private static bool IsAllSnakesReady()
	{
		foreach (SnakeInfo snakeInfo in allSnakes)
		{
			if (!snakeInfo.isReady)
			{
				//if at least one snake is not ready, than false
				return false;
			}
		}

		//All snakes are ready
		return true;
	}



}
