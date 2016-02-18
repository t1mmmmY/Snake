using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Factory : BaseSingleton<Factory>
{
	//Create all objects at one frame
	public List<T> CreateObjects<T>(T prefab, int count, bool stickToGrid = true) where T : MonoBehaviour
	{
		List<T> listOfObjects = new List<T>();

		for (int i = 0; i < count; i++)
		{
			listOfObjects.Add(CreateObject(prefab, GetRandomPosition(stickToGrid)));
		}

		return listOfObjects;
	}

	//Separate objects creation to some period of time
	public void CreateObjectsAsync<T>(T prefab, int count, System.Action<T> callbackOnCreateObject, System.Action callbackOnFinish, bool stickToGrid = true) where T : MonoBehaviour
	{
		StartCoroutine(CreateObjectsCoroutine<T>(prefab, count, callbackOnCreateObject, callbackOnFinish, stickToGrid));
	}


	IEnumerator CreateObjectsCoroutine<T>(T prefab, int count, System.Action<T> callbackOnCreateObject, System.Action callbackOnFinish, bool stickToGrid) where T : MonoBehaviour
	{
		for (int i = 0; i < count; i++)
		{
			T obj = CreateObject(prefab, GetRandomPosition(stickToGrid));

			if (callbackOnCreateObject != null)
			{
				callbackOnCreateObject(obj);
			}
			
			yield return new WaitForEndOfFrame();
		}

		if (callbackOnFinish != null)
		{
			callbackOnFinish();
		}
	}


	T CreateObject<T>(T prefab, Vector3 position) where T : MonoBehaviour
	{
		T obj = GameObject.Instantiate<T>(prefab);
		obj.transform.position = position;
		
		return obj;
	}
	
	Vector3 GetRandomPosition(bool stickToGrid)
	{
		Vector3 randomPosition = Vector3.zero;
		Vector2 randomCell = GameMap.GetRandomEmptyCell();

		randomPosition.x = randomCell.x;
		randomPosition.z = randomCell.y;


//		Vector2 levelSize = LevelManager.Instance.playableLevelSize;
//		Vector3 randomPosition = new Vector3(Random.Range(-levelSize.x / 2.0f, levelSize.x / 2.0f),
//			0,
//			Random.Range(-levelSize.y / 2.0f, levelSize.y / 2.0f));
//
//		if (stickToGrid)
//		{
//			randomPosition.x = Mathf.RoundToInt(randomPosition.x);
//			randomPosition.z = Mathf.RoundToInt(randomPosition.z);
//		}

		return randomPosition;
	}
	

}
