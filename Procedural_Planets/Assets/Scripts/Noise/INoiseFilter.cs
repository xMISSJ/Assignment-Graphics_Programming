using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INoiseFilter 
{
	// Define the one method that any class has to have if it wants to qualify as a noise filter.
	float Evaluate(Vector3 point);
}
