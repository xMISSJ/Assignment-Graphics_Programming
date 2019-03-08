/// <summary>
/// The main Planet class
/// Contains all methods for creating six terrain faces. For each face, the direction they're facing at will be defined.
/// </summary>
using UnityEngine;

public class Planet : MonoBehaviour
{
	// Max is 256, because 256 squared is about the maxium amount vertices a mesh can have.
	[Range(2, 256)]
	public int resolution = 10;

	// Save these, but hide them.
	[SerializeField, HideInInspector]
	MeshFilter[] meshFilters;
	TerrainFace[] terrainFaces;

	// Work in the editor, whenever we update anything.
	private void OnValidate()
	{
		Initialize();
		GenerateMesh();
	}

	private void Initialize()
	{
		if (meshFilters == null || meshFilters.Length == 0)
		{
			// For displaying the terrainFaces.
			meshFilters = new MeshFilter[6];
		}
		terrainFaces = new TerrainFace[6];

		Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
		for (int i = 0; i < 6; i++)
		{
			if (meshFilters[i] == null)
			{
				GameObject meshObject = new GameObject("mesh");

				// Parent it to the current transform, to keep clean hierarchy.
				meshObject.transform.parent = transform;

				// Default material.
				meshObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
				meshFilters[i] = meshObject.AddComponent<MeshFilter>();
				meshFilters[i].sharedMesh = new Mesh();
			}

			terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, resolution, directions[i]);
		}
	}

	private void GenerateMesh()
	{
		foreach (TerrainFace face in terrainFaces)
		{
			face.ConstructMesh();
		}
	}

}