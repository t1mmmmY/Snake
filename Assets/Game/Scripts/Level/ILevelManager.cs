using UnityEngine;

public interface ILevelManager
{
	void SetLevelSize(Vector2 size);

	void CreateObstacle(Obstacle obstacle);

	void CreateObstacles(Obstacle obstacle, int count);

	void ClearMap();
}
