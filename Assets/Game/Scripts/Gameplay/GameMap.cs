using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class GameMap 
{
	private static int[,] allCells;

	static GameMap()
	{
		Vector2 size = LevelSettings.Instance.GetLevelSize();
		allCells = new int[(int)size.x, (int)size.y];
	}


//	public static void Init(Vector2 size)
//	{
//	}

	//May be changed to time of cell, not just filled or empty.
	public static bool SetCell(Vector2 cell, bool filled)
	{
		if ((int)cell.x < allCells.GetLength(0) && (int)cell.y < allCells.GetLength(1))
		{
			allCells[(int)cell.x, (int)cell.y] = filled ? 1 : 0;
			return true;
		}
		else
		{
			Debug.LogError("Cell " + cell.ToString() + " does not exist!");
			return false;
		}
	}

	public static bool GetCell(Vector2 cell)
	{
		if ((int)cell.x < allCells.GetLength(0) && (int)cell.y < allCells.GetLength(1))
		{
			return allCells[(int)cell.x, (int)cell.y] == 1 ? true : false;
		}
		else
		{
			Debug.LogError("Cell " + cell.ToString() + " does not exist!");
			return false;
		}
	}

	public static Vector2 GetRandomEmptyCell()
	{
		Vector2[] allEmptyCells = GetAllEmptyCells();

		if (allEmptyCells.Length == 0)
		{
			//no empty cells left
			Debug.LogWarning("No empty cells left!");
			return Vector2.zero;
		}
		else
		{
			return allEmptyCells[Random.Range(0, allEmptyCells.Length)];
		}
	}

	public static Vector2[] GetAllEmptyCells()
	{
		List<Vector2> allEmptyCells = new List<Vector2>();

		//Start from 1 and end on length - 1 because of borders
		for (int i = 1; i < allCells.GetLength(0) - 1; i++)
		{
			for (int j = 1; j < allCells.GetLength(1) -1 ; j++)
			{
				if (allCells[i, j] == 0)
				{
					allEmptyCells.Add(new Vector2(i - allCells.GetLength(0) / 2, j - allCells.GetLength(1) / 2));
				}
			}
		}

		return allEmptyCells.ToArray();
	}

}
