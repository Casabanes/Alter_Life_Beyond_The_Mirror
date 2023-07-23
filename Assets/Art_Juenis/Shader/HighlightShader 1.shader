// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WaterfallShader"
{
	Properties
	{
		_123592787seamlesspatternthecrackstexturewhiteandblackvectorbackgroundmosaicvector1("123592787-seamless-pattern-the-cracks-texture-white-and-black-vector-background-mosaic-vector (1)", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Tiling("Tiling", Vector) = (0,0,0,0)
		_Velocidadpanneo("Velocidad panneo", Float) = 0.15
		_Vector2("Vector 2", Vector) = (1,1,0,0)
		_Vector1("Vector 1", Vector) = (0.5,0.5,0,0)
		_Colorespuma("Color espuma", Color) = (0.4245283,0.4225258,0.4225258,0)
		_StepEspuma("StepEspuma", Float) = 0.5
		_SubAddEspuma("SubAddEspuma", Float) = 2.93
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float2 _Vector2;
		uniform float2 _Vector1;
		uniform float _StepEspuma;
		uniform float _SubAddEspuma;
		uniform float4 _Colorespuma;
		uniform sampler2D _TextureSample0;
		uniform float _Velocidadpanneo;
		uniform float2 _Tiling;
		uniform sampler2D _123592787seamlesspatternthecrackstexturewhiteandblackvectorbackgroundmosaicvector1;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord211 = i.uv_texcoord * _Vector2;
			float mulTime164 = _Time.y * _Velocidadpanneo;
			float2 _Vector0 = float2(3,3);
			float dotResult4_g1 = dot( float2( 0,0 ) , float2( 12.9898,78.233 ) );
			float lerpResult10_g1 = lerp( _Vector0.x , _Vector0.y , frac( ( sin( dotResult4_g1 ) * 43758.55 ) ));
			float2 temp_cast_0 = (lerpResult10_g1).xx;
			float2 uv_TexCoord166 = i.uv_texcoord * _Tiling;
			float2 myVarName195 = uv_TexCoord166;
			float2 panner184 = ( sin( mulTime164 ) * temp_cast_0 + myVarName195);
			float4 color121 = IsGammaSpace() ? float4(0.1826273,0.4412826,0.6792453,1) : float4(0.02794765,0.1636605,0.418999,1);
			float4 color140 = IsGammaSpace() ? float4(0.1066661,0.3028149,0.4811321,0) : float4(0.01108867,0.07464047,0.196991,0);
			float4 lerpResult120 = lerp( color121 , color140 , tex2D( _123592787seamlesspatternthecrackstexturewhiteandblackvectorbackgroundmosaicvector1, myVarName195 ));
			o.Emission = saturate( ( ( saturate( ( ( distance( uv_TexCoord211 , _Vector1 ) * _StepEspuma ) - _SubAddEspuma ) ) * _Colorespuma ) + ( tex2D( _TextureSample0, panner184 ) + saturate( lerpResult120 ) ) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
920;92;993;540;-946.1471;314.2477;1.397094;False;False
Node;AmplifyShaderEditor.Vector2Node;193;226.996,31.4444;Inherit;False;Property;_Tiling;Tiling;2;0;Create;True;0;0;False;0;False;0,0;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;166;367.5486,35.90257;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;212;297.6652,-387.9153;Inherit;False;Property;_Vector2;Vector 2;4;0;Create;True;0;0;False;0;False;1,1;1.73,1.94;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;195;582.0268,49.11048;Inherit;False;myVarName;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;211;424.3219,-411.6267;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;210;469.2136,-292.3096;Inherit;False;Property;_Vector1;Vector 1;5;0;Create;True;0;0;False;0;False;0.5,0.5;0.86,1.05;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;165;307.5146,253.9488;Inherit;False;Property;_Velocidadpanneo;Velocidad panneo;3;0;Create;True;0;0;False;0;False;0.15;-0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;207;641.0533,-360.5512;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;214;690.1277,-113.9249;Inherit;False;Property;_StepEspuma;StepEspuma;7;0;Create;True;0;0;False;0;False;0.5;338.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;185;308.1693,132.4097;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;False;3,3;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;164;498.9312,260.6918;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;194;293.7251,703.3157;Inherit;False;195;myVarName;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;188;464.4938,698.7778;Inherit;True;Property;_123592787seamlesspatternthecrackstexturewhiteandblackvectorbackgroundmosaicvector1;123592787-seamless-pattern-the-cracks-texture-white-and-black-vector-background-mosaic-vector (1);0;0;Create;True;0;0;False;0;False;-1;336ccac88ee14cf4b918a92d888eadbf;336ccac88ee14cf4b918a92d888eadbf;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;244;912.1277,-493.3956;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;249;979.7723,-239.2621;Inherit;False;Property;_SubAddEspuma;SubAddEspuma;8;0;Create;True;0;0;False;0;False;2.93;350;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;121;574.8661,355.16;Inherit;False;Constant;_Color0;Color 0;8;0;Create;True;0;0;False;0;False;0.1826273,0.4412826,0.6792453,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;251;469.2323,135.0013;Inherit;False;Random Range;-1;;1;7b754edb8aebbfb4a9ace907af661cfc;0;3;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;163;662.8862,259.4714;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;140;578.6481,525.0314;Inherit;False;Constant;_Color1;Color 1;12;0;Create;True;0;0;False;0;False;0.1066661,0.3028149,0.4811321,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;120;924.3766,451.5102;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;248;1189.498,-294.5868;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;184;820.4333,166.4937;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;191;1069.195,213.9742;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;False;-1;c753d52eed54dd643a0d395a12b047d6;c753d52eed54dd643a0d395a12b047d6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;250;1399.53,-225.6451;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;138;1185.863,457.4132;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;231;1311.798,-102.3962;Inherit;False;Property;_Colorespuma;Color espuma;6;0;Create;True;0;0;False;0;False;0.4245283,0.4225258,0.4225258,0;0.3679245,0.3679245,0.3679245,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;242;1584.888,-22.77856;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;143;1385.445,365.9234;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;240;1773.075,56.60788;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;241;1989.44,73.51148;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2155.52,-180.2784;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;WaterfallShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;166;0;193;0
WireConnection;195;0;166;0
WireConnection;211;0;212;0
WireConnection;207;0;211;0
WireConnection;207;1;210;0
WireConnection;164;0;165;0
WireConnection;188;1;194;0
WireConnection;244;0;207;0
WireConnection;244;1;214;0
WireConnection;251;2;185;1
WireConnection;251;3;185;2
WireConnection;163;0;164;0
WireConnection;120;0;121;0
WireConnection;120;1;140;0
WireConnection;120;2;188;0
WireConnection;248;0;244;0
WireConnection;248;1;249;0
WireConnection;184;0;195;0
WireConnection;184;2;251;0
WireConnection;184;1;163;0
WireConnection;191;1;184;0
WireConnection;250;0;248;0
WireConnection;138;0;120;0
WireConnection;242;0;250;0
WireConnection;242;1;231;0
WireConnection;143;0;191;0
WireConnection;143;1;138;0
WireConnection;240;0;242;0
WireConnection;240;1;143;0
WireConnection;241;0;240;0
WireConnection;0;2;241;0
ASEEND*/
//CHKSM=ACA235986C787066052C864B00846313CFC083C2