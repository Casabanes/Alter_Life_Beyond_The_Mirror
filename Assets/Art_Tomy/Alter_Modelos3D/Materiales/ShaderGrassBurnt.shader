// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ShaderGrassBurnt"
{
	Properties
	{
		_MaskGrassShader1("MaskGrassShader 1", 2D) = "white" {}
		_BaseGrass_lambert1_BaseColor("BaseGrass_lambert1_BaseColor", 2D) = "white" {}
		_Color2("Color 2", Color) = (1,0.8620698,0.2216981,0)
		_Color3("Color 3", Color) = (0.764151,0.6637183,0.3568441,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _BaseGrass_lambert1_BaseColor;
		uniform float4 _BaseGrass_lambert1_BaseColor_ST;
		uniform float4 _Color2;
		uniform float4 _Color3;
		uniform sampler2D _MaskGrassShader1;
		uniform float4 _MaskGrassShader1_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BaseGrass_lambert1_BaseColor = i.uv_texcoord * _BaseGrass_lambert1_BaseColor_ST.xy + _BaseGrass_lambert1_BaseColor_ST.zw;
			float4 color5 = IsGammaSpace() ? float4(0.4901961,0.5960785,0.4117647,1) : float4(0.2050788,0.3139888,0.1412633,1);
			float4 color6 = IsGammaSpace() ? float4(0.7960785,0.8862746,0.6666667,1) : float4(0.597202,0.7605247,0.4019779,1);
			o.Albedo = (_Color2 + (tex2D( _BaseGrass_lambert1_BaseColor, uv_BaseGrass_lambert1_BaseColor ) - color5) * (_Color3 - _Color2) / (color6 - color5)).rgb;
			float2 uv_MaskGrassShader1 = i.uv_texcoord * _MaskGrassShader1_ST.xy + _MaskGrassShader1_ST.zw;
			o.Alpha = tex2D( _MaskGrassShader1, uv_MaskGrassShader1 ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
7;18;1352;689;1420.361;134.9438;1.652088;True;False
Node;AmplifyShaderEditor.SamplerNode;3;-612.261,-138.9231;Inherit;True;Property;_BaseGrass_lambert1_BaseColor;BaseGrass_lambert1_BaseColor;1;0;Create;True;0;0;False;0;False;-1;387e40ce0d908b94da740418a6b3f8ab;387e40ce0d908b94da740418a6b3f8ab;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;-635.5521,74.58749;Inherit;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;False;0.4901961,0.5960785,0.4117647,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;6;-622.5521,272.5875;Inherit;False;Constant;_Color1;Color 1;2;0;Create;True;0;0;False;0;False;0.7960785,0.8862746,0.6666667,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;8;-641.9562,788.617;Inherit;False;Property;_Color3;Color 3;3;0;Create;True;0;0;False;0;False;0.764151,0.6637183,0.3568441,0;0.764151,0.6637183,0.3568441,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;7;-618.7822,463.4554;Inherit;False;Property;_Color2;Color 2;2;0;Create;True;0;0;False;0;False;1,0.8620698,0.2216981,0;1,0.8620698,0.2216981,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-343.1525,245.6487;Inherit;True;Property;_MaskGrassShader1;MaskGrassShader 1;0;0;Create;True;0;0;False;0;False;-1;54a44123a2113574c9a2824ae0858b79;54a44123a2113574c9a2824ae0858b79;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;4;-281.5521,28.58746;Inherit;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0.4901961,0.5960785,0.4117647,1;False;2;COLOR;0.7960785,0.8862746,0.6666667,1;False;3;COLOR;0.8018868,0.671337,0.2307316,0;False;4;COLOR;1,0.8676101,0.5518868,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;26,5;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ShaderGrassBurnt;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;3;0
WireConnection;4;1;5;0
WireConnection;4;2;6;0
WireConnection;4;3;7;0
WireConnection;4;4;8;0
WireConnection;0;0;4;0
WireConnection;0;9;2;0
ASEEND*/
//CHKSM=F008225EFE296D62AEEA68B3B81F6E46FC72ADEE