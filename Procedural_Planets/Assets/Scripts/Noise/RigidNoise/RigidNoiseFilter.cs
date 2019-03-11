using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter: INoiseFilter
{
	NoiseSettings.RigidNoiseSettings settings;
	Noise noise = new Noise();

	public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
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
		float weight = 1;

		for (int i = 0; i < settings.numLayers; i++)
		{
			// Get absolute value of the noise to get the rigids and peaks.
			float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.centre));
			// Make the rigids more pronounced.
			v *= v;
			// Make the noise in the rigids more detailed than the valleys below.
			// The way to do this is to weighting the noise in each layer based on the layers that came before it.
			v *= weight;
			// For the next layer. Weight doesn't grow greater than one.
			weight = Mathf.Clamp01(v * settings.weightMultiplier);
			noiseValue += v  * amplitude;		// Because of the absolute value, it's in the range of 0 and 1 already.
			// When the roughness is greater than 1, the greater the frequency will increase with each layer. 
			frequency *= settings.roughness;
			// When the persistence is less than 1, the amplitude wil decrease with each layer.
			amplitude *= settings.persistence;
		}

		// Makes the terrain recede into the base sphere.
		noiseValue = noiseValue - settings.minValue;
		return noiseValue * settings.strength;
	}
}
