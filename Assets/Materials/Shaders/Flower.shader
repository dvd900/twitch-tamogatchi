// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Flower"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_albedo("albedo", 2D) = "white" {}
		_Texture0("Texture 0", 2D) = "bump" {}
		_normal("normal", 2D) = "white" {}
		_MetallicSmoothness("Metallic/ Smoothness", 2D) = "white" {}
		_Transmissionn("Transmissionn", Color) = (0,0,0,0)
		_albedo_tint("albedo_tint", Color) = (0,0,0,0)
		_Smoothness("Smoothness", Range( -1 , 1)) = 0
		_NormalIntensity("Normal Intensity", Range( 0 , 2)) = 0
		_Color0("Color 0", Color) = (0,0,0,0)
		_AlphaMask("Alpha Mask", 2D) = "white" {}
		_Metallic("Metallic", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#pragma target 3.0
		#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
		#else//ASE Sampling Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex2D(tex,coord)
		#endif//ASE Sampling Macros

		#pragma surface surf StandardCustom keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		struct SurfaceOutputStandardCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			half3 Transmission;
		};

		UNITY_DECLARE_TEX2D_NOSAMPLER(_Texture0);
		uniform float4 _Texture0_ST;
		SamplerState sampler_Texture0;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_normal);
		uniform float4 _normal_ST;
		SamplerState sampler_normal;
		uniform float _NormalIntensity;
		uniform float4 _albedo_tint;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_albedo);
		SamplerState sampler_albedo;
		uniform float4 _Color0;
		uniform float _Metallic;
		uniform float _Smoothness;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_MetallicSmoothness);
		uniform float4 _MetallicSmoothness_ST;
		SamplerState sampler_MetallicSmoothness;
		uniform float4 _Transmissionn;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_AlphaMask);
		uniform float4 _AlphaMask_ST;
		SamplerState sampler_AlphaMask;
		uniform float _Cutoff = 0.5;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			half3 transmission = max(0 , -dot(s.Normal, gi.light.dir)) * gi.light.color * s.Transmission;
			half4 d = half4(s.Albedo * transmission , 0);

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + d;
		}

		inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi )
		{
			#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
				gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
			#else
				UNITY_GLOSSY_ENV_FROM_SURFACE( g, s, data );
				gi = UnityGlobalIllumination( data, s.Occlusion, s.Normal, g );
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardCustom o )
		{
			float2 uv_Texture0 = i.uv_texcoord * _Texture0_ST.xy + _Texture0_ST.zw;
			float2 uv_normal = i.uv_texcoord * _normal_ST.xy + _normal_ST.zw;
			float3 lerpResult32 = lerp( UnpackNormal( SAMPLE_TEXTURE2D( _Texture0, sampler_Texture0, uv_Texture0 ) ) , UnpackNormal( SAMPLE_TEXTURE2D( _normal, sampler_normal, uv_normal ) ) , _NormalIntensity);
			o.Normal = lerpResult32;
			o.Albedo = ( _albedo_tint * SAMPLE_TEXTURE2D( _albedo, sampler_albedo, i.uv_texcoord ) ).rgb;
			o.Emission = _Color0.rgb;
			o.Metallic = _Metallic;
			float2 uv_MetallicSmoothness = i.uv_texcoord * _MetallicSmoothness_ST.xy + _MetallicSmoothness_ST.zw;
			float4 temp_output_13_0 = CalculateContrast(_Smoothness,SAMPLE_TEXTURE2D( _MetallicSmoothness, sampler_MetallicSmoothness, uv_MetallicSmoothness ));
			o.Smoothness = temp_output_13_0.r;
			o.Transmission = _Transmissionn.rgb;
			o.Alpha = 1;
			float2 uv_AlphaMask = i.uv_texcoord * _AlphaMask_ST.xy + _AlphaMask_ST.zw;
			clip( SAMPLE_TEXTURE2D( _AlphaMask, sampler_AlphaMask, uv_AlphaMask ).r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18400
220;186;1220;576;634.2724;284.767;2.048296;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;4;703.9744,-220.1252;Inherit;True;Property;_albedo;albedo;1;0;Create;True;0;0;False;0;False;None;1594a129dfd8742258fb60f5b5fa5b56;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;734.7583,151.9213;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;28;-1141.733,347.2542;Inherit;True;Property;_Texture0;Texture 0;2;0;Create;True;0;0;False;0;False;None;None;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;5;-1147.137,125.5067;Inherit;True;Property;_normal;normal;3;0;Create;True;0;0;False;0;False;None;719ffde8a0a1442c786961ffc0e729af;True;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;25;-864.4383,334.2274;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;False;-1;None;ed66bd548134140a6a2174cd6a054fdb;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-796.5531,722.5919;Inherit;False;Property;_Smoothness;Smoothness;8;0;Create;True;0;0;False;0;False;0;-0.6;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;42;336.8742,466.0071;Inherit;True;Property;_AlphaMask;Alpha Mask;11;0;Create;True;0;0;False;0;False;None;05070da85bb34477fbf9a9140d79cec9;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;1;1001.045,66.18525;Inherit;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-803.0107,466.7692;Inherit;True;Property;_MetallicSmoothness;Metallic/ Smoothness;4;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-892.8236,127.3283;Inherit;True;Property;_Normal;Normal;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;9;1037.375,-145.6244;Inherit;False;Property;_albedo_tint;albedo_tint;7;0;Create;True;0;0;False;0;False;0,0,0,0;0.9150943,0.616597,0.4704965,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-557.9949,380.6898;Inherit;False;Property;_NormalIntensity;Normal Intensity;9;0;Create;True;0;0;False;0;False;0;0.57;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;44;16.3789,202.651;Inherit;False;Property;_Color0;Color 0;10;0;Create;True;0;0;False;0;False;0,0,0,0;0.7924528,0.44482,0.44482,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;47;176.651,292.9747;Inherit;False;Property;_Transmissionn;Transmissionn;5;0;Create;True;0;0;False;0;False;0,0,0,0;0.8588235,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;32;-362.0203,226.5241;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;1857.298,148.3786;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;8;172.1822,730.0621;Inherit;False;Property;_Emission;Emission;6;0;Create;True;0;0;False;0;False;0,0,0,0;0.4056586,0,0.1098734,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleContrastOpNode;13;-387.2462,479.3641;Inherit;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;43;569.0937,397.9624;Inherit;True;Property;_TextureSample2;Texture Sample 2;12;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;14;-70.75651,485.3361;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;48;125.6456,10.18753;Inherit;False;Property;_Metallic;Metallic;12;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;368.4063,-142.3795;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Flower;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;True;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;25;0;28;0
WireConnection;1;0;4;0
WireConnection;1;1;7;0
WireConnection;2;0;5;0
WireConnection;32;0;25;0
WireConnection;32;1;2;0
WireConnection;32;2;16;0
WireConnection;10;0;9;0
WireConnection;10;1;1;0
WireConnection;13;1;3;0
WireConnection;13;0;12;0
WireConnection;43;0;42;0
WireConnection;14;0;13;0
WireConnection;0;0;10;0
WireConnection;0;1;32;0
WireConnection;0;2;44;0
WireConnection;0;3;48;0
WireConnection;0;4;13;0
WireConnection;0;6;47;0
WireConnection;0;10;43;0
ASEEND*/
//CHKSM=7166880F9763E27F73C83C00997984FD8B75F4E0