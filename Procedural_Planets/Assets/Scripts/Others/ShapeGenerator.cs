using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator 
{
	private ShapeSettings settings;
	private NoiseFilter[] noiseFilters;

	public ShapeGenerator(ShapeSettings settings)
	{
		this.settings = settings;
		noiseFilters = new NoiseFilter[settings.noiseLayers.Length];

		// Initialize all the NoiseFilters.
		for (int i = 0; i < noiseFilters.Length; i++)
		{
			noiseFilters[i] = new NoiseFilter (settings.noiseLayers[i].noiseSettings);
		}
	}

	public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
	{
		float elevation = 0;
		for (int i = 0; i < noiseFilters.Length; i++)
		{
			if (settings.noiseLayers[i].enabled)
			{
				elevation += noiseFilters[i].Evaluate(pointOnUnitSphere);
			}
		}
		return pointOnUnitSphere * settings.planetRadius * (1 + elevation);
	}
}
