using UnityEngine;

// This is like a sex slave class
// Just use it when needed, use and forget, like women
public static class TP_Helper
{
	public static float ClampAngle(float angle, float min, float max)
	{
		do
		{
			if(angle < -360f)
				angle += 360f;
			if(angle > 360f)
				angle -= 360f;

		} while(angle < -360f || angle > 360f);

		return Mathf.Clamp(angle, min, max);
	}
}
