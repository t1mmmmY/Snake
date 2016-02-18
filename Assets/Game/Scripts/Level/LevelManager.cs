using UnityEngine;
using System.Collections;

public abstract class LevelManager : BaseSingleton<LevelManager>, ILevelManager
{
	[SerializeField] Transform _cameraPoint;
	public Transform cameraPoint
	{
		get { return _cameraPoint; }
	}

	public Vector2 levelSize { get; protected set; }
	public Vector2 playableLevelSize
	{
		get 
		{
			//levl size minus border
			return levelSize - Vector2.one;
		}
	}

	public virtual void SetLevelSize(Vector2 size)
	{
		levelSize = size;
	}

	public abstract void CreateObstacle(Obstacle obstacle);

	public abstract void CreateObstacles(Obstacle obstacle, int count);

	public abstract void ClearMap();
}
