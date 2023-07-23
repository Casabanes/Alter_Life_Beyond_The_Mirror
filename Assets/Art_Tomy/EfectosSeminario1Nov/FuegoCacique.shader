// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FuegoCacique"
{
	Properties
	{
		_Alturafuego("Altura fuego", Float) = -0.25
		_CantidadFuego("Cantidad Fuego", Float) = 0.68
		_Tilingfuego("Tiling fuego", Vector) = (1,1,0,0)
		_Scalefuego("Scale fuego", Float) = 3.53
		_Stepfuego("Step fuego", Float) = 0.2
		_Velocidadpanner("Velocidad panner", Float) = 1
		_DireccionXpanner("Direccion X panner", Range( -2 , 2)) = -0.1413431
		_DireccionYpanner("Direccion Y panner", Range( -1 , 1)) = -1
		_Color1("Color 1", Color) = (1,0.989954,0,0)
		_Color0("Color 0", Color) = (1,0,0,0)
		_Coloroutline("Color outline", Color) = (0.1267338,1,0,0)
		_Cantidadoutline("Cantidad outline", Float) = 0.24
		_Alturacolorinterior("Altura color interior", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Cantidadoutline;
		uniform float _Stepfuego;
		uniform float _Velocidadpanner;
		uniform float _DireccionXpanner;
		uniform float _DireccionYpanner;
		uniform float2 _Tilingfuego;
		uniform float _Scalefuego;
		uniform float _CantidadFuego;
		uniform float _Alturafuego;
		uniform float4 _Coloroutline;
		uniform float4 _Color1;
		uniform float4 _Color0;
		uniform float _Alturacolorinterior;


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


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float mulTime23 = _Time.y * _Velocidadpanner;
			float2 appendResult27 = (float2(_DireccionXpanner , _DireccionYpanner));
			float2 uv_TexCoord13 = i.uv_texcoord * _Tilingfuego;
			float2 panner20 = ( mulTime23 * appendResult27 + uv_TexCoord13);
			float simplePerlin2D14 = snoise( panner20*_Scalefuego );
			simplePerlin2D14 = simplePerlin2D14*0.5 + 0.5;
			float temp_output_3_0 = ( simplePerlin2D14 * ( 1.0 - ( ( _CantidadFuego * i.uv_texcoord.y ) - _Alturafuego ) ) );
			float temp_output_3_0_g2 = ( ( _Cantidadoutline + _Stepfuego ) - temp_output_3_0 );
			float2 temp_cast_0 = (_Alturacolorinterior).xx;
			float4 lerpResult32 = lerp( _Color1 , _Color0 , float4( ( i.uv_texcoord - temp_cast_0 ), 0.0 , 0.0 ));
			o.Emission = saturate( ( ( saturate( ( temp_output_3_0_g2 / fwidth( temp_output_3_0_g2 ) ) ) * _Coloroutline ) + lerpResult32 ) ).rgb;
			float temp_output_3_0_g1 = ( _Stepfuego - temp_output_3_0 );
			float Mask28 = ( 1.0 - saturate( ( temp_output_3_0_g1 / fwidth( temp_output_3_0_g1 ) ) ) );
			o.Alpha = Mask28;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

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
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
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
-85;156;1352;695;344.7762;4602.057;1.3;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-536.0046,-3848.719;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;16;-769.1013,-4236.009;Inherit;False;Property;_Tilingfuego;Tiling fuego;2;0;Create;True;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;8;-507.1832,-3926.842;Inherit;False;Property;_CantidadFuego;Cantidad Fuego;1;0;Create;True;0;0;False;0;False;0.68;2.63;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-881.2228,-4105.007;Inherit;False;Property;_DireccionXpanner;Direccion X panner;6;0;Create;True;0;0;False;0;False;-0.1413431;-2;-2;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-882.2307,-4030.034;Inherit;False;Property;_DireccionYpanner;Direccion Y panner;7;0;Create;True;0;0;False;0;False;-1;-0.75;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-516.7799,-4012.061;Inherit;False;Property;_Velocidadpanner;Velocidad panner;5;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-310.1651,-3913.667;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-597.205,-4254.786;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;23;-320.2497,-4013.509;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-290.8658,-3689.073;Inherit;False;Property;_Alturafuego;Altura fuego;0;0;Create;True;0;0;False;0;False;-0.25;0.94;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;27;-518.0984,-4110.056;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;5;-80.5811,-3881.391;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-159.1285,-4074.599;Inherit;False;Property;_Scalefuego;Scale fuego;3;0;Create;True;0;0;False;0;False;3.53;5.94;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;20;-178.9763,-4198.839;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;11;354.9705,-3885.239;Inherit;False;Property;_Stepfuego;Step fuego;4;0;Create;True;0;0;False;0;False;0.2;0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;365.4586,-4277.328;Inherit;False;Property;_Cantidadoutline;Cantidad outline;11;0;Create;True;0;0;False;0;False;0.24;0.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;14;12.64168,-4166.243;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;9;134.2949,-3917.391;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;48;545.3586,-4258.728;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;53;86.19043,-3317.033;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;55;188.5016,-3139.493;Inherit;False;Property;_Alturacolorinterior;Altura color interior;12;0;Create;True;0;0;False;0;False;0;0.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;300.491,-4101.821;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;50;910.698,-4129.502;Inherit;False;Property;_Coloroutline;Color outline;10;0;Create;True;0;0;False;0;False;0.1267338,1,0,0;0.0781354,0.6226415,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;33;432.5554,-3505.401;Inherit;False;Property;_Color0;Color 0;9;0;Create;True;0;0;False;0;False;1,0,0,0;1,0.2449554,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;46;756.3586,-4381.728;Inherit;True;Step Antialiasing;-1;;2;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;12;522.3156,-4008.471;Inherit;True;Step Antialiasing;-1;;1;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;54;438.2612,-3259.859;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;34;433.3553,-3673.302;Inherit;False;Property;_Color1;Color 1;8;0;Create;True;0;0;False;0;False;1,0.989954,0,0;1,0.6401812,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;1148.421,-4099.41;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;32;709.4554,-3635.534;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;19;748.2213,-3979.871;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;28;905.4554,-3982.102;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;51;1088.238,-3754.863;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;52;1356.052,-3828.588;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;29;830.2617,-3241.149;Inherit;True;28;Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;18;1312.794,-3583.771;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;FuegoCacique;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;8;0
WireConnection;7;1;2;2
WireConnection;13;0;16;0
WireConnection;23;0;22;0
WireConnection;27;0;24;0
WireConnection;27;1;25;0
WireConnection;5;0;7;0
WireConnection;5;1;6;0
WireConnection;20;0;13;0
WireConnection;20;2;27;0
WireConnection;20;1;23;0
WireConnection;14;0;20;0
WireConnection;14;1;15;0
WireConnection;9;0;5;0
WireConnection;48;0;47;0
WireConnection;48;1;11;0
WireConnection;3;0;14;0
WireConnection;3;1;9;0
WireConnection;46;1;3;0
WireConnection;46;2;48;0
WireConnection;12;1;3;0
WireConnection;12;2;11;0
WireConnection;54;0;53;0
WireConnection;54;1;55;0
WireConnection;49;0;46;0
WireConnection;49;1;50;0
WireConnection;32;0;34;0
WireConnection;32;1;33;0
WireConnection;32;2;54;0
WireConnection;19;0;12;0
WireConnection;28;0;19;0
WireConnection;51;0;49;0
WireConnection;51;1;32;0
WireConnection;52;0;51;0
WireConnection;18;2;52;0
WireConnection;18;9;29;0
ASEEND*/
//CHKSM=EC79CEA0DF07E252ACBA7A00BA746D09E7D9AA83