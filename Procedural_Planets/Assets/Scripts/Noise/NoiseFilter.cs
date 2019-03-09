using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter
{
	Noise noise = new Noise();
	
	public float Evaluate(Vector3 point)
	{
		// Squish value to 0 to 1.
		float noiseValue = (noise.Evaluate(point) + 1) * .5f;
		return noiseValue;
	}
}
