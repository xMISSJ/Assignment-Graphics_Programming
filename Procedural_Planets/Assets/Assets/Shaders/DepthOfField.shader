Shader "Hidden/DepthOfField" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
	}

	CGINCLUDE
#include "UnityCG.cginc"

	sampler2D _MainTex;
	sampler2D _CameraDepthTexture;
	float4 _MainTex_TexelSize;

	float _FocusDistance;
	float _FocusRange;

	struct VertexData {
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct Interpolators {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	Interpolators VertexProgram(VertexData v) {
		Interpolators i;
		i.pos = UnityObjectToClipPos(v.vertex);
		i.uv = v.uv;
		return i;
	}

	ENDCG

		SubShader{
			Cull Off
			ZTest Always
			ZWrite Off

			Pass { // 0 CircleOfConfusionPass
				CGPROGRAM
					#pragma vertex VertexProgram
					#pragma fragment FragmentProgram

					half FragmentProgram(Interpolators i) : SV_Target {
					half depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
					depth = LinearEyeDepth(depth);

					float coc = (depth - _FocusDistance) / _FocusRange;
					return coc;
					}
				ENDCG
			}
	}
}