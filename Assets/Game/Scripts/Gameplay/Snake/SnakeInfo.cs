using UnityEngine;
using System.Collections;

public class SnakeInfo
{
	public Snake snake;
	public bool isReady;
	public int length;

	public SnakeInfo(Snake snake)
	{
		this.snake = snake;
		isReady = false;
		length = 1;
	}

	public void Ready()
	{
		isReady = true;
	}
}
