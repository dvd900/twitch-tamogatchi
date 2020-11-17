// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Beehive2"
{
	Properties
	{
		_albedo_stage_2("albedo_stage_2", 2D) = "bump" {}
		_normal_stage_2("normal_stage_2", 2D) = "bump" {}
		_normal_stage_3("normal_stage_3", 2D) = "bump" {}
		_albedo_stage_3("albedo_stage_3", 2D) = "bump" {}
		_normal_stage_1("normal_stage_1", 2D) = "white" {}
		_albedo_stage_1("albedo_stage_1", 2D) = "white" {}
		_MetallicSmoothness_stage_2("Metallic/ Smoothness_stage_2", 2D) = "white" {}
		_MetallicSmoothness_stage_1("Metallic/ Smoothness_stage_1", 2D) = "white" {}
		_MetallicSmoothness_stage_3("Metallic/ Smoothness_stage_3", 2D) = "white" {}
		_albedo_tint("albedo_tint", Color) = (0,0,0,0)
		_Smoothness("Smoothness", Range( -1 , 1)) = 0
		_Stage2("Stage2", Range( 0 , 1)) = 0
		_Stage3("Stage3", Range( 0 , 1)) = 0
		_emission("emission", Color) = (0,0,0,0)
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
		uniform float _Stage2;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_normal_stage_3);
		uniform float4 _normal_stage_3_ST;
		SamplerState sampler_normal_stage_3;
		uniform float _Stage3;
		uniform float4 _albedo_tint;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_albedo_stage_1);
		uniform float4 _albedo_stage_1_ST;
		SamplerState sampler_albedo_stage_1;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_albedo_stage_2);
		uniform float4 _albedo_stage_2_ST;
		SamplerState sampler_albedo_stage_2;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_albedo_stage_3);
		uniform float4 _albedo_stage_3_ST;
		SamplerState sampler_albedo_stage_3;
		uniform float4 _emission;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_MetallicSmoothness_stage_2);
		uniform float4 _MetallicSmoothness_stage_2_ST;
		SamplerState sampler_MetallicSmoothness_stage_2;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_MetallicSmoothness_stage_3);
		uniform float4 _MetallicSmoothness_stage_3_ST;
		SamplerState sampler_MetallicSmoothness_stage_3;
		uniform float _Smoothness;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_MetallicSmoothness_stage_1);
		uniform float4 _MetallicSmoothness_stage_1_ST;
		SamplerState sampler_MetallicSmoothness_stage_1;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_normal_stage_1 = i.uv_texcoord * _normal_stage_1_ST.xy + _normal_stage_1_ST.zw;
			float2 uv_normal_stage_2 = i.uv_texcoord * _normal_stage_2_ST.xy + _normal_stage_2_ST.zw;
			float3 lerpResult45 = lerp( UnpackNormal( SAMPLE_TEXTURE2D( _normal_stage_1, sampler_normal_stage_1, uv_normal_stage_1 ) ) , UnpackNormal( SAMPLE_TEXTURE2D( _normal_stage_2, sampler_normal_stage_2, uv_normal_stage_2 ) ) , _Stage2);
			float2 uv_normal_stage_3 = i.uv_texcoord * _normal_stage_3_ST.xy + _normal_stage_3_ST.zw;
			float3 lerpResult49 = lerp( lerpResult45 , UnpackNormal( SAMPLE_TEXTURE2D( _normal_stage_3, sampler_normal_stage_3, uv_normal_stage_3 ) ) , _Stage3);
			o.Normal = lerpResult49;
			float2 uv_albedo_stage_1 = i.uv_texcoord * _albedo_stage_1_ST.xy + _albedo_stage_1_ST.zw;
			float2 uv_albedo_stage_2 = i.uv_texcoord * _albedo_stage_2_ST.xy + _albedo_stage_2_ST.zw;
			float4 lerpResult56 = lerp( SAMPLE_TEXTURE2D( _albedo_stage_1, sampler_albedo_stage_1, uv_albedo_stage_1 ) , SAMPLE_TEXTURE2D( _albedo_stage_2, sampler_albedo_stage_2, uv_albedo_stage_2 ) , _Stage2);
			float2 uv_albedo_stage_3 = i.uv_texcoord * _albedo_stage_3_ST.xy + _albedo_stage_3_ST.zw;
			float4 lerpResult58 = lerp( lerpResult56 , SAMPLE_TEXTURE2D( _albedo_stage_3, sampler_albedo_stage_3, uv_albedo_stage_3 ) , _Stage3);
			o.Albedo = ( _albedo_tint * lerpResult58 ).rgb;
			o.Emission = _emission.rgb;
			float2 uv_MetallicSmoothness_stage_2 = i.uv_texcoord * _MetallicSmoothness_stage_2_ST.xy + _MetallicSmoothness_stage_2_ST.zw;
			float4 tex2DNode61 = SAMPLE_TEXTURE2D( _MetallicSmoothness_stage_2, sampler_MetallicSmoothness_stage_2, uv_MetallicSmoothness_stage_2 );
			float2 uv_MetallicSmoothness_stage_3 = i.uv_texcoord * _MetallicSmoothness_stage_3_ST.xy + _MetallicSmoothness_stage_3_ST.zw;
			float4 tex2DNode64 = SAMPLE_TEXTURE2D( _MetallicSmoothness_stage_3, sampler_MetallicSmoothness_stage_3, uv_MetallicSmoothness_stage_3 );
			float4 lerpResult67 = lerp( tex2DNode61 , tex2DNode64 , _Stage3);
			o.Metallic = lerpResult67.r;
			float2 uv_MetallicSmoothness_stage_1 = i.uv_texcoord * _MetallicSmoothness_stage_1_ST.xy + _MetallicSmoothness_stage_1_ST.zw;
			float4 tex2DNode3 = SAMPLE_TEXTURE2D( _MetallicSmoothness_stage_1, sampler_MetallicSmoothness_stage_1, uv_MetallicSmoothness_stage_1 );
			float4 lerpResult63 = lerp( CalculateContrast(_Smoothness,tex2DNode3) , CalculateContrast(_Smoothness,tex2DNode61) , _Stage2);
			float4 lerpResult66 = lerp( lerpResult63 , CalculateContrast(_Smoothness,tex2DNode64) , _Stage3);
			o.Smoothness = lerpResult66.r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18703
125;202;1220;576;740.9077;349.8589;1.051933;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;51;-31.16731,-821.4181;Inherit;True;Property;_albedo_stage_2;albedo_stage_2;1;0;Create;True;0;0;False;0;False;None;e5e0873b11b57449f9820866ae98183b;False;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;52;-17.92133,-610.6065;Inherit;True;Property;_albedo_stage_1;albedo_stage_1;6;0;Create;True;0;0;False;0;False;None;f92e1a06f1a534c708a4366adb23316a;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;54;248.7919,-598.6606;Inherit;True;Property;_TextureSample5;Texture Sample 5;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-968.8725,763.7724;Inherit;True;Property;_MetallicSmoothness_stage_1;Metallic/ Smoothness_stage_1;8;0;Create;True;0;0;False;0;False;-1;None;2919ebc556f0648cfb76d9fcbbd10ea4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;53;273.3891,-810.6464;Inherit;True;Property;_TextureSample3;Texture Sample 3;2;0;Create;True;0;0;False;0;False;-1;None;ed66bd548134140a6a2174cd6a054fdb;True;0;True;bump;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;46;-244.1557,-25.42061;Inherit;False;Property;_Stage2;Stage2;12;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-985.1405,982.4075;Inherit;False;Property;_Smoothness;Smoothness;11;0;Create;True;0;0;False;0;False;0;-0.59;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;61;-954.8979,532.998;Inherit;True;Property;_MetallicSmoothness_stage_2;Metallic/ Smoothness_stage_2;7;0;Create;True;0;0;False;0;False;-1;None;2919ebc556f0648cfb76d9fcbbd10ea4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;43;-1171.282,-274.3116;Inherit;True;Property;_normal_stage_2;normal_stage_2;2;0;Create;True;0;0;False;0;False;None;ce6018a33f5bf4152ad287b454199a70;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;5;-1158.036,-63.49998;Inherit;True;Property;_normal_stage_1;normal_stage_1;5;0;Create;True;0;0;False;0;False;None;be6e0c004b82146cc91c5365cbd02917;True;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;55;-26.53729,-1049.389;Inherit;True;Property;_albedo_stage_3;albedo_stage_3;4;0;Create;True;0;0;False;0;False;None;839e33a5e0cda45e39d2878905b7d081;False;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;57;288.4823,-1039.27;Inherit;True;Property;_TextureSample4;Texture Sample 4;2;0;Create;True;0;0;False;0;False;-1;None;ed66bd548134140a6a2174cd6a054fdb;True;0;True;bump;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;56;625.25,-818.2706;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;62;-479.2201,545.5929;Inherit;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;64;-981.9105,315.9326;Inherit;True;Property;_MetallicSmoothness_stage_3;Metallic/ Smoothness_stage_3;9;0;Create;True;0;0;False;0;False;-1;None;b7ab0c650c4eb403ba8a89617a852297;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;50;-188.4295,-205.6491;Inherit;False;Property;_Stage3;Stage3;13;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;13;-482.8651,794.9611;Inherit;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;44;-866.7255,-263.5399;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;False;-1;None;ed66bd548134140a6a2174cd6a054fdb;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-891.3228,-51.55413;Inherit;True;Property;_Normal;Normal;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;48;-1166.652,-502.2825;Inherit;True;Property;_normal_stage_3;normal_stage_3;3;0;Create;True;0;0;False;0;False;None;284ae2f03765a4613964093de185269d;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.LerpOp;45;-514.8646,-271.1641;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;65;-467.5215,327.6886;Inherit;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;47;-851.6323,-492.1638;Inherit;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;False;-1;None;ed66bd548134140a6a2174cd6a054fdb;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;63;0.391075,682.8958;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;59;903.9595,-517.0769;Inherit;False;Property;_albedo_tint;albedo_tint;10;0;Create;True;0;0;False;0;False;0,0,0,0;0.745283,0.7154014,0.3269401,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;58;942.3525,-1043.327;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;73;533.0754,1266.695;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;744.6649,98.42548;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;49;-500.9569,-508.8226;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;507.8389,726.2844;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;66;64.40721,309.8977;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;42;143.6067,-62.39602;Inherit;False;Property;_emission;emission;18;0;Create;True;0;0;False;0;False;0,0,0,0;1,0.4301676,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;342.8018,1109.644;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;41;1838.96,517.7576;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;68;-666.7644,169.9317;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;39;1007.814,274.9669;Inherit;False;Constant;_Color0;Color 0;10;0;Create;True;0;0;False;0;False;0.7455336,1,0.3537736,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;989.157,62.22262;Inherit;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;33;1040.968,495.0761;Inherit;True;Property;_TextureSample1;Texture Sample 1;16;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;67;-460.0236,154.2222;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;1551.813,440.693;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;703.9744,-220.1252;Inherit;True;Property;_albedo;albedo;0;0;Create;True;0;0;False;0;False;None;f92e1a06f1a534c708a4366adb23316a;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;70;183.8905,855.2235;Inherit;True;Property;_Heightmap;Heightmap;15;0;Create;True;0;0;False;0;False;None;2bdbb83edf10d4f55ad87f3b754b36d5;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;1508.563,576.5839;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;69;474.584,826.2686;Inherit;True;Property;_TextureSample6;Texture Sample 6;19;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;37;1081.816,716.9438;Inherit;False;Property;_SickAmt;SickAmt;17;0;Create;True;0;0;False;0;False;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;237.0945,1369.383;Inherit;False;Property;_HeightAmt;HeightAmt;14;0;Create;True;0;0;False;0;False;0;2.97;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;1857.298,148.3786;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;1723.882,-223.0739;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;468.739,-99.15108;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Beehive2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;32;10;25;False;1;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;True;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;54;0;52;0
WireConnection;53;0;51;0
WireConnection;57;0;55;0
WireConnection;56;0;54;0
WireConnection;56;1;53;0
WireConnection;56;2;46;0
WireConnection;62;1;61;0
WireConnection;62;0;12;0
WireConnection;13;1;3;0
WireConnection;13;0;12;0
WireConnection;44;0;43;0
WireConnection;2;0;5;0
WireConnection;45;0;2;0
WireConnection;45;1;44;0
WireConnection;45;2;46;0
WireConnection;65;1;64;0
WireConnection;65;0;12;0
WireConnection;47;0;48;0
WireConnection;63;0;13;0
WireConnection;63;1;62;0
WireConnection;63;2;46;0
WireConnection;58;0;56;0
WireConnection;58;1;57;0
WireConnection;58;2;50;0
WireConnection;49;0;45;0
WireConnection;49;1;47;0
WireConnection;49;2;50;0
WireConnection;74;0;72;0
WireConnection;74;1;73;0
WireConnection;66;0;63;0
WireConnection;66;1;65;0
WireConnection;66;2;50;0
WireConnection;72;0;69;0
WireConnection;72;1;71;0
WireConnection;41;0;10;0
WireConnection;41;1;40;0
WireConnection;41;2;36;0
WireConnection;68;0;3;0
WireConnection;68;1;61;0
WireConnection;68;2;46;0
WireConnection;1;0;4;0
WireConnection;1;1;7;0
WireConnection;67;0;61;0
WireConnection;67;1;64;0
WireConnection;67;2;50;0
WireConnection;40;0;10;0
WireConnection;40;1;39;0
WireConnection;36;0;33;0
WireConnection;36;1;37;0
WireConnection;69;0;70;0
WireConnection;10;1;1;0
WireConnection;60;0;59;0
WireConnection;60;1;58;0
WireConnection;0;0;60;0
WireConnection;0;1;49;0
WireConnection;0;2;42;0
WireConnection;0;3;67;0
WireConnection;0;4;66;0
ASEEND*/
//CHKSM=09469B5273AD0026A08D0D09C4F1ACA8E41A6DE9