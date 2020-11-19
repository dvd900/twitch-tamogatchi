// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Beehive"
{
	Properties
	{
		_normal_stage_2("normal_stage_2", 2D) = "bump" {}
		_albedo_stage_3("albedo_stage_3", 2D) = "bump" {}
		_normal_stage_1("normal_stage_1", 2D) = "white" {}
		_albedo_stage_1("albedo_stage_1", 2D) = "white" {}
		_MetallicSmoothness_stage_2("Metallic/ Smoothness_stage_2", 2D) = "white" {}
		_MetallicSmoothness_stage_1("Metallic/ Smoothness_stage_1", 2D) = "white" {}
		_albedo_tint("albedo_tint", Color) = (0,0,0,0)
		_Smoothness("Smoothness", Range( -1 , 1)) = 0
		_Stage3("Stage3", Range( 0 , 1)) = 0
		_emission("emission", Color) = (0,0,0,0)
		_Texture0("Texture 0", 2D) = "white" {}
		_EmissionMap("EmissionMap", 2D) = "white" {}
		_Crack_emission("Crack_emission", Float) = 30.65
		_TextureSample7("Texture Sample 7", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 4.6
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

		UNITY_DECLARE_TEX2D_NOSAMPLER(_normal_stage_1);
		uniform float4 _normal_stage_1_ST;
		SamplerState sampler_normal_stage_1;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_normal_stage_2);
		uniform float4 _normal_stage_2_ST;
		SamplerState sampler_normal_stage_2;
		uniform float _Stage3;
		uniform float4 _albedo_tint;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_albedo_stage_1);
		uniform float4 _albedo_stage_1_ST;
		SamplerState sampler_albedo_stage_1;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_albedo_stage_3);
		uniform float4 _albedo_stage_3_ST;
		SamplerState sampler_albedo_stage_3;
		uniform float4 _emission;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_Texture0);
		uniform float4 _Texture0_ST;
		SamplerState sampler_Texture0;
		uniform float _Crack_emission;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_EmissionMap);
		uniform float4 _EmissionMap_ST;
		SamplerState sampler_EmissionMap;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_TextureSample7);
		uniform float4 _TextureSample7_ST;
		SamplerState sampler_TextureSample7;
		uniform float _Smoothness;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_MetallicSmoothness_stage_1);
		uniform float4 _MetallicSmoothness_stage_1_ST;
		SamplerState sampler_MetallicSmoothness_stage_1;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_MetallicSmoothness_stage_2);
		uniform float4 _MetallicSmoothness_stage_2_ST;
		SamplerState sampler_MetallicSmoothness_stage_2;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_normal_stage_1 = i.uv_texcoord * _normal_stage_1_ST.xy + _normal_stage_1_ST.zw;
			float2 uv_normal_stage_2 = i.uv_texcoord * _normal_stage_2_ST.xy + _normal_stage_2_ST.zw;
			float3 lerpResult49 = lerp( UnpackNormal( SAMPLE_TEXTURE2D( _normal_stage_1, sampler_normal_stage_1, uv_normal_stage_1 ) ) , UnpackNormal( SAMPLE_TEXTURE2D( _normal_stage_2, sampler_normal_stage_2, uv_normal_stage_2 ) ) , _Stage3);
			o.Normal = lerpResult49;
			float2 uv_albedo_stage_1 = i.uv_texcoord * _albedo_stage_1_ST.xy + _albedo_stage_1_ST.zw;
			float2 uv_albedo_stage_3 = i.uv_texcoord * _albedo_stage_3_ST.xy + _albedo_stage_3_ST.zw;
			float4 lerpResult58 = lerp( SAMPLE_TEXTURE2D( _albedo_stage_1, sampler_albedo_stage_1, uv_albedo_stage_1 ) , SAMPLE_TEXTURE2D( _albedo_stage_3, sampler_albedo_stage_3, uv_albedo_stage_3 ) , _Stage3);
			o.Albedo = ( _albedo_tint * lerpResult58 ).rgb;
			float2 uv_Texture0 = i.uv_texcoord * _Texture0_ST.xy + _Texture0_ST.zw;
			float2 uv_EmissionMap = i.uv_texcoord * _EmissionMap_ST.xy + _EmissionMap_ST.zw;
			float4 lerpResult82 = lerp( SAMPLE_TEXTURE2D( _Texture0, sampler_Texture0, uv_Texture0 ) , CalculateContrast(( _Stage3 * _Crack_emission ),SAMPLE_TEXTURE2D( _EmissionMap, sampler_EmissionMap, uv_EmissionMap )) , 1.0);
			float2 uv_TextureSample7 = i.uv_texcoord * _TextureSample7_ST.xy + _TextureSample7_ST.zw;
			float4 tex2DNode124 = SAMPLE_TEXTURE2D( _TextureSample7, sampler_TextureSample7, uv_TextureSample7 );
			float4 lerpResult134 = lerp( lerpResult82 , tex2DNode124 , _Stage3);
			o.Emission = ( _emission * ( ( lerpResult82 + 0.46 ) * lerpResult134 ) ).rgb;
			float2 uv_MetallicSmoothness_stage_1 = i.uv_texcoord * _MetallicSmoothness_stage_1_ST.xy + _MetallicSmoothness_stage_1_ST.zw;
			float2 uv_MetallicSmoothness_stage_2 = i.uv_texcoord * _MetallicSmoothness_stage_2_ST.xy + _MetallicSmoothness_stage_2_ST.zw;
			float4 lerpResult63 = lerp( CalculateContrast(_Smoothness,SAMPLE_TEXTURE2D( _MetallicSmoothness_stage_1, sampler_MetallicSmoothness_stage_1, uv_MetallicSmoothness_stage_1 )) , CalculateContrast(_Smoothness,SAMPLE_TEXTURE2D( _MetallicSmoothness_stage_2, sampler_MetallicSmoothness_stage_2, uv_MetallicSmoothness_stage_2 )) , _Stage3);
			o.Metallic = lerpResult63.r;
			o.Smoothness = lerpResult63.r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18703
131;308;1304;576;2177.869;801.5315;3.395751;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;80;-1704.389,462.418;Inherit;True;Property;_EmissionMap;EmissionMap;16;0;Create;True;0;0;False;0;False;None;c4616dceb5d6043ccaac0c28b1fde7fe;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;50;-1654.551,6.724029;Inherit;True;Property;_Stage3;Stage3;9;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-1593.655,275.3888;Inherit;False;Property;_Crack_emission;Crack_emission;17;0;Create;True;0;0;False;0;False;30.65;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;75;-510.4402,37.59376;Inherit;True;Property;_Texture0;Texture 0;15;0;Create;True;0;0;False;0;False;None;0000000000000000f000000000000000;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;-1332.638,248.878;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;81;-1484.61,464.4467;Inherit;True;Property;_TextureSample3;Texture Sample 3;16;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleContrastOpNode;78;-1089.968,455.1144;Inherit;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;76;-1118.644,183.0636;Inherit;True;Property;_TextureSample0;Texture Sample 0;16;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;102;-1449.303,384.158;Inherit;False;Constant;_Float2;Float 2;19;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;82;-744.7468,298.4689;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;55;-26.53729,-1049.389;Inherit;True;Property;_albedo_stage_3;albedo_stage_3;2;0;Create;True;0;0;False;0;False;None;7ea262a0f13f744008a38bc5d0222806;False;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;124;-1155.323,707.4395;Inherit;True;Property;_TextureSample7;Texture Sample 7;18;0;Create;True;0;0;False;0;False;-1;None;bf0563a484de94ee59b91fbfa7a7a92e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;52;74.80679,-564.2422;Inherit;True;Property;_albedo_stage_1;albedo_stage_1;4;0;Create;True;0;0;False;0;False;None;f92e1a06f1a534c708a4366adb23316a;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;138;-418.3433,440.2055;Inherit;False;Constant;_Float0;Float 0;19;0;Create;True;0;0;False;0;False;0.46;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;137;-406.5338,331.5582;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;48;-1396.053,-409.8892;Inherit;True;Property;_normal_stage_2;normal_stage_2;1;0;Create;True;0;0;False;0;False;None;284ae2f03765a4613964093de185269d;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;5;-1386.473,-174.0339;Inherit;True;Property;_normal_stage_1;normal_stage_1;3;0;Create;True;0;0;False;0;False;None;be6e0c004b82146cc91c5365cbd02917;True;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;61;-869.129,919.6793;Inherit;True;Property;_MetallicSmoothness_stage_2;Metallic/ Smoothness_stage_2;5;0;Create;True;0;0;False;0;False;-1;None;b7ab0c650c4eb403ba8a89617a852297;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-733.4071,1390.88;Inherit;False;Property;_Smoothness;Smoothness;8;0;Create;True;0;0;False;0;False;0;-0.27;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;57;288.4823,-1039.27;Inherit;True;Property;_TextureSample4;Texture Sample 4;2;0;Create;True;0;0;False;0;False;-1;None;ed66bd548134140a6a2174cd6a054fdb;True;0;True;bump;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-816.2921,1180.255;Inherit;True;Property;_MetallicSmoothness_stage_1;Metallic/ Smoothness_stage_1;6;0;Create;True;0;0;False;0;False;-1;None;2919ebc556f0648cfb76d9fcbbd10ea4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;134;-503.3853,575.2158;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;54;338.6223,-598.6606;Inherit;True;Property;_TextureSample5;Texture Sample 5;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;59;973.5703,-417.5304;Inherit;False;Property;_albedo_tint;albedo_tint;7;0;Create;True;0;0;False;0;False;0,0,0,0;1,0.9114347,0.4292447,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;42;-177.6134,598.3632;Inherit;False;Property;_emission;emission;14;0;Create;True;0;0;False;0;False;0,0,0,0;1,0.3787874,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleContrastOpNode;62;-435.3048,929.8516;Inherit;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;58;942.3525,-1043.327;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;13;-402.3538,1204.838;Inherit;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;-245.9236,263.0632;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;47;-992.8881,-409.4171;Inherit;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;False;-1;None;ed66bd548134140a6a2174cd6a054fdb;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-973.8604,-182.9575;Inherit;True;Property;_Normal;Normal;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;71;237.0945,1369.383;Inherit;False;Property;_HeightAmt;HeightAmt;10;0;Create;True;0;0;False;0;False;0;2.97;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;41;1838.96,517.7576;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;112;-934.3761,-1076.844;Inherit;False;Constant;_Float3;Float 3;20;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;113;-97.92956,92.68683;Inherit;False;Property;_normalintensity;normal intensity;19;0;Create;True;0;0;False;0;False;39.02;0.22;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;1551.813,440.693;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;106;374.9554,-281.983;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;1857.298,148.3786;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;713.7784,-236.4651;Inherit;True;Property;_albedo;albedo;0;0;Create;True;0;0;False;0;False;None;f92e1a06f1a534c708a4366adb23316a;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;507.8389,726.2844;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;37;1081.816,716.9438;Inherit;False;Property;_SickAmt;SickAmt;13;0;Create;True;0;0;False;0;False;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;342.8018,1109.644;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;63;-135.4871,1099.879;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;744.6649,98.42548;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;989.157,62.22262;Inherit;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;73;533.0754,1266.695;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;1419.962,-278.6295;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;26.77443,311.877;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;1508.563,576.5839;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;135;-783.6297,820.7782;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;49;-405.9655,-409.7125;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexturePropertyNode;70;183.8905,855.2235;Inherit;True;Property;_Heightmap;Heightmap;11;0;Create;True;0;0;False;0;False;None;2bdbb83edf10d4f55ad87f3b754b36d5;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;69;474.584,826.2686;Inherit;True;Property;_TextureSample6;Texture Sample 6;19;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;33;1040.968,495.0761;Inherit;True;Property;_TextureSample1;Texture Sample 1;12;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;105;1010.11,-713.9209;Inherit;False;Constant;_Color1;Color 1;19;0;Create;True;0;0;False;0;False;0.4433962,0.4433962,0.4433962,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;39;1007.814,274.9669;Inherit;False;Constant;_Color0;Color 0;10;0;Create;True;0;0;False;0;False;0.7455336,1,0.3537736,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;583.9985,-17.32438;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Beehive;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;32;10;25;False;1;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;True;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;103;0;50;0
WireConnection;103;1;79;0
WireConnection;81;0;80;0
WireConnection;78;1;81;0
WireConnection;78;0;103;0
WireConnection;76;0;75;0
WireConnection;82;0;76;0
WireConnection;82;1;78;0
WireConnection;82;2;102;0
WireConnection;137;0;82;0
WireConnection;137;1;138;0
WireConnection;57;0;55;0
WireConnection;134;0;82;0
WireConnection;134;1;124;0
WireConnection;134;2;50;0
WireConnection;54;0;52;0
WireConnection;62;1;61;0
WireConnection;62;0;12;0
WireConnection;58;0;54;0
WireConnection;58;1;57;0
WireConnection;58;2;50;0
WireConnection;13;1;3;0
WireConnection;13;0;12;0
WireConnection;136;0;137;0
WireConnection;136;1;134;0
WireConnection;47;0;48;0
WireConnection;2;0;5;0
WireConnection;41;0;10;0
WireConnection;41;1;40;0
WireConnection;41;2;36;0
WireConnection;40;0;10;0
WireConnection;40;1;39;0
WireConnection;106;0;50;0
WireConnection;10;1;1;0
WireConnection;74;0;72;0
WireConnection;74;1;73;0
WireConnection;72;0;69;0
WireConnection;72;1;71;0
WireConnection;63;0;13;0
WireConnection;63;1;62;0
WireConnection;63;2;50;0
WireConnection;1;0;4;0
WireConnection;1;1;7;0
WireConnection;60;0;59;0
WireConnection;60;1;58;0
WireConnection;77;0;42;0
WireConnection;77;1;136;0
WireConnection;36;0;33;0
WireConnection;36;1;37;0
WireConnection;135;0;124;0
WireConnection;49;0;2;0
WireConnection;49;1;47;0
WireConnection;49;2;50;0
WireConnection;69;0;70;0
WireConnection;0;0;60;0
WireConnection;0;1;49;0
WireConnection;0;2;77;0
WireConnection;0;3;63;0
WireConnection;0;4;63;0
ASEEND*/
//CHKSM=4BC9FA52AD0722253264C9942855C190A5FA4723