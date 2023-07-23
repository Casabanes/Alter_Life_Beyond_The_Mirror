// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Aura"
{
	Properties
	{
		_Ciclos("Ciclos", Float) = 9.16
		_Velocidadciclo("Velocidad ciclo", Float) = 5
		_Offsetcolor("Offset color", Float) = 1
		_Lineas("Lineas", Float) = 36
		_Distrosa("Dist rosa", Float) = 3.52
		_Anchozonamedia("Ancho zona media", Float) = -0.23
		_Posicionlineasazules("Posicion lineas azules", Float) = -0.1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Ciclos;
		uniform float _Velocidadciclo;
		uniform float _Distrosa;
		uniform float _Offsetcolor;
		uniform float _Posicionlineasazules;
		uniform float _Anchozonamedia;
		uniform float _Lineas;


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
			float colorPos = saturate((time - gradient.colors[c-1].w) / ( 0.00001 + (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1));
			color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
			}
			#ifndef UNITY_COLORSPACE_GAMMA
			color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
			#endif
			float alpha = gradient.alphas[0].x;
			UNITY_UNROLL
			for (int a = 1; a < 8; a++)
			{
			float alphaPos = saturate((time - gradient.alphas[a-1].y) / ( 0.00001 + (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1));
			alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
			}
			return float4(color, alpha);
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float mulTime161 = _Time.y * _Velocidadciclo;
			float4 appendResult14 = (float4(0.0 , 0.0 , cos( ( ( ase_vertex3Pos * _Ciclos * 6.28318548202515 ) + mulTime161 ) ).xy));
			float4 Movimiento195 = appendResult14;
			v.vertex.xyz += Movimiento195.xyz;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color226 = IsGammaSpace() ? float4(0,0.6910961,1,0) : float4(0,0.4354132,1,0);
			Gradient gradient216 = NewGradient( 0, 3, 2, float4( 1, 0.6076495, 0, 0 ), float4( 1, 0, 0.7332482, 0.5029374 ), float4( 1, 0.5650071, 0.0514564, 1 ), 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float4 Parteinteriorgradiente262 = saturate( SampleGradient( gradient216, ( ( i.uv_texcoord.x * _Distrosa ) + _Offsetcolor ) ) );
			float temp_output_248_0 = ( i.uv_texcoord.y + _Posicionlineasazules );
			float AlphaLineas258 = saturate( ( temp_output_248_0 * ( ( 1.0 - temp_output_248_0 ) + _Anchozonamedia ) * 50.0 ) );
			float4 lerpResult284 = lerp( color226 , Parteinteriorgradiente262 , AlphaLineas258);
			o.Emission = lerpResult284.rgb;
			float CiclosLineas196 = saturate( cos( ( i.uv_texcoord.y * _Lineas * 6.28318548202515 ) ) );
			o.Alpha = CiclosLineas196;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

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
				vertexDataFunc( v, customInputData );
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
Version=18912
0;0;1366;707;3265.108;1623.083;2.274608;True;False
Node;AmplifyShaderEditor.CommentaryNode;286;-2552.75,-1077.079;Inherit;False;1678.729;509.8897;Borde aura;11;253;254;249;248;275;247;252;256;285;258;222;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;222;-2502.75,-1027.079;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;247;-2373.646,-904.7903;Inherit;False;Property;_Posicionlineasazules;Posicion lineas azules;6;0;Create;True;0;0;0;False;0;False;-0.1;-0.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;275;-2277.87,-1026.479;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.CommentaryNode;266;-2547.789,-1458.87;Inherit;False;1300.55;348.0381;Zona interior aura;9;217;215;214;204;213;261;216;211;262;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;248;-2128.286,-1001.768;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;199;-2553.887,-539.8542;Inherit;False;1208.504;412.292;Movimiento vertex offset;10;16;160;161;13;162;8;14;195;288;289;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;261;-2497.789,-1392.618;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;213;-2437.248,-1276.864;Inherit;False;Property;_Distrosa;Dist rosa;4;0;Create;True;0;0;0;False;0;False;3.52;1.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;160;-2500.22,-211.8564;Inherit;False;Property;_Velocidadciclo;Velocidad ciclo;1;0;Create;True;0;0;0;False;0;False;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;253;-1931.513,-839.491;Inherit;False;Property;_Anchozonamedia;Ancho zona media;5;0;Create;True;0;0;0;False;0;False;-0.23;-0.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;214;-2260.328,-1316.361;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;200;-1329.869,-537.5391;Inherit;False;992.8527;409.5307;Lineas con opacity;7;196;174;168;165;166;171;287;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-2491.172,-349.4921;Inherit;False;Property;_Ciclos;Ciclos;0;0;Create;True;0;0;0;False;0;False;9.16;0.075;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;252;-1877.601,-906.092;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TauNode;288;-2424.932,-278.4526;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;289;-2510.985,-487.0109;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;204;-2260.259,-1225.832;Inherit;False;Property;_Offsetcolor;Offset color;2;0;Create;True;0;0;0;False;0;False;1;-0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;161;-2316.123,-206.4873;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;171;-1279.869,-487.5392;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GradientNode;216;-2185.21,-1405.582;Inherit;False;0;3;2;1,0.6076495,0,0;1,0,0.7332482,0.5029374;1,0.5650071,0.0514564,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.TauNode;287;-1157.207,-305.178;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;166;-1194.821,-375.7371;Inherit;False;Property;_Lineas;Lineas;3;0;Create;True;0;0;0;False;0;False;36;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-2293.984,-387.7517;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;215;-2115.328,-1316.361;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;254;-1703.871,-910.1176;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;256;-1710.606,-697.7112;Inherit;False;Constant;_Edgelineasazules;Edge lineas azules;4;0;Create;True;0;0;0;False;0;False;50;50;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;162;-2135.981,-380.5627;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;249;-1460.039,-1001.731;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;217;-1964.959,-1404.682;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;165;-1016.94,-430.6943;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;168;-885.5155,-431.5777;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;8;-1939.46,-381.6887;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;285;-1252.667,-1001.633;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;211;-1664.938,-1404.708;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;174;-722.1513,-430.1518;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;14;-1811.58,-382.3166;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT2;0,0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;262;-1524.235,-1408.557;Inherit;False;Parteinteriorgradiente;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;258;-1117.02,-1008.178;Inherit;False;AlphaLineas;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;263;-1197.932,-1289.979;Inherit;False;262;Parteinteriorgradiente;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;195;-1588.383,-380.2886;Inherit;False;Movimiento;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;196;-578.3324,-433.6436;Inherit;False;CiclosLineas;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;226;-1197.489,-1458.556;Inherit;False;Constant;_Colorborde;Color borde;4;0;Create;True;0;0;0;False;0;False;0,0.6910961,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;260;-1194.421,-1211.045;Inherit;False;258;AlphaLineas;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;284;-851.9995,-1277.805;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;198;-815.3494,-1059.336;Inherit;False;196;CiclosLineas;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;197;-812.7251,-988.3422;Inherit;False;195;Movimiento;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-608.2639,-1260.146;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Aura;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;5;True;True;0;False;Transparent;;Transparent;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;275;0;222;0
WireConnection;248;0;275;1
WireConnection;248;1;247;0
WireConnection;214;0;261;1
WireConnection;214;1;213;0
WireConnection;252;0;248;0
WireConnection;161;0;160;0
WireConnection;13;0;289;0
WireConnection;13;1;16;0
WireConnection;13;2;288;0
WireConnection;215;0;214;0
WireConnection;215;1;204;0
WireConnection;254;0;252;0
WireConnection;254;1;253;0
WireConnection;162;0;13;0
WireConnection;162;1;161;0
WireConnection;249;0;248;0
WireConnection;249;1;254;0
WireConnection;249;2;256;0
WireConnection;217;0;216;0
WireConnection;217;1;215;0
WireConnection;165;0;171;2
WireConnection;165;1;166;0
WireConnection;165;2;287;0
WireConnection;168;0;165;0
WireConnection;8;0;162;0
WireConnection;285;0;249;0
WireConnection;211;0;217;0
WireConnection;174;0;168;0
WireConnection;14;2;8;0
WireConnection;262;0;211;0
WireConnection;258;0;285;0
WireConnection;195;0;14;0
WireConnection;196;0;174;0
WireConnection;284;0;226;0
WireConnection;284;1;263;0
WireConnection;284;2;260;0
WireConnection;0;2;284;0
WireConnection;0;9;198;0
WireConnection;0;11;197;0
ASEEND*/
//CHKSM=60F521D1DDF3D49841E4D07656EEC31B0C9D7052