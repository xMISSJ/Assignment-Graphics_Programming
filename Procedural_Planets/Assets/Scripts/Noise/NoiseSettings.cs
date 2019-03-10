using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Values in this class show up in the inspector.
[System.Serializable]
public class NoiseSettings
{
	// List different types of noises.
	public enum FilterType { Simple, Rigid };
	public FilterType filterType;

	[ConditionalHide("filterType", 0)]
	public SimpleNoiseSettings simpleNoiseSettings;

	[ConditionalHide("filterType", 1)]
	public RigidNoiseSettings rigidNoiseSettings;

	[System.Serializable]
	public class SimpleNoiseSettings
	{
		// Noise characteristics.
		public float strength = 1;
		[Range(1, 8)]
		public int numLayers = 1;
		public float baseRoughness = 1;
		public float roughness = 2;
		public float persistence = .5f;     // Amplitude will be halved with each layer.
		public Vector3 centre;              // Move around the noise.
		public float minValue;
	}

	[System.Serializable]
	public class RigidNoiseSettings : SimpleNoiseSettings
	{
		public float weightMultiplier = .8f;
	}
}
