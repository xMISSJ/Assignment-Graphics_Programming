using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactory
{
	public static INoiseFilter CreateNoiseFilter(NoiseSettings settings)
	{
		// The type of noise want to create, depends on filter type specified in the settings.
		switch (settings.filterType)
		{
			case NoiseSettings.FilterType.Simple:
				return new SimpleNoiseFilter(settings.simpleNoiseSettings);
			case NoiseSettings.FilterType.Rigid:
				return new RigidNoiseFilter(settings.rigidNoiseSettings);
		}
		return null;
	}
}
