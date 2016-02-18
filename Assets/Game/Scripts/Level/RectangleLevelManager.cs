using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RectangleLevelManager : LevelManager 
{
	[SerializeField] Transform floorTransform;
	[SerializeField] MeshRenderer floorMeshRenderer;
	[SerializeField] Transform obstaclesParent;

	List<Obstacle> allObstacles;

	protected override void Awake()
	{
		base.Awake();

		allObstacles = new List<Obstacle>();
	}


	public override void SetLevelSize(Vector2 size)
	{
		base.SetLevelSize(size);

		SetFloor(size);
	}

	public override void ClearMap()
	{
		foreach(Obstacle obstacle in allObstacles)
		{
			Destroy(obstacle.gameObject);
		}
		allObstacles = new List<Obstacle>();
	}

	public override void CreateObstacle(Obstacle obstacle)
	{
		CreateObstacles(obstacle, 1);
	}

	public override void CreateObstacles(Obstacle obstacle, int count)
	{
		Factory.Instance.CreateObjectsAsync<Obstacle>(obstacle, count, OnCreateObstacle, OnFinishCreateObstacles, true);
	}

	void OnCreateObstacle(Obstacle obstacle)
	{
		allObstacles.Add(obstacle);
		obstacle.transform.parent = obstaclesParent;
		GameMap.SetCell(obstacle.cellPosition, true);
	}

	void OnFinishCreateObstacles()
	{
		//Ready to start game
		GameController.LevelReady();
	}

	private void SetFloor(Vector2 size)
	{
		floorTransform.localScale = new Vector3(size.x, size.y, 1);

		floorMeshRenderer.material.SetTextureScale("_MainTex", size / 2);
		floorMeshRenderer.material.SetTextureOffset("_MainTex", new Vector2(0.25f, 0.25f));
	}

}
