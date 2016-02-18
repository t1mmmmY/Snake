using UnityEngine;
using System.Collections;

public class LevelSettings : BaseSingleton<LevelSettings> 
{
	[SerializeField] Vector2 levelSize = Vector2.zero;

	[Range(0, 100)]
	[SerializeField] int numberOfObstacles = 10;

	[Range(0, 20)]
	[SerializeField] int startSnakeSize = 5;

	//Only one type for now
	[SerializeField] Obstacle obstaclePrefab;


	void Start()
	{
		SetLevelSize();

		GameController.onClearMap += ClearMap;
		GameController.onPrepareGame += OnPrepareGame;

		GameController.SetStartSnakeSize(startSnakeSize);
	}

	void OnDestroy()
	{
		GameController.onClearMap -= ClearMap;
		GameController.onPrepareGame -= OnPrepareGame;
	}

	public Vector2 GetLevelSize()
	{
		return levelSize;
	}

	void SetLevelSize()
	{
		LevelManager.Instance.SetLevelSize(levelSize);
	}

	void ClearMap()
	{
		LevelManager.Instance.ClearMap();
	}

	void OnPrepareGame()
	{
		CreateObstacles();
	}

	void CreateObstacles()
	{
		LevelManager.Instance.CreateObstacles(obstaclePrefab, numberOfObstacles);
	}

}
