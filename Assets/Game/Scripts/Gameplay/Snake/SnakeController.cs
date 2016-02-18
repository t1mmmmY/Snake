using UnityEngine;
using System.Collections;

public class SnakeController : MonoBehaviour 
{
	public Snake snake { get; private set; }

	CameraType cameraType;

	public Transform cameraPoint
	{
		get { return snake.cameraPoint; }
	}

	public void Init(Snake snake, CameraType cameraType = CameraType.TopView)
	{
		this.snake = snake;
		this.cameraType = cameraType;
	}

	public void SetControllerType(CameraType cameraType)
	{
		this.cameraType = cameraType;
	}

	void Update()
	{
		

//		if (horizontal != 0 || vertical != 0)
		{
			switch (cameraType)
			{
				case CameraType.TopView:
					Vector2 direction = GetTopViewDirection();
					if (direction.magnitude != 0)
					{
						snake.SetDirection(direction);
					}
					break;
				case CameraType.ThirdPersonView:
					float horizontal = GetHorizontalInput();
					if (horizontal != 0)
					{
						snake.SetDirection(ConvertThirdPersonToDirection(horizontal));
					}
					break;
			}
		}
	}

	private float GetHorizontalInput()
	{
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			//Turn left
			return -1;
		}
		else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			return 1;
		}
		else 
		{
			return 0;
		}
	}

	private float GetVerticalInput()
	{
		if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
		{
			//Turn left
			return -1;
		}
		else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			return 1;
		}
		else 
		{
			return 0;
		}
	}

	private Vector2 GetTopViewDirection()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = 0;

		if (horizontal != 0)
		{
			if (horizontal < -0.01f)
			{
				horizontal = -1;
			}
			else if (horizontal > 0.01f)
			{
				horizontal = 1;
			}
			else 
			{
				horizontal = 0;
			}
		}
		else
		{
			vertical = Input.GetAxis("Vertical");

			if (vertical < -0.01f)
			{
				vertical = -1;
			}
			else if (vertical > 0.01f)
			{
				vertical = 1;
			}
			else
			{
				vertical = 0;
			}
		}

		return new Vector2(horizontal, vertical);
	}

	private Vector2 ConvertThirdPersonToDirection(float horizontal)
	{
		SnakeDirection snakeDirection = snake.GetDirection();

		if (horizontal < 0)
		{
			//Turn left
			switch (snakeDirection)
			{
				case SnakeDirection.Up:
					return new Vector2(-1, 0);
				case SnakeDirection.Right:
					return new Vector2(0, 1);
				case SnakeDirection.Down:
					return new Vector2(1, 0);
				case SnakeDirection.Left:
					return new Vector2(0, -1);
			}
		}
		else
		{
			//Turn right
			switch (snakeDirection)
			{
				case SnakeDirection.Up:
					return new Vector2(1, 0);
				case SnakeDirection.Right:
					return new Vector2(0, -1);
				case SnakeDirection.Down:
					return new Vector2(-1, 0);
				case SnakeDirection.Left:
					return new Vector2(0, 1);
			}
		}

		return Vector2.zero;

	}

}
