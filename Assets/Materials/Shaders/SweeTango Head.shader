// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SweeTango Head"
{
	Properties
	{
		_albedo("albedo", 2D) = "white" {}
		_Texture0("Texture 0", 2D) = "bump" {}
		_normal("normal", 2D) = "white" {}
		_MetallicSmoothness("Metallic/ Smoothness", 2D) = "white" {}
		_albedo_tint("albedo_tint", Color) = (0,0,0,0)
		_Smoothness("Smoothness", Range( -1 , 1)) = 0
		_NormalIntensity("Normal Intensity", Range( 0 , 2)) = 0
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_AboutToHurl("About To Hurl", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
		#else//ASE Sampling Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex2D(tex,coord)
		#endif//ASE Sampling Macros

		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
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
		UNITY_DECLARE_TEX2D_NOSAMPLER(_TextureSample1);
		uniform float4 _TextureSample1_ST;
		SamplerState sampler_TextureSample1;
		uniform float _AboutToHurl;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_MetallicSmoothness);
		uniform float4 _MetallicSmoothness_ST;
		SamplerState sampler_MetallicSmoothness;
		uniform float _Smoothness;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Texture0 = i.uv_texcoord * _Texture0_ST.xy + _Texture0_ST.zw;
			float2 uv_normal = i.uv_texcoord * _normal_ST.xy + _normal_ST.zw;
			float3 lerpResult32 = lerp( UnpackNormal( SAMPLE_TEXTURE2D( _Texture0, sampler_Texture0, uv_Texture0 ) ) , UnpackNormal( SAMPLE_TEXTURE2D( _normal, sampler_normal, uv_normal ) ) , _NormalIntensity);
			o.Normal = lerpResult32;
			float4 temp_output_10_0 = ( _albedo_tint * SAMPLE_TEXTURE2D( _albedo, sampler_albedo, i.uv_texcoord ) );
			float4 color39 = IsGammaSpace() ? float4(0.7455336,1,0.3537736,0) : float4(0.5155908,1,0.1027432,0);
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float4 lerpResult41 = lerp( temp_output_10_0 , ( temp_output_10_0 * color39 ) , ( SAMPLE_TEXTURE2D( _TextureSample1, sampler_TextureSample1, uv_TextureSample1 ) * _AboutToHurl ));
			o.Albedo = lerpResult41.rgb;
			float2 uv_MetallicSmoothness = i.uv_texcoord * _MetallicSmoothness_ST.xy + _MetallicSmoothness_ST.zw;
			float4 tex2DNode3 = SAMPLE_TEXTURE2D( _MetallicSmoothness, sampler_MetallicSmoothness, uv_MetallicSmoothness );
			o.Metallic = tex2DNode3.r;
			float4 temp_output_13_0 = CalculateContrast(_Smoothness,tex2DNode3);
			o.Smoothness = temp_output_13_0.r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18400
328;131;1048;576;-96.68268;396.5611;2.087319;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;4;703.9744,-220.1252;Inherit;True;Property;_albedo;albedo;0;0;Create;True;0;0;False;0;False;None;6f5c5f75c269a41a6a9a265143d4c511;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;744.6649,98.42548;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;9;1037.375,-145.6244;Inherit;False;Property;_albedo_tint;albedo_tint;5;0;Create;True;0;0;False;0;False;0,0,0,0;1,0.9215686,0.9215686,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;989.157,62.22262;Inherit;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;28;-1141.733,347.2542;Inherit;True;Property;_Texture0;Texture 0;1;0;Create;True;0;0;False;0;False;None;ed66bd548134140a6a2174cd6a054fdb;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ColorNode;39;1007.814,274.9669;Inherit;False;Constant;_Color0;Color 0;10;0;Create;True;0;0;False;0;False;0.7455336,1,0.3537736,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;33;1040.968,495.0761;Inherit;True;Property;_TextureSample1;Texture Sample 1;8;0;Create;True;0;0;False;0;False;-1;None;48435bcd2e7f84cb4a5601557da573d3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;1857.298,148.3786;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;37;1081.816,716.9438;Inherit;False;Property;_AboutToHurl;About To Hurl;9;0;Create;True;0;0;False;0;False;0;0.773;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;5;-1147.137,125.5067;Inherit;True;Property;_normal;normal;2;0;Create;True;0;0;False;0;False;None;4397af4e0153b4391a758259decac975;True;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;1551.813,440.693;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;1508.563,576.5839;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-557.9949,380.6898;Inherit;False;Property;_NormalIntensity;Normal Intensity;7;0;Create;True;0;0;False;0;False;0;0.5;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-803.0107,466.7692;Inherit;True;Property;_MetallicSmoothness;Metallic/ Smoothness;3;0;Create;True;0;0;False;0;False;-1;None;15a0ef2b8d5aa43a7b281f9cbd10c53c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-796.5531,722.5919;Inherit;False;Property;_Smoothness;Smoothness;6;0;Create;True;0;0;False;0;False;0;-0.88;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;25;-864.4383,334.2274;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;False;-1;None;ed66bd548134140a6a2174cd6a054fdb;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-892.8236,127.3283;Inherit;True;Property;_Normal;Normal;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;14;-70.75651,485.3361;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;41;1838.96,517.7576;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;32;-362.0203,226.5241;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;8;172.1822,730.0621;Inherit;False;Property;_Emission;Emission;4;0;Create;True;0;0;False;0;False;0,0,0,0;0.4056586,0,0.1098734,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleContrastOpNode;13;-387.2462,479.3641;Inherit;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;421.9022,-156.2488;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SweeTango Head;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;True;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;0;4;0
WireConnection;1;1;7;0
WireConnection;10;0;9;0
WireConnection;10;1;1;0
WireConnection;40;0;10;0
WireConnection;40;1;39;0
WireConnection;36;0;33;0
WireConnection;36;1;37;0
WireConnection;25;0;28;0
WireConnection;2;0;5;0
WireConnection;14;0;13;0
WireConnection;41;0;10;0
WireConnection;41;1;40;0
WireConnection;41;2;36;0
WireConnection;32;0;25;0
WireConnection;32;1;2;0
WireConnection;32;2;16;0
WireConnection;13;1;3;0
WireConnection;13;0;12;0
WireConnection;0;0;41;0
WireConnection;0;1;32;0
WireConnection;0;3;3;0
WireConnection;0;4;13;0
ASEEND*/
//CHKSM=D364BEB30D8B46C56FAC4D174D119439478AAE15