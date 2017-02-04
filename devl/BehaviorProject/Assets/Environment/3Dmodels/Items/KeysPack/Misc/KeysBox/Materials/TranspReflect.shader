// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Transparent/Gem"
{
	Properties 
	{
_Base("_Base", 2D) = "black" {}
_CubeMap("_CubeMap", Cube) = "black" {}
_Color_Spec("_Color_Spec", Color) = (1,1,1,1)
_Range_Spec("_Range_Spec", Range(0,1) ) = 0.5
_Gloss("_Gloss", Range(0,1) ) = 0.5
_Reflection_Color("_Reflection_Color", Color) = (1,1,1,1)
_Trasparency("_Trasparency", Range(0,1) ) = 0.5

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Transparent"
"IgnoreProjector"="False"
"RenderType"="Transparent"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  addshadow exclude_path:prepass fullforwardshadows alpha decal:blend vertex:vert
#pragma target 3.0


sampler2D _Base;
samplerCUBE _CubeMap;
float4 _Color_Spec;
float _Range_Spec;
float _Gloss;
float4 _Reflection_Color;
float _Trasparency;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float3 simpleWorldRefl;

			};

			void vert (inout appdata_full v, out Input o) {
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);

o.simpleWorldRefl = -reflect( normalize(WorldSpaceViewDir(v.vertex)), normalize(mul((float3x3)unity_ObjectToWorld, SCALED_NORMAL)));

			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Tex2D0_1_NoInput = float4(0,0,0,0);
float4 Tex2D0=tex2D(_Base,Tex2D0_1_NoInput.xy);
float4 TexCUBE0=texCUBE(_CubeMap,float4( IN.simpleWorldRefl.x, IN.simpleWorldRefl.y,IN.simpleWorldRefl.z,1.0 ));
float4 Add0=TexCUBE0 + _Reflection_Color;
float4 Multiply0=Tex2D0 * Add0;
float4 Add1=_Color_Spec + _Range_Spec.xxxx;
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_2_NoInput = float4(0,0,0,0);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Multiply0;
o.Specular = _Gloss.xxxx;
o.Gloss = Add1;
o.Alpha = _Trasparency.xxxx;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}