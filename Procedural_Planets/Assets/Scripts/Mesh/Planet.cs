/// <summary>
/// The main Planet class
/// Contains all methods for creating six terrain faces. For each face, the direction they're facing at will be defined.
/// </summary>
using UnityEngine;

public class Planet : MonoBehaviour
{
	// Max is 256, because 256 squared is about the maxium amount vertices a mesh can have.
	[Range(2, 256)]
	public int resolution = 256;
	[Range(0, 6)]
	public int levelOfDetail;
	public bool autoUpdate = true;
	public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
	public FaceRenderMask faceRenderMask;

	public ShapeSettings shapeSettings;
	public ColourSettings colourSettings;

	[HideInInspector]
	public bool shapeSettingsFoldout;
	public bool colourSettingsFoldout;

	private ShapeGenerator shapeGenerator = new ShapeGenerator();
	private ColourGenerator colourGenerator = new ColourGenerator();

	// Save these, but hide them.
	[SerializeField, HideInInspector]
	MeshFilter[] meshFilters;
	TerrainFace[] terrainFaces;

	private void Initialize()
	{
		shapeGenerator.UpdateSettings(shapeSettings);
		colourGenerator.UpdateSettings(colourSettings);

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
				meshObject.AddComponent<MeshRenderer>();
				meshFilters[i] = meshObject.AddComponent<MeshFilter>();
				meshFilters[i].sharedMesh = new Mesh();
			}
			// Materials are assigned to each of the meshes.
			meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

			terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
			// Is true if the faceRenderMask is equal to all.
			// If (int)faceRenderMask - 1 == i, then the current terrain face is the one we want to render.
			bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
			meshFilters[i].gameObject.SetActive(renderFace);
		}
	}

	// Method to call to generate the planet.
	public void GeneratePlanet()
	{
		Initialize();
		GenerateMesh();
		GenerateColours();
	}

	// If only the shape settings have changed, call this method.
	public void OnShapeSettingsUpdated()
	{
		if (autoUpdate)
		{
			Initialize();
			GenerateMesh();
		}
	}

	// If only the colour settings have changed, call this method.
	public void OnColourSettingsUpdated()
	{
		if (autoUpdate)
		{
			Initialize();
			GenerateColours();
		}
	}


	private void GenerateMesh()
	{
		for (int i = 0; i < 6; i++)
		{
			if (meshFilters[i].gameObject.activeSelf)
			{
				terrainFaces[i].ConstructMesh();
			}
		}
		colourGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
	}

	void GenerateColours()
	{
		// Loop through meshes and set material's colour to the colour in our settings.
		foreach (MeshFilter mesh in meshFilters)
		{
			colourGenerator.UpdateColours();
		}
	}

}