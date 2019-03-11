using UnityEngine;

public class ColourGenerator
{
	ColourSettings settings;
	Texture2D texture;
	const int textureResolution = 50;
	INoiseFilter biomeNoiseFilter;

	public void UpdateSettings(ColourSettings settings)
	{
		this.settings = settings;
		// Doesn't initialize a new texture for each change in the settings.
		if (texture == null || texture.height != settings.biomeColourSettings.biomes.Length)
		{
			// Height that corresponds to a number of biomes, so that each row of the texture can store the colours of that biome.
			texture = new Texture2D(textureResolution, settings.biomeColourSettings.biomes.Length);
			//texture = new Texture2D(textureResolution, settings.biomeColourSettings.biomes.Length, TextureFormat.RGBA32, false);
		}
		biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColourSettings.noise);
	}

	public void UpdateElevation(MinMax elevationMinMax)
	{
		// Send information to the shader.
		settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
	}

	// Return value of 0, if we're in the first biome.
	// Return value of 1, if we're int he last biome.
	public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
	{
		// unitSphere goes from -1 to 1.
		float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
		heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeColourSettings.noiseOffset) * settings.biomeColourSettings.noiseStrength;
		float biomeIndex = 0;
		int numBiomes = settings.biomeColourSettings.biomes.Length;
		float blendRange = settings.biomeColourSettings.blendAmount / 2f + .001f;

		for (int i = 0; i < numBiomes; i++)
		{
			// Distance of biomes startHeight of the current heightPercent.
			float dst = heightPercent - settings.biomeColourSettings.biomes[i].startHeight;
			float weight = Mathf.InverseLerp(-blendRange, blendRange, dst);
			// So biome index doesn't get too large.
			biomeIndex *= (1 - weight);
			biomeIndex += i * weight;
		}
		// If biomeIndex is 0, then we'll use one.
		return biomeIndex / Mathf.Max(1, numBiomes - 1);
	}

	public void UpdateColours()
	{
		// Take to account the new height of the texture.
		Color[] colours = new Color[texture.width * texture.height];
		int colourIndex = 0;
		foreach (var biome in settings.biomeColourSettings.biomes)
		{
			for (int i = 0; i < textureResolution; i++)
			{
				// Round result down to float with -1f.
				Color gradientCol = biome.gradient.Evaluate(i / (textureResolution - 1f));
				Color tintCol = biome.tint;
				colours[colourIndex] = gradientCol * (1 - biome.tintPercent) + tintCol * biome.tintPercent;
				colourIndex++;
			}
		}
		texture.SetPixels(colours);
		texture.Apply();
		settings.planetMaterial.SetTexture("_texture", texture);
	}
}
