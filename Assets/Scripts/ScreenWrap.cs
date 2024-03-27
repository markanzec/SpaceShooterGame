using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
	[HideInInspector]
	public Vector2 wrappedPosition;

	public bool CheckForWrapping(Vector2 position, float top, float right)
	{
		if (position.y > top) //check for top
		{
			wrappedPosition = new Vector2(position.x, -top);
			return true;
		}
		else if (position.y < -top) //bottom
		{
			wrappedPosition = new Vector2(position.x, top);
			return true;
		}
		else if (position.x > right) //right
		{ 
			wrappedPosition = new Vector2(-right, position.y);
			return true;
		}
		else if (position.x < -right) //left
		{
			wrappedPosition = new Vector2(right, position.y);
			return true;
		}
		else
			return false;
	}
}
