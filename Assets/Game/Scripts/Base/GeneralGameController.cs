using UnityEngine;
using System.Collections;

public class GeneralGameController : MonoBehaviour 
{
	[SerializeField] CameraController cameraController;
	bool gameLoading = false;

	void Start()
	{
		GameController.onFinishGame += FinishGame;
		GameController.onStartGame += OnStarGame;
	}

	public void OnDestroy()
	{
		GameController.onFinishGame -= FinishGame;
		GameController.onStartGame -= OnStarGame;
	}

//	public void ChangeCameraView()
//	{
//		cameraController.ChangeView();
//	}

	public void PlayGame()
	{
		if (gameLoading)
		{
			return;
		}

		gameLoading = true;
		GameController.PrepareGame();
	}

	void FinishGame()
	{
	}

	void OnStarGame()
	{
		gameLoading = false;
	}

}
