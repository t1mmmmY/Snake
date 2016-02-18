using UnityEngine;
using System.Collections;

public abstract class ObjectOnMap : MonoBehaviour 
{
	public virtual Vector2 cellPosition
	{
		get 
		{
			Vector2 pos = new Vector2(Mathf.RoundToInt(transform.localPosition.x), Mathf.RoundToInt(transform.localPosition.z));
			pos.x += LevelSettings.Instance.GetLevelSize().x / 2;
			pos.y += LevelSettings.Instance.GetLevelSize().y / 2;

			return pos;
		}
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
	}

}
