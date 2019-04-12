/// <summary>
/// The terrainFace class.
/// Is responsible for constructing its mesh.
/// </summary>
using UnityEngine;

public class TerrainFace
{
	ShapeGenerator shapeGenerator;

	Mesh mesh;
	int resolution;		               // Amount of details regarding the mesh (256 is max).
	Vector3 localUp;                   // Direction the mesh will be facing.
	Vector3 axisA;
	Vector3 axisB;

	public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int planetChunkSize, Vector3 localUp)
	{
		this.shapeGenerator = shapeGenerator;
		this.mesh = mesh;
		this.resolution = planetChunkSize;
		this.localUp = localUp;

		// The localUp coordinates are used, but swapped.
		// x = y, y = z, z = x.
		axisA = new Vector3(localUp.y, localUp.z, localUp.x);
		// The axisB coordinates are perpendicular to localUp and axisA.
		// For this cross product is used to calculate axisB.
		axisB = Vector3.Cross(localUp, axisA);
	}

	public void ConstructMesh()
	{
		// Resolution is number of vertices along a single edge of the face.
		// Total resolution is thus squared.
		Vector3[] vertices = new Vector3[resolution * resolution];
		Vector2[] uvs = new Vector2[vertices.Length];
		// UVs array has same length as the vertices array.
		int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
		int triIndex = 0;

		// Don't lose uv data which has already been calculated.
		Vector2[] uv = (mesh.uv.Length == vertices.Length) ? mesh.uv : new Vector2 [vertices.Length];

			for (int y = 0; y < resolution; y++)
			{
				for (int x = 0; x < resolution; x++)
				{
					// Number of iterations innerloop + number of iterations outerloop.
					// As we're doing an entire row for each innerloop, we have to multiply it with resolution.
					// Can also be done with int = 0; adding it to only the outerloop and then add i++ in the innerloop.
					int i = x + y * resolution;
					// How close is each of this loop to complete. Use this to define vertex.
					Vector2 percent = new Vector2(x, y) / (resolution - 1);
					Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;

					// In order to create a sphere, you want all the vertices to be the same distance away from the centre.
					Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
					float unscaledElevation = shapeGenerator.CalculateUnscaledElevation(pointOnUnitSphere);
					vertices[i] = pointOnUnitSphere * shapeGenerator.GetScaledElevation(unscaledElevation);
					uv[i].y = unscaledElevation;

					// We can create triangles, so long the triangles aren't the right edge or bottom edge.
					if (x != resolution - 1 && y != resolution - 1)
					{

						// First triangle.
						triangles[triIndex] = i;                            // First vertex.            
						triangles[triIndex + 1] = i + resolution + 1;       // Second vertex.
						triangles[triIndex + 2] = i + resolution;           // Third vertex.

						// Second triangle.
						triangles[triIndex + 3] = i;                        // First vertex.
						triangles[triIndex + 4] = i + 1;                    // Second vertex.
						triangles[triIndex + 5] = i + resolution + 1;       // Third vertex.

						// Added 6 vertices, so add 6.
						triIndex += 6;
					}
				}
			}

		//Generate the UVs.
		//for (int i = 0; i < vertices.Length; i++)
		//{
		//	uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
		//}
		uvs = new Vector2[vertices.Length];
		for (int i = 0; i < resolution; i++)
		{
			uvs[i * 2 + 1] = new Vector2(((float)i) / (resolution + 1), 0);
			uvs[i * 2] = new Vector2(((float)i) / (resolution + 1), 1);
		}

		//Put it back.
	   mesh.Clear();
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.RecalculateNormals();

		if (mesh.uv.Length == uv.Length)
		{
			mesh.uv = uv;
		}
	}

	public void UpdateUvs(ColourGenerator colourGenerator)
	{
		// Resolution * resolution is number of vertices.
		Vector2[] uv = mesh.uv;

		for (int y = 0; y < resolution; y++)
		{
			for (int x = 0; x < resolution; x++)
			{
				int i = x + y * resolution;
				Vector2 percent = new Vector2(x, y) / (resolution - 1);
				Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
				Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
				uv[i].x = colourGenerator.BiomePercentFromPoint(pointOnUnitSphere);
			}
		}
		mesh.uv = uv;
	}
}
