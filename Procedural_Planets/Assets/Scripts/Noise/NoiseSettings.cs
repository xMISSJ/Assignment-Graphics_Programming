using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Values in this class show up in the inspector.
[System.Serializable]
public class NoiseSettings
{

	// Noise characteristics.
	public float strength = 1;
	[Range(1, 8)]
	public int numLayers = 1;
	public float baseRoughness = 1;
	public float roughness = 2;
	public float persistence = .5f;		// Amplitude will be halved with each layer.
	public Vector3 centre;              // Move around the noise.
	public float minValue;


}
