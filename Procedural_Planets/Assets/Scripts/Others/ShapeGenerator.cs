using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator 
{
	private ShapeSettings settings;
	private INoiseFilter[] noiseFilters;
	public MinMax elevationMinMax;

	public void UpdateSettings(ShapeSettings settings)
	{
		this.settings = settings;
		noiseFilters = new INoiseFilter[settings.noiseLayers.Length];

		// Initialize all the NoiseFilters.
		for (int i = 0; i < noiseFilters.Length; i++)
		{
			noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
		}
		elevationMinMax = new MinMax();
	}

	public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
	{
		// Store the value of the first layer, so it can be used as a mask for the subsequential layers.
		float firstLayerValue = 0;
		float elevation = 0;

		if (noiseFilters.Length > 0)
		{
			firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
			if (settings.noiseLayers[0].enabled)
			{
				elevation = firstLayerValue;
			}
		}

		// Waste to do the Evaluate again.
		for (int i = 1; i < noiseFilters.Length; i++)
		{
			if (settings.noiseLayers[i].enabled)
			{
				/* Value of mask depends on whether or not the current noise layer is using the first layer as a mask.
				 * If it is, then the mask will be equal to the first layer value and otherwise it will be equal one,
				 *which means there is no mask, since we will be multiplying this by the mask value.
				 */
				float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1;
				elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
			}
		}
		elevation = settings.planetRadius * (1 + elevation);
		elevationMinMax.AddValue(elevation);
		return pointOnUnitSphere * elevation;
	}
}
