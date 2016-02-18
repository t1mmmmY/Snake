using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodController : BaseSingleton<FoodController> 
{
	[SerializeField] Food foodPrefab;

	List<Food> allFood;

	public int score { get; private set; }
	public static System.Action<int> onEatFood;

	protected override void Awake()
	{
		base.Awake();

		allFood = new List<Food>();
	}

	void Start()
	{
		GameController.onClearMap += ClearMap;
		GameController.onPrepareGame += OnPrepareGame;
		GameController.onStartGame += OnStartGame;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		GameController.onClearMap -= ClearMap;
		GameController.onPrepareGame -= OnPrepareGame;
		GameController.onStartGame -= OnStartGame;
	}


	private void ClearMap()
	{
		foreach(Food food in allFood)
		{
			Destroy(food.gameObject);
		}
		allFood = new List<Food>();

		score = 0;
	}

	public bool EatFood(Food food)
	{
		if (allFood.Contains(food))
		{
			GameMap.SetCell(food.cellPosition, false);
			allFood.Remove(food);
			Destroy(food.gameObject);
			CreateFood();
			score++;

			if (onEatFood != null)
			{
				onEatFood(score);
			}
			return true;
		}
		else
		{
			Debug.LogError("allFood does not contains " + food.gameObject.name);
			return false;
		}
	}

	void OnPrepareGame()
	{
		//Two apple = twice faster:)
		CreateFood();
		CreateFood();

		score = 0;
	}

	void OnStartGame()
	{
	}

	void CreateFood()
	{
		List<Food> newFood = Factory.Instance.CreateObjects<Food>(foodPrefab, 1, true);

		foreach (Food food in newFood)
		{
			GameMap.SetCell(food.cellPosition, true);
			allFood.Add(food);
		}
//		Factory.Instance.CreateObjectsAsync<Food>(foodPrefab, 1, OnCreateFood, OnFinishCreatingFood, true);
	}

	void OnCreateFood(Food food)
	{
		GameMap.SetCell(food.cellPosition, true);
		allFood.Add(food);
	}

	void OnFinishCreatingFood()
	{
		
	}

}
