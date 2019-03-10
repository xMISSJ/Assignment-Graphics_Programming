using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
	public float gizmoSize = .75f;
	public Color gizmoColour = Color.yellow;

	private void OnDrawGizmos()
	{
		Gizmos.color = gizmoColour;
		Gizmos.DrawWireSphere(transform.position, gizmoSize);
	}
}
