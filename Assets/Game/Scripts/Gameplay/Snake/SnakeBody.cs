using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SnakeBody : ObjectOnMap 
{
	[SerializeField] bool isHead = false;
	SnakeBody previousPart;
	Snake snake;

	Vector3 currentPoint = Vector3.zero;
	Vector3 nextPoint = Vector3.zero;
	System.Action<Vector3> onChangePosition;
	System.Action onStop;

	public System.Action onCompleteRotating;

	bool ignoreCollisions = true;

	Tweener movingTweener;
	Tweener rotatingTweener;

	public Vector3 position
	{
		get
		{
			return transform.localPosition;
		}
	}

	public float angle
	{
		get
		{
			return transform.localRotation.x;
		}
	}

	public void Activate(Snake snake)
	{
		this.snake = snake;
		this.gameObject.SetActive(true);
	}

	public void Activate(SnakeBody parent, Snake snake)
	{
		Activate(snake);

		previousPart = parent;
		previousPart.onChangePosition += OnChangePosition;
		previousPart.onStop += Stop;
		ignoreCollisions = true;

		SetCurrentCell(parent.position);
	}

	public void Deactivate()
	{
		this.gameObject.SetActive(false);
	}

	void OnDestroy()
	{
//		previousPart.onChangePosition -= OnChangePosition;
	}

	public void SetCurrentCell(Vector3 cellPosition)
	{
		currentPoint = cellPosition;
		nextPoint = cellPosition;

		transform.localPosition = cellPosition;


		if (!isHead)
		{
			transform.localRotation = previousPart.transform.localRotation;
		}
	}

	public void SetNextPoint(Vector3 point)
	{
		currentPoint = nextPoint;
		nextPoint = point;


		UpdatePosition();

		if (onChangePosition != null)
		{
			if (isHead)
			{
				//Don't need to shift between head and body, because head is only on one cell
				onChangePosition(nextPoint);
			}
			else
			{
				onChangePosition(currentPoint);
			}
		}
	}

	public void Stop()
	{
		movingTweener.Pause();
		rotatingTweener.Pause();

		if (onStop != null)
		{
			onStop();
		}
	}

	void OnChangePosition(Vector3 point)
	{
		SetNextPoint(point);
	}

	void UpdatePosition()
	{
		if (snake != null)
		{
			movingTweener = transform.DOLocalMove(nextPoint, 1.0f / snake.speed).OnComplete(OnCompleteMoving);

			//If look rotating vector is not a zero
			if (Vector3.Distance(position, nextPoint) != 0)
			{
				rotatingTweener = transform.DOLookAt(nextPoint, 1.0f / snake.speed).OnComplete(OnCompleteRotating);
			}
		}
		else
		{
			Debug.LogError(this.gameObject.name + " snake == null");
		}
	}

	void OnCompleteMoving()
	{
		ignoreCollisions = false;
	}

	void OnCompleteRotating()
	{
		if (onCompleteRotating != null)
		{
			onCompleteRotating();
		}
	}


	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);

		if (ignoreCollisions)
		{
			return;
		}

		if (isHead)
		{
			ObjectOnMap someObject = other.GetComponent<ObjectOnMap>();
			if (someObject == null)
			{
				Debug.LogWarning("someObject == null");
				return;
			}

			if (someObject is Food)
			{
				Debug.Log("Collect food");
				snake.EatFood((Food)someObject);
			}
			else if (someObject is Obstacle)
			{
				Debug.Log("Hit obstancle!");
				snake.HitObstancle();
			}
			else if (someObject is SnakeBody)
			{
				Debug.Log("Hit myself!");
				snake.HitObstancle();
			}
			else
			{
				Debug.LogError("Hit something strange " + other.gameObject.name);
			}
//			Debug.Log("Trigger " + other.gameObject.name);		
		}
	}

}
