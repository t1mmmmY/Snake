using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public enum CameraType
{
	TopView,
	ThirdPersonView
}

public class CameraController : MonoBehaviour 
{
	[SerializeField] Transform cameraTransform;
	[SerializeField] SnakeCreator snakeCreator;


	Transform targetPoint;

	string[] cameraTypes = { "Top view", "3rd person view" };

	int currentView = 0;
	bool inGame = false;

	CameraType cameraType;

	public string ChangeView()
	{
		currentView++;
		if (currentView >= cameraTypes.Length)
		{
			currentView = 0;
		}
		return cameraTypes[currentView];
	}

	public string GetCurrentViewName()
	{
		return cameraTypes[currentView];
	}

	void Start()
	{
		cameraType = CameraType.TopView;

		GameController.onStartGame += OnStartGame;
		GameController.onFinishGame += OnFinishGame;
	}

	void OnDestroy()
	{
		GameController.onStartGame -= OnStartGame;
		GameController.onFinishGame -= OnFinishGame;
	}

	void OnStartGame()
	{
		switch (currentView)
		{
			case 0:
				SetTopView();
				break;
			case 1:
				SetThirdPesonView();
				break;
			default:
				break;
		}

		snakeCreator.SetSnakeController(cameraType);

		inGame = true;
	}

	void OnFinishGame()
	{
		SetTopView();
		ChangeCameraPosition();

		inGame = false;
	}

	void SetTopView()
	{
		LevelManager levelManager = FindObjectOfType<LevelManager>();
		if (levelManager == null)
		{
			Debug.LogError("levelManager == null");
			return;
		}
		targetPoint = levelManager.cameraPoint;
		cameraType = CameraType.TopView;

		ChangeCameraPosition();
	}

	void SetThirdPesonView()
	{
		Snake snake = snakeCreator.GetPlayerSnake();

		if (snake == null)
		{
			Debug.LogError("snake == null");
			return;
		}
		targetPoint = snake.cameraPoint;
		cameraType = CameraType.ThirdPersonView;
	}

	void LateUpdate()
	{
		//Change camera position on update only if it is a 3-rd person view
		if (cameraType == CameraType.ThirdPersonView)
		{
			ChangeCameraPosition();
		}
	}

	void ChangeCameraPosition()
	{
		cameraTransform.DOMove(targetPoint.position, 1.0f);
		cameraTransform.DORotate(targetPoint.rotation.eulerAngles, 1.0f);
	}



}
