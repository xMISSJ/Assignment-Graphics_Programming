using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
	private Planet planet;
	private Editor shapeEditor;
	private Editor colourEditor;

	public override void OnInspectorGUI()
	{
		using (var check = new EditorGUI.ChangeCheckScope())
		{
			base.OnInspectorGUI();
			// Replaces onValidate method in Planet.cs. This so all the updates will be done in this editor script.
			if (check.changed)
			{
				planet.GeneratePlanet();
			}
		DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldout, ref shapeEditor);
		DrawSettingsEditor(planet.colourSettings, planet.OnColourSettingsUpdated, ref planet.colourSettingsFoldout, ref colourEditor);
	}

		// Button to manually generate a planet.
		if (GUILayout.Button("Generate Planet"))
		{
			planet.GeneratePlanet();
		}

		// Draw editor for shape- and coloursettings.
		// Ref is used here to modify the value of the reference itself, rather than the object which the reference refers.
		void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
	{
		// Only draw settings object when it's not null.
		if (settings != null)
			// Value of true tells whether the Titlebar is always folded out or not.
			foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
		using (var check = new EditorGUI.ChangeCheckScope())
		{
			if (foldout)
			{
				// Null will give the default editor type.
				// With the CreateCachedEditor method, the editor that has been passed in will be stored in the Editor values above (shapeEditor and colourEditor).
				// Only creates new editor when it has to.
				CreateCachedEditor(settings, null, ref editor);

				editor.OnInspectorGUI();

				// Update editor automatically.
				if (check.changed)
				{
					if (onSettingsUpdated != null)
					{
						onSettingsUpdated();
					}
				}
			}
		}
	}
}

	private void OnEnable()
	{
		planet = (Planet)target;
	}
}
