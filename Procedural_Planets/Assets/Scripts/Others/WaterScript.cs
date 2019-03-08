using UnityEngine;

public class WaterScript : MonoBehaviour
{

	private Mesh mesh;

	private Vector3[] vertices;
	private Vector2[] uvs;


	// Start is called before the first frame update.
	private void Start()
	{
		mesh = GetComponent<MeshFilter>().mesh;
		vertices = mesh.vertices;

		// Generates random "waves". Using Mathf.Cos to make it less like triangles and more round.
		for (int i = 0; i < vertices.Length; i++)
		{
			vertices[i] += new Vector3(Mathf.Cos(Random.Range(-0.8f, 0.8f)), Mathf.Cos(Random.Range(-0.8f, 0.8f)), Mathf.Cos(Random.Range(-0.8f, 0.8f)));
		}

		// Put vertices back.
		mesh.vertices = vertices;

	}

	// Update is called once per frame.
	private void Update()
	{ 

	}

	private void OnDrawGizmos()
	{
		if (vertices == null) return;
		Gizmos.color = Color.red;
		for (int i = 0; i < mesh.vertices.Length; i++)
		{
			Gizmos.DrawSphere(mesh.vertices[i], .05f);
		}
	}
}
