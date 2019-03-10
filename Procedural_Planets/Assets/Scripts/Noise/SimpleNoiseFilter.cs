using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
	NoiseSettings.SimpleNoiseSettings settings;
	Noise noise = new Noise();

	public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
	{
		this.settings = settings;
	}
	
	public float Evaluate(Vector3 point)
	{
		/* Squish value to 0 to 1.
		 * Point * settings.roughness => the further the points are we're sampling, the greater the difference between the values will be. 
		 * The terrain will be more rough.
		 */
		float noiseValue = 0;
		float frequency = settings.baseRoughness;
		float amplitude = 1;

		for (int i = 0; i < settings.numLayers; i++)
		{
			float v = noise.Evaluate(point * frequency + settings.centre);
			noiseValue += (v + 1) * .5f * amplitude;	// To get in the range 0 to 1.
			// When the roughness is greater than 1, the greater the frequency will increase with each layer. 
			frequency *= settings.roughness;
			// When the persistence is less than 1, the amplitude wil decrease with each layer.
			amplitude *= settings.persistence;
		}

		// Makes the terrain recede into the base sphere.
		noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
		return noiseValue * settings.strength;
	}
}
