Shader "Hidden/DepthOfField" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
	}

	CGINCLUDE
#include "UnityCG.cginc"

	sampler2D _MainTex, _CameraDepthTexture;
	float4 _MainTex_TexelSize;
	float _FocusDistance, _FocusRange;

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

			Pass { // 0 circleOfConfusionPass
				CGPROGRAM
					#pragma vertex VertexProgram
					#pragma fragment FragmentProgram

					half FragmentProgram(Interpolators i) : SV_Target {
					float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
					depth = LinearEyeDepth(depth);
					float coc = (depth - _FocusDistance) / _FocusRange;
					coc = clamp(coc, -1, 1);
					return coc;
					}
				ENDCG
			}

			Pass { // 1 bokehPass
			CGPROGRAM
					#pragma vertex VertexProgram
					#pragma fragment FragmentProgram

					half4 FragmentProgram(Interpolators i) : SV_Target {
					half3 color = 0;
					for (int u = -4; u <= 4; u++) {
							for (int v = -4; v <= 4; v++) {
									float2 o = float2(u, v) * _MainTex_TexelSize.xy;
									color += tex2D(_MainTex, i.uv + o).rgb;
							}
					}
					color *= 1.0 / 81;
					return half4(color, 1);
					}
				ENDCG
		}
	}
}