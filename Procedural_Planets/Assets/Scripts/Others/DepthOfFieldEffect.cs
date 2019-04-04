using UnityEngine;
using System;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class DepthOfFieldEffect : MonoBehaviour
{
	[HideInInspector]
	public Shader dofShader;

	[NonSerialized]
	Material dofMaterial;

	const int circleOfConfusionPass = 0;
	const int preFilterPass = 1;
	const int bokehPass = 2;
	const int postFilterPass = 3;

	[Range(0.1f, 100f)]
	public float focusDistance = 10f;

	[Range(0.1f, 10f)]
	public float focusRange = 3f;

	[Range(1f, 10f)]
	public float bokehRadius = 4f;

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (dofMaterial == null)
		{
			dofMaterial = new Material(dofShader);
			dofMaterial.hideFlags = HideFlags.HideAndDontSave;
			dofMaterial.SetFloat("_BokehRadius", bokehRadius);
			dofMaterial.SetFloat("_FocusDistance", focusDistance);
			dofMaterial.SetFloat("_FocusRange", focusRange);
		}

		RenderTexture coc = RenderTexture.GetTemporary(
		source.width, source.height, 0,
		RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear
		);

		int width = source.width / 2;
		int height = source.height / 2;
		RenderTextureFormat format = source.format;
		RenderTexture dof0 = RenderTexture.GetTemporary(width, height, 0, format);
		RenderTexture dof1 = RenderTexture.GetTemporary(width, height, 0, format);

		dofMaterial.SetTexture("_CoCTex", coc);

		Graphics.Blit(source, coc, dofMaterial, circleOfConfusionPass);
		Graphics.Blit(source, dof0, dofMaterial, preFilterPass);
		Graphics.Blit(dof0, dof1, dofMaterial, bokehPass);
		Graphics.Blit(dof1, dof0, dofMaterial, postFilterPass);
		Graphics.Blit(dof0, destination);

		RenderTexture.ReleaseTemporary(coc);
		RenderTexture.ReleaseTemporary(dof0);
		RenderTexture.ReleaseTemporary(dof1);
	}
}