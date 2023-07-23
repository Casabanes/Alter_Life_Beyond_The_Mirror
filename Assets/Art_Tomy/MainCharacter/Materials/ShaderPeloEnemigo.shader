// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ShaderAgua"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 4.4
		_VelocidadOlas("VelocidadOlas", Float) = 0.78
		_Frecuencia("Frecuencia", Float) = 2.5
		_LimiteOlas("LimiteOlas", Float) = 0
		_IntensidadLimite("IntensidadLimite", Float) = 0
		_Float0("Float 0", Float) = 0
		_IntensidadOlas("IntensidadOlas", Float) = 0.85
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Frecuencia;
		uniform float _VelocidadOlas;
		uniform float _IntensidadOlas;
		uniform float _LimiteOlas;
		uniform float _IntensidadLimite;
		uniform float _Float0;
		uniform float _EdgeLength;


		struct Gradient
		{
			int type;
			int colorsLength;
			int alphasLength;
			float4 colors[8];
			float2 alphas[8];
		};


		Gradient NewGradient(int type, int colorsLength, int alphasLength, 
		float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
		float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
		{
			Gradient g;
			g.type = type;
			g.colorsLength = colorsLength;
			g.alphasLength = alphasLength;
			g.colors[ 0 ] = colors0;
			g.colors[ 1 ] = colors1;
			g.colors[ 2 ] = colors2;
			g.colors[ 3 ] = colors3;
			g.colors[ 4 ] = colors4;
			g.colors[ 5 ] = colors5;
			g.colors[ 6 ] = colors6;
			g.colors[ 7 ] = colors7;
			g.alphas[ 0 ] = alphas0;
			g.alphas[ 1 ] = alphas1;
			g.alphas[ 2 ] = alphas2;
			g.alphas[ 3 ] = alphas3;
			g.alphas[ 4 ] = alphas4;
			g.alphas[ 5 ] = alphas5;
			g.alphas[ 6 ] = alphas6;
			g.alphas[ 7 ] = alphas7;
			return g;
		}


		float4 SampleGradient( Gradient gradient, float time )
		{
			float3 color = gradient.colors[0].rgb;
			UNITY_UNROLL
			for (int c = 1; c < 8; c++)
			{
			float colorPos = saturate((time - gradient.colors[c-1].w) / (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1);
			color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
			}
			#ifndef UNITY_COLORSPACE_GAMMA
			color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
			#endif
			float alpha = gradient.alphas[0].x;
			UNITY_UNROLL
			for (int a = 1; a < 8; a++)
			{
			float alphaPos = saturate((time - gradient.alphas[a-1].y) / (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1);
			alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
			}
			return float4(color, alpha);
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertex3Pos = v.vertex.xyz;
			float mulTime41 = _Time.y * _VelocidadOlas;
			float myVarName57 = ( 0.41 - ase_vertex3Pos.z );
			float4 appendResult50 = (float4(0.0 , ( ( cos( ( ( ase_vertex3Pos.z * _Frecuencia ) + mulTime41 ) ) * _IntensidadOlas ) * saturate( ( ( -myVarName57 + _LimiteOlas ) * _IntensidadLimite ) ) * _Float0 ) , 0.0 , 0.0));
			v.vertex.xyz += appendResult50.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float temp_output_92_0 = ( 0.66 - i.uv_texcoord.y );
			Gradient gradient81 = NewGradient( 0, 5, 2, float4( 0.4339623, 0.4339623, 0.4339623, 0 ), float4( 1, 0.1450828, 0, 0.1617609 ), float4( 1, 0.4976526, 0, 0.4705882 ), float4( 1, 0.983255, 0.615566, 0.7000076 ), float4( 1, 1, 0.6367924, 1 ), 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			o.Albedo = ( ( 1.0 - temp_output_92_0 ) * SampleGradient( gradient81, i.uv_texcoord.y ) ).rgb;
			o.Emission = ( temp_output_92_0 * SampleGradient( gradient81, i.uv_texcoord.y ) ).rgb;
			o.Alpha = ( ( i.uv_texcoord.y - 0.09 ) * 2.26 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
				vertexDataFunc( v );
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
785;61;1352;689;1536.974;1055.064;1.3;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;7;-1726.576,-63.9169;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;73;-1641.102,-209.7371;Inherit;False;Constant;_Float1;Float 1;8;0;Create;True;0;0;False;0;False;0.41;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;72;-1415.464,-203.0285;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;57;-1162.22,-1.064465;Inherit;False;myVarName;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1419.704,378.6992;Inherit;True;Property;_Frecuencia;Frecuencia;6;0;Create;True;0;0;False;0;False;2.5;2.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-1197.306,383.5578;Inherit;False;Property;_VelocidadOlas;VelocidadOlas;5;0;Create;True;0;0;False;0;False;0.78;-2.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;-1525.727,728.9795;Inherit;False;57;myVarName;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-1162.05,148.0653;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;41;-1022.541,405.275;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-1381.144,847.5104;Inherit;False;Property;_LimiteOlas;LimiteOlas;7;0;Create;True;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;61;-1356.669,731.8055;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-875.9075,305.8452;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-1191.532,991.0937;Inherit;False;Property;_IntensidadLimite;IntensidadLimite;8;0;Create;True;0;0;False;0;False;0;-0.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;64;-1213.144,785.5104;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;43;-761.5918,279.5642;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;93;-930.8818,-906.1808;Inherit;False;Constant;_Float4;Float 4;8;0;Create;True;0;0;False;0;False;0.66;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-1034.857,648.1498;Inherit;False;Property;_IntensidadOlas;IntensidadOlas;10;0;Create;True;0;0;False;0;False;0.85;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;76;-1201.826,-678.1531;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-1053.732,812.9937;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;81;-1194.868,-748.8966;Inherit;False;0;5;2;0.4339623,0.4339623,0.4339623,0;1,0.1450828,0,0.1617609;1,0.4976526,0,0.4705882;1,0.983255,0.615566,0.7000076;1,1,0.6367924,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;92;-765.0411,-872.5489;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-923.1898,-323.2938;Inherit;True;Constant;_Float2;Float 2;8;0;Create;True;0;0;False;0;False;0.09;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-804.7156,573.6278;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;68;-900.64,805.0748;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-957.9312,992.7378;Inherit;False;Property;_Float0;Float 0;9;0;Create;True;0;0;False;0;False;0;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;74;-1566.136,67.11363;Inherit;False;285;304;Para shader oleaje;1;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;94;-504.7746,-674.8142;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;84;-727.5866,-367.0856;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-629.3536,-94.37354;Inherit;False;Constant;_Float3;Float 3;8;0;Create;True;0;0;False;0;False;2.26;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;80;-846.6649,-650.4808;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-807.1077,896.7205;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-326.6744,-611.1141;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector3Node;6;-1727.576,91.08311;Inherit;False;Property;_Vector0;Vector 0;11;0;Create;True;0;0;False;0;False;0,0,-0.92;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;50;-425.8767,574.1561;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DistanceOpNode;1;-1516.136,117.1136;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;-470.4713,-383.1451;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;-469.424,-857.1345;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;71;-33.60891,-707.4883;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;ShaderAgua;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;4.4;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;72;0;73;0
WireConnection;72;1;7;3
WireConnection;57;0;72;0
WireConnection;38;0;7;3
WireConnection;38;1;39;0
WireConnection;41;0;42;0
WireConnection;61;0;58;0
WireConnection;40;0;38;0
WireConnection;40;1;41;0
WireConnection;64;0;61;0
WireConnection;64;1;65;0
WireConnection;43;0;40;0
WireConnection;66;0;64;0
WireConnection;66;1;67;0
WireConnection;92;0;93;0
WireConnection;92;1;76;2
WireConnection;46;0;43;0
WireConnection;46;1;47;0
WireConnection;68;0;66;0
WireConnection;94;0;92;0
WireConnection;84;0;76;2
WireConnection;84;1;85;0
WireConnection;80;0;81;0
WireConnection;80;1;76;2
WireConnection;53;0;46;0
WireConnection;53;1;68;0
WireConnection;53;2;59;0
WireConnection;95;0;94;0
WireConnection;95;1;80;0
WireConnection;50;1;53;0
WireConnection;1;0;7;0
WireConnection;1;1;6;0
WireConnection;86;0;84;0
WireConnection;86;1;87;0
WireConnection;90;0;92;0
WireConnection;90;1;80;0
WireConnection;71;0;95;0
WireConnection;71;2;90;0
WireConnection;71;9;86;0
WireConnection;71;11;50;0
ASEEND*/
//CHKSM=ADCAD3FEEA153508A838BF19D7D66B699616470E