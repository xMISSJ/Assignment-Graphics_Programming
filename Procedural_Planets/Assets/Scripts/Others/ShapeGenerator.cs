using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator 
{
	private ShapeSettings settings;

	public ShapeGenerator(ShapeSettings settings)
	{
		this.settings = settings;
	}

	public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
	{
		return pointOnUnitSphere * settings.planetRadius;
	}
}
