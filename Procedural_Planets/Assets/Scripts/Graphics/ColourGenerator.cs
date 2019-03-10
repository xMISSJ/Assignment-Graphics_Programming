using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGenerator
{
	ColourSettings settings;
	Texture2D texture;
	const int textureResolution = 50;

	public void UpdateSettings(ColourSettings settings)
	{
		this.settings = settings;
		// Doesn't initialize a new texture for each change in the settings.
		if (texture == null)
		{
			texture = new Texture2D(textureResolution, 1);
		}
	}

	public void UpdateElevation(MinMax elevationMinMax)
	{
		// Send information to the shader.
		settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
	}

	public void UpdateColours()
	{
		Color[] colours = new Color[textureResolution];
		for (int i = 0; i < textureResolution; i++)
		{
			// Round result down to float with -1f.
			colours[i] = settings.gradient.Evaluate(i / (textureResolution - 1f));
		}
		texture.SetPixels(colours);
		texture.Apply();
		settings.planetMaterial.SetTexture("_texture", texture);
	}
}
