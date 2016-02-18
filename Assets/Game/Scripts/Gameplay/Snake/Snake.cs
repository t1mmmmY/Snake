using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public enum SnakeDirection
{
	Up,
	Right,
	Down,
	Left
}

public class Snake : MonoBehaviour 
{
	[Range(10, 500)]
	[SerializeField] int maxLength = 200;

	[SerializeField] SnakeBody snakeHead;
	[SerializeField] SnakeBody bodyPartPrefab;

	[SerializeField] Transform _cameraPoint;

	public Transform cameraPoint 
	{ 
		get { return _cameraPoint; }
	}

	public int length { get; private set; }

	List<SnakeBody> allBodyParts;


	public float speed { get; private set; }

	Vector3 direction = Vector3.forward;


	Vector3 currentCell;
	SnakeDirection snakeDirection;


	void Awake()
	{
		allBodyParts = new List<SnakeBody>();
		snakeDirection = SnakeDirection.Up;

		//Add head
		AddBodyPart(snakeHead);

		GameController.AddSnake(this);

		//Create pull body parts
		Factory.Instance.CreateObjectsAsync<SnakeBody>(bodyPartPrefab, maxLength, OnCreateBodyPart, OnCreateFullSnake);
	}

	void Start()
	{
		
	}


	public void Deactivate()
	{
		this.gameObject.SetActive(false);
	}

	public void Activate()
	{
		length = 1;
		speed = 4;
		direction = new Vector3(0, 0, 1);
		currentCell = transform.localPosition;

		//Snake position must be (0, 0, 0), only snake parts changed their positions
		this.transform.localPosition = Vector3.zero;

		this.gameObject.SetActive(true);

		snakeHead.Activate(this);
		snakeHead.SetCurrentCell(currentCell);
		snakeHead.onCompleteRotating += OnCompleteRotating;

		//Start game
		//i == 0 is a head. It is already created
		for (int i = 1; i < GameController.startSnakeSize; i++)
		{
			ActivateBodyPart(allBodyParts[i], allBodyParts[i - 1]);
		}

		Move();
	}

	public SnakeDirection GetDirection()
	{
		return snakeDirection;
	}


	public void SetDirection(Vector2 direction)
	{
		Vector3 newDirection = new Vector3(direction.x, 0, direction.y);

		if (CanChangeDirection(ConvertoToSnakeDirection(newDirection)))
		{
			if (newDirection != this.direction)
			{
				this.direction = newDirection;
			}
		}
	}

	public void EatFood(Food food)
	{
		FoodController.Instance.EatFood(food);
		ActivateBodyPart(length);
		speed += 0.1f;
//		length++;
	}

	public void HitObstancle()
	{
		Stop();
		GameController.GameOver();
	}

	void Stop()
	{
		snakeHead.Stop();
	}

	void LateUpdate()
	{
		Vector3 nearestCellPosition = GetNearestCell();
		
		if (CanMakeMove(nearestCellPosition) && Vector3.Distance(snakeHead.position, nearestCellPosition) < 0.05f * speed)
		{
			currentCell = nearestCellPosition;
			Move();
		}
	}

	void Move()
	{
		snakeHead.SetNextPoint(currentCell + direction);
		snakeDirection = ConvertoToSnakeDirection(direction);
	}

	void OnCompleteRotating()
	{
	}

	Vector3 GetNearestCell()
	{
		int posX = Mathf.RoundToInt(snakeHead.position.x);
		int posY = Mathf.RoundToInt(snakeHead.position.z);

		return new Vector3(posX, snakeHead.position.y, posY);
	}

	bool CanMakeMove(Vector3 nearestCellPosition)
	{
		return currentCell != nearestCellPosition;
	}

	bool CanChangeDirection(SnakeDirection dir)
	{
		//Can't rotate to opposite direction
		switch (snakeDirection)
		{
			case SnakeDirection.Up:
				return dir == SnakeDirection.Down ? false : true;
			case SnakeDirection.Right:
				return dir == SnakeDirection.Left ? false : true;
			case SnakeDirection.Down:
				return dir == SnakeDirection.Up ? false : true;
			case SnakeDirection.Left:
				return dir == SnakeDirection.Right ? false : true;
			default:
				return false;
		}
	}


	public SnakeDirection ConvertoToSnakeDirection(Vector3 dir)
	{
		if (dir.x == 1)
		{
			//Turn right
			return SnakeDirection.Right;
		}
		else if (dir.x == - 1)
		{
			//Turn left
			return SnakeDirection.Left;
		}
		else if (dir.z == 1)
		{
			//Turn up
			return SnakeDirection.Up;
		}
		else if (dir.z == -1)
		{
			//Turn down
			return SnakeDirection.Down;
		}
		else
		{
			Debug.LogError("Wrong direction " + dir.ToString());
			return SnakeDirection.Up;
		}
	}


	void OnCreateBodyPart(SnakeBody bodyPart)
	{
		AddBodyPart(bodyPart);

		if (this != null)
		{
			bodyPart.transform.parent = this.transform;
			bodyPart.Deactivate();
		}
		else
		{
			//this object was destroyed
			Destroy(bodyPart.gameObject);
		}
	}

	void OnCreateFullSnake()
	{
		//Ready to start
		GameController.SnakeReady(this);
	}


	void AddBodyPart(SnakeBody bodyPart)
	{
		allBodyParts.Add(bodyPart);
		bodyPart.gameObject.name = string.Format("{0}_{1}", bodyPart.gameObject.name.Replace("(Clone)", ""), allBodyParts.Count.ToString());
	}


	void ActivateBodyPart(int number)
	{
		ActivateBodyPart(allBodyParts[number], allBodyParts[number - 1]);
	}

	void ActivateBodyPart(SnakeBody bodyPart, SnakeBody parentBody)
	{
		bodyPart.Activate(parentBody, this);
		length++;
	}

}
