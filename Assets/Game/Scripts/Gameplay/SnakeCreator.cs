using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeCreator : MonoBehaviour 
{
	[SerializeField] Snake snakePrefab;
	[SerializeField] Vector2 startSnakePosition = Vector2.zero;

	List<Snake> allSnakes;
	SnakeController playerSnakeController;

	//Only one for now
	int countSnakes = 1;

	void Awake()
	{
		allSnakes = new List<Snake>();
	}

	void Start()
	{
		GameController.onClearMap += ClearMap;
		GameController.onPrepareGame += OnPrepareGame;
		GameController.onStartGame += OnStartGame;

	}

	void OnDestroy()
	{
		GameController.onClearMap -= ClearMap;
		GameController.onPrepareGame -= OnPrepareGame;
		GameController.onStartGame -= OnStartGame;
	}

	void ClearMap()
	{
		foreach(Snake snake in allSnakes)
		{
			Destroy(snake.gameObject);
		}
		allSnakes = new List<Snake>();
	}

	void OnPrepareGame()
	{
		Factory.Instance.CreateObjectsAsync<Snake>(snakePrefab, countSnakes, OnCreateSnake, OnFinishCreatingSnakes, true);
	}

	void OnCreateSnake(Snake snake)
	{
		allSnakes.Add(snake);

		snake.transform.localPosition = new Vector3(startSnakePosition.x, snake.transform.localPosition.y, startSnakePosition.y);

		//Attach snake controller to the first snake
		if (allSnakes.Count == 1)
		{
			playerSnakeController = snake.gameObject.AddComponent<SnakeController>();

			playerSnakeController.Init(snake);
		}

		snake.Deactivate();
	}

	void OnFinishCreatingSnakes()
	{
	}

	void OnStartGame()
	{
		foreach (Snake snake in allSnakes)
		{
			snake.Activate();
		}
	}

	public Snake GetPlayerSnake()
	{
		return playerSnakeController.snake;
	}

	public void SetSnakeController(CameraType cameraType)
	{
		playerSnakeController.SetControllerType(cameraType);
	}

}
