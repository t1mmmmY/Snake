using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	[SerializeField] TweenPosition tweenShowWindow;
	[SerializeField] TweenPosition tweenShowGameUI;
	[SerializeField] GeneralGameController generalGameController;
	[SerializeField] CameraController cameraController;

	[SerializeField] UILabel scoreLabel;
	[SerializeField] UILabel cameraLabel;


	public void ChangeView()
	{
		cameraLabel.text = cameraController.ChangeView();
	}

	public void PlayGame()
	{
		generalGameController.PlayGame();
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void ExitToMenu()
	{
		GameController.GameOver();
	}

	void Start()
	{
		tweenShowWindow.PlayForward();

		cameraLabel.text = cameraController.GetCurrentViewName();
		GameController.onPrepareGame += OnPrepareGame;
		GameController.onStartGame += OnStartGame;
		GameController.onFinishGame += OnFinishGame;
		FoodController.onEatFood += OnEatFood;
	}

	void OnDestroy()
	{
		GameController.onPrepareGame -= OnPrepareGame;
		GameController.onStartGame -= OnStartGame;
		GameController.onFinishGame -= OnFinishGame;
		FoodController.onEatFood -= OnEatFood;
	}

	void OnPrepareGame()
	{
		tweenShowWindow.PlayReverse();
		tweenShowGameUI.PlayForward();

		scoreLabel.text = FoodController.Instance.score.ToString();
	}

	void OnStartGame()
	{
	}

	void OnFinishGame()
	{
		tweenShowWindow.PlayForward();
		tweenShowGameUI.PlayReverse();
	}

	void OnEatFood(int score)
	{
		scoreLabel.text = score.ToString();
	}

}
