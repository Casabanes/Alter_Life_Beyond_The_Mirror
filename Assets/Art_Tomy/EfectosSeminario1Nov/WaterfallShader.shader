// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WaterfallShader"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
		_Cracktext("Crack text", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Velocidadpanneo("Velocidad panneo", Float) = 0.15
		_Distanciabrillo("Distancia brillo", Vector) = (0.5,0.5,0,0)
		_Colorbrillo("Color brillo", Color) = (0.4245283,0.4225258,0.4225258,0)
		_StepBrillo("Step Brillo", Float) = 0.5
		_Substractbrillo("Substract brillo", Float) = 2.93
		_Float2("Velocidad rotation", Float) = 0.2
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Distance("Distance", Vector) = (0.5,0.5,0,0)
		_Coloraguaarriba("Color agua arriba", Color) = (0.1826273,0.4412826,0.6792453,1)
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_Scalenoisefoam("Scale noise foam", Float) = 2.96
		_Stepfoam("Step foam", Float) = 0.45
		_Colorfoam("Color foam", Color) = (0.8820755,1,0.968103,0)
		_TilingUVAgua("Tiling UV Agua", Vector) = (0.2,0.2,0,0)
		_Flowmapaguaintensidad("Flowmap agua intensidad", Float) = 0.025
		_Tilingbordeybrillo("Tiling borde y brillo", Vector) = (1,1,0,0)
		_Bordefoam("Borde foam", Float) = 0.81
		_Flowmapfoam("Flowmap foam", Float) = 1
		_Coloraguafondo("Color agua fondo", Color) = (0.1066661,0.3028149,0.4811321,0)
		_Intensidadfoam("Intensidad foam", Float) = 3.24
		_Intensidadpartenegrafoam("Intensidad parte negra foam", Float) = 6.98
		_Partenegrafoam("Parte negra foam", Float) = 1.77
		_Intensidadtextfoam("Intensidad text foam", Float) = 1
		_Scaletextfoam("Scale text foam", Float) = -1.54
		_Panningtextfoam("Panning text foam", Vector) = (0,1,0,0)
		_Velocidadtextfoam("Velocidad text foam", Float) = 0.25
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Unlit alpha:fade keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform float _Velocidadpanneo;
		uniform float _Flowmapaguaintensidad;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float2 _TilingUVAgua;
		uniform float4 _Coloraguaarriba;
		uniform float4 _Coloraguafondo;
		uniform sampler2D _Cracktext;
		uniform float2 _Tilingbordeybrillo;
		uniform float2 _Distanciabrillo;
		uniform float _StepBrillo;
		uniform float _Substractbrillo;
		uniform float4 _Colorbrillo;
		uniform float4 _Colorfoam;
		uniform float _Flowmapfoam;
		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;
		uniform float _Float2;
		uniform float _Scalenoisefoam;
		uniform float _Stepfoam;
		uniform float _Intensidadpartenegrafoam;
		uniform float2 _Distance;
		uniform float _Partenegrafoam;
		uniform float _Intensidadfoam;
		uniform float _Bordefoam;
		uniform float _Velocidadtextfoam;
		uniform float2 _Panningtextfoam;
		uniform float _Scaletextfoam;
		uniform float _Intensidadtextfoam;
		uniform float _EdgeLength;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float mulTime164 = _Time.y * _Velocidadpanneo;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float2 TilingAgua379 = _TilingUVAgua;
			float2 uv_TexCoord321 = i.uv_texcoord * TilingAgua379;
			float2 panner184 = ( sin( mulTime164 ) * float2( 0.01,0.01 ) + ( ( _Flowmapaguaintensidad * (tex2D( _TextureSample1, uv_TextureSample1 )).rg ) + uv_TexCoord321 ));
			float2 uv_TexCoord166 = i.uv_texcoord * TilingAgua379;
			float4 lerpResult120 = lerp( _Coloraguaarriba , _Coloraguafondo , tex2D( _Cracktext, uv_TexCoord166 ));
			float4 Lineasymovimiento254 = ( tex2D( _TextureSample0, panner184 ) + saturate( lerpResult120 ) );
			float2 TilingFoamYBrillo375 = _Tilingbordeybrillo;
			float2 uv_TexCoord211 = i.uv_texcoord * TilingFoamYBrillo375;
			float4 Brillo252 = ( saturate( ( ( distance( uv_TexCoord211 , _Distanciabrillo ) * _StepBrillo ) - _Substractbrillo ) ) * _Colorbrillo );
			float2 uv_TexCoord273 = i.uv_texcoord * TilingFoamYBrillo375;
			float2 uv_TextureSample3 = i.uv_texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
			float mulTime269 = _Time.y * _Float2;
			float cos266 = cos( mulTime269 );
			float sin266 = sin( mulTime269 );
			float2 rotator266 = mul( ( float4( uv_TexCoord273, 0.0 , 0.0 ) + ( _Flowmapfoam * tex2D( _TextureSample3, uv_TextureSample3 ) ) ).rg - float2( 0.5,0.5 ) , float2x2( cos266 , -sin266 , sin266 , cos266 )) + float2( 0.5,0.5 );
			float simplePerlin2D283 = snoise( rotator266*_Scalenoisefoam );
			simplePerlin2D283 = simplePerlin2D283*0.5 + 0.5;
			float2 uv_TexCoord340 = i.uv_texcoord * TilingFoamYBrillo375;
			float IntEspuma16_g7 = _Intensidadfoam;
			float2 uv_TexCoord353 = i.uv_texcoord * TilingFoamYBrillo375;
			float temp_output_10_0_g7 = uv_TexCoord353.x;
			float CantEspuma12_g7 = _Bordefoam;
			float IntEspuma16_g6 = _Intensidadfoam;
			float temp_output_10_0_g6 = uv_TexCoord353.y;
			float CantEspuma12_g6 = _Bordefoam;
			float mulTime389 = _Time.y * _Velocidadtextfoam;
			float2 panner386 = ( sin( mulTime389 ) * _Panningtextfoam + i.uv_texcoord);
			float simplePerlin2D381 = snoise( panner386*_Scaletextfoam );
			simplePerlin2D381 = simplePerlin2D381*0.5 + 0.5;
			float Foam350 = saturate( ( saturate( ( ( step( simplePerlin2D283 , _Stepfoam ) * ( ( _Intensidadpartenegrafoam * distance( uv_TexCoord340 , _Distance ) ) - _Partenegrafoam ) ) + ( ( ( IntEspuma16_g7 * ( ( 1.0 - temp_output_10_0_g7 ) - CantEspuma12_g7 ) ) * ( IntEspuma16_g7 * ( CantEspuma12_g7 - temp_output_10_0_g7 ) ) ) + ( ( IntEspuma16_g6 * ( ( 1.0 - temp_output_10_0_g6 ) - CantEspuma12_g6 ) ) * ( IntEspuma16_g6 * ( CantEspuma12_g6 - temp_output_10_0_g6 ) ) ) ) ) ) * ( simplePerlin2D381 * _Intensidadtextfoam ) ) );
			float4 lerpResult300 = lerp( ( Lineasymovimiento254 + Brillo252 ) , _Colorfoam , Foam350);
			o.Emission = saturate( lerpResult300 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
262;181;1352;561;195.2931;-1520.063;1.109741;True;False
Node;AmplifyShaderEditor.CommentaryNode;373;-512.8767,729.8854;Inherit;False;2472.053;1513.694;Foam;40;336;337;335;273;268;269;340;334;342;267;372;290;341;345;266;343;353;292;283;367;368;347;346;291;348;371;315;316;374;375;376;381;383;384;386;389;392;393;388;395;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;372;-292.3432,1460.094;Inherit;False;Property;_Tilingbordeybrillo;Tiling borde y brillo;22;0;Create;True;0;0;False;0;False;1,1;0.55,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;375;-104.4811,1460.573;Inherit;False;TilingFoamYBrillo;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;374;-430.7768,801.0193;Inherit;False;375;TilingFoamYBrillo;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;337;-462.8767,970.1703;Inherit;True;Property;_TextureSample3;Texture Sample 3;16;0;Create;True;0;0;False;0;False;-1;5644c9d607227794bb3a801d33c12903;5644c9d607227794bb3a801d33c12903;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;336;-348.1659,896.1922;Inherit;False;Property;_Flowmapfoam;Flowmap foam;24;0;Create;True;0;0;False;0;False;1;0.94;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;349;-513.4556,-290.5973;Inherit;False;2045.965;1004.162;Comment;23;165;164;163;121;140;184;120;191;138;143;254;166;188;185;327;331;321;325;323;326;318;379;380;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;335;-156.5373,903.9063;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;268;-136.8764,1169.344;Inherit;False;Property;_Float2;Velocidad rotation;12;0;Create;True;0;0;False;0;False;0.2;-0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;273;-233.1421,779.8854;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;342;142.6783,1575.03;Inherit;False;Property;_Distance;Distance;14;0;Create;True;0;0;False;0;False;0.5,0.5;0.27,0.25;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;269;54.47426,1177.762;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;334;6.210983,833.2043;Inherit;True;2;2;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;340;91.38038,1442.345;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;331;-427.2747,124.9516;Inherit;False;Property;_TilingUVAgua;Tiling UV Agua;20;0;Create;True;0;0;False;0;False;0.2,0.2;0.6,0.6;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;258;-513.0373,-780.3854;Inherit;False;1589.364;454.2306;Comment;12;244;214;207;211;210;249;250;248;231;242;252;378;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;267;52.09298,1043.196;Inherit;False;Constant;_Vector1;Vector 1;9;0;Create;True;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;378;-492.7661,-711.0022;Inherit;False;375;TilingFoamYBrillo;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;345;212.6822,1361.595;Inherit;False;Property;_Intensidadpartenegrafoam;Intensidad parte negra foam;27;0;Create;True;0;0;False;0;False;6.98;11.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;318;-463.4556,-161.3326;Inherit;True;Property;_TextureSample1;Texture Sample 1;13;0;Create;True;0;0;False;0;False;-1;835adb7187474cd4c86042d52ae6a56d;835adb7187474cd4c86042d52ae6a56d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;341;337.0648,1505.884;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;290;357.2804,1214.842;Inherit;False;Property;_Scalenoisefoam;Scale noise foam;17;0;Create;True;0;0;False;0;False;2.96;23.45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;266;257.3002,997.5093;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;376;109.0606,1841.872;Inherit;False;375;TilingFoamYBrillo;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;388;1035.518,2014.318;Inherit;False;Property;_Velocidadtextfoam;Velocidad text foam;32;0;Create;True;0;0;False;0;False;0.25;-0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;379;-225.6858,122.1208;Inherit;False;TilingAgua;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;353;306.9333,1825.456;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;326;-194.9034,-238.5973;Inherit;False;Property;_Flowmapaguaintensidad;Flowmap agua intensidad;21;0;Create;True;0;0;False;0;False;0.025;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;389;1233.815,2018.358;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;292;632.2801,1271.842;Inherit;False;Property;_Stepfoam;Step foam;18;0;Create;True;0;0;False;0;False;0.45;0.63;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;347;624.3763,1647.794;Inherit;False;Property;_Partenegrafoam;Parte negra foam;28;0;Create;True;0;0;False;0;False;1.77;2.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;283;539.0805,1056.707;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;368;365.2903,2039.215;Inherit;False;Property;_Intensidadfoam;Intensidad foam;26;0;Create;True;0;0;False;0;False;3.24;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;367;366.0338,1956.331;Inherit;False;Property;_Bordefoam;Borde foam;23;0;Create;True;0;0;False;0;False;0.81;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;380;-111.3672,502.1907;Inherit;False;379;TilingAgua;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;323;-175.3874,-165.45;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;211;-294.3805,-730.3855;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;165;1.13836,30.33318;Inherit;False;Property;_Velocidadpanneo;Velocidad panneo;7;0;Create;True;0;0;False;0;False;0.15;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;210;-234.4888,-602.0685;Inherit;False;Property;_Distanciabrillo;Distancia brillo;8;0;Create;True;0;0;False;0;False;0.5,0.5;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;343;543.3438,1399.45;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;393;1387.818,1915.658;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;321;3.03006,-91.81061;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;164;192.555,38.67619;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;291;832.2802,1089.842;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;397;620.7487,1989.579;Inherit;True;FunctionBordes;-1;;6;1c664b9ff265ee24793fb26eaf19ab3b;0;3;10;FLOAT;0;False;11;FLOAT;0;False;15;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;392;1163.521,1893.279;Inherit;False;Property;_Panningtextfoam;Panning text foam;31;0;Create;True;0;0;False;0;False;0,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleSubtractOpNode;346;831.225,1385.167;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;325;84.53116,-187.8265;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;384;1285.118,1772.658;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;398;620.6844,1769.444;Inherit;True;FunctionBordes;-1;;7;1c664b9ff265ee24793fb26eaf19ab3b;0;3;10;FLOAT;0;False;11;FLOAT;0;False;15;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;214;-12.57469,-445.6833;Inherit;False;Property;_StepBrillo;Step Brillo;10;0;Create;True;0;0;False;0;False;0.5;-1.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;207;-65.64897,-666.3101;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;166;49.31983,483.8933;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;348;1074.849,1329.206;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;163;356.5099,37.45579;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;140;269.2718,302.0157;Inherit;False;Property;_Coloraguafondo;Color agua fondo;25;0;Create;True;0;0;False;0;False;0.1066661,0.3028149,0.4811321,0;0.4639107,0.5906016,0.7075472,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;121;268.4898,132.1444;Inherit;False;Property;_Coloraguaarriba;Color agua arriba;15;0;Create;True;0;0;False;0;False;0.1826273,0.4412826,0.6792453,1;0.3152367,0.6047398,0.8679245,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;327;341.8291,-186.3341;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;244;184.4253,-698.1544;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;386;1513.918,1771.358;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;185;295.703,-94.00028;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;False;0.01,0.01;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;188;268.6616,483.5649;Inherit;True;Property;_Cracktext;Crack text;5;0;Create;True;0;0;False;0;False;-1;3c994547c26c03d4086797a10f8d567b;3c994547c26c03d4086797a10f8d567b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;249;216.0699,-471.0206;Inherit;False;Property;_Substractbrillo;Substract brillo;11;0;Create;True;0;0;False;0;False;2.93;-0.59;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;371;1083.314,1599.508;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;383;1514.289,1889.937;Inherit;False;Property;_Scaletextfoam;Scale text foam;30;0;Create;True;0;0;False;0;False;-1.54;-1.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;120;618.0002,229.4945;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;248;435.7956,-636.3456;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;315;1334.067,1506.318;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;381;1688.118,1771.358;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;395;1733.639,2020.364;Inherit;False;Property;_Intensidadtextfoam;Intensidad text foam;29;0;Create;True;0;0;False;0;False;1;1.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;184;514.057,-55.52193;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;250;569.8277,-633.4038;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;191;762.8187,-8.04143;Inherit;True;Property;_TextureSample0;Texture Sample 0;6;0;Create;True;0;0;False;0;False;-1;3c994547c26c03d4086797a10f8d567b;3c994547c26c03d4086797a10f8d567b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;394;1936.579,1903.782;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;138;879.4868,235.3975;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;316;1574.882,1541.014;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;231;510.0956,-538.1548;Inherit;False;Property;_Colorbrillo;Color brillo;9;0;Create;True;0;0;False;0;False;0.4245283,0.4225258,0.4225258,0;0.745283,0.745283,0.745283,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;143;1079.069,143.9078;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;385;2026.629,1712.997;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;242;729.1862,-601.5374;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;252;852.3262,-602.8586;Inherit;False;Brillo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;396;2257.919,1754.935;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;254;1283.509,164.755;Inherit;False;Lineasymovimiento;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;350;2459.907,1669.295;Inherit;True;Foam;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;257;2072.929,525.8467;Inherit;False;254;Lineasymovimiento;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;253;2126.86,601.5487;Inherit;False;252;Brillo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;240;2299.277,556.3097;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;297;2220.01,672.723;Inherit;False;Property;_Colorfoam;Color foam;19;0;Create;True;0;0;False;0;False;0.8820755,1,0.968103,0;1,0.04039566,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;351;2260.017,846.133;Inherit;False;350;Foam;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;300;2442.449,695.798;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;241;2591.08,694.87;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2739.527,620.117;Float;False;True;-1;6;ASEMaterialInspector;0;0;Unlit;WaterfallShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;375;0;372;0
WireConnection;335;0;336;0
WireConnection;335;1;337;0
WireConnection;273;0;374;0
WireConnection;269;0;268;0
WireConnection;334;0;273;0
WireConnection;334;1;335;0
WireConnection;340;0;375;0
WireConnection;341;0;340;0
WireConnection;341;1;342;0
WireConnection;266;0;334;0
WireConnection;266;1;267;0
WireConnection;266;2;269;0
WireConnection;379;0;331;0
WireConnection;353;0;376;0
WireConnection;389;0;388;0
WireConnection;283;0;266;0
WireConnection;283;1;290;0
WireConnection;323;0;318;0
WireConnection;211;0;378;0
WireConnection;343;0;345;0
WireConnection;343;1;341;0
WireConnection;393;0;389;0
WireConnection;321;0;379;0
WireConnection;164;0;165;0
WireConnection;291;0;283;0
WireConnection;291;1;292;0
WireConnection;397;10;353;2
WireConnection;397;11;367;0
WireConnection;397;15;368;0
WireConnection;346;0;343;0
WireConnection;346;1;347;0
WireConnection;325;0;326;0
WireConnection;325;1;323;0
WireConnection;398;10;353;1
WireConnection;398;11;367;0
WireConnection;398;15;368;0
WireConnection;207;0;211;0
WireConnection;207;1;210;0
WireConnection;166;0;380;0
WireConnection;348;0;291;0
WireConnection;348;1;346;0
WireConnection;163;0;164;0
WireConnection;327;0;325;0
WireConnection;327;1;321;0
WireConnection;244;0;207;0
WireConnection;244;1;214;0
WireConnection;386;0;384;0
WireConnection;386;2;392;0
WireConnection;386;1;393;0
WireConnection;188;1;166;0
WireConnection;371;0;398;0
WireConnection;371;1;397;0
WireConnection;120;0;121;0
WireConnection;120;1;140;0
WireConnection;120;2;188;0
WireConnection;248;0;244;0
WireConnection;248;1;249;0
WireConnection;315;0;348;0
WireConnection;315;1;371;0
WireConnection;381;0;386;0
WireConnection;381;1;383;0
WireConnection;184;0;327;0
WireConnection;184;2;185;0
WireConnection;184;1;163;0
WireConnection;250;0;248;0
WireConnection;191;1;184;0
WireConnection;394;0;381;0
WireConnection;394;1;395;0
WireConnection;138;0;120;0
WireConnection;316;0;315;0
WireConnection;143;0;191;0
WireConnection;143;1;138;0
WireConnection;385;0;316;0
WireConnection;385;1;394;0
WireConnection;242;0;250;0
WireConnection;242;1;231;0
WireConnection;252;0;242;0
WireConnection;396;0;385;0
WireConnection;254;0;143;0
WireConnection;350;0;396;0
WireConnection;240;0;257;0
WireConnection;240;1;253;0
WireConnection;300;0;240;0
WireConnection;300;1;297;0
WireConnection;300;2;351;0
WireConnection;241;0;300;0
WireConnection;0;2;241;0
ASEEND*/
//CHKSM=02A0E41B70584AB298F3D70A2DF1874186EF641B