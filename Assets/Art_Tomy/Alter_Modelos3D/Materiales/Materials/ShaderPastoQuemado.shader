// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ShaderPastoQuemado"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_MaskGrassShader1("MaskGrassShader 1", 2D) = "white" {}
		_TexOtroGrass("TexOtroGrass", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TexOtroGrass;
		uniform float4 _TexOtroGrass_ST;
		uniform sampler2D _MaskGrassShader1;
		uniform float4 _MaskGrassShader1_ST;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexOtroGrass = i.uv_texcoord * _TexOtroGrass_ST.xy + _TexOtroGrass_ST.zw;
			o.Albedo = tex2D( _TexOtroGrass, uv_TexOtroGrass ).rgb;
			o.Alpha = 1;
			float2 uv_MaskGrassShader1 = i.uv_texcoord * _MaskGrassShader1_ST.xy + _MaskGrassShader1_ST.zw;
			float4 tex2DNode1 = tex2D( _MaskGrassShader1, uv_MaskGrassShader1 );
			clip( tex2DNode1.r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
744;746;1352;689;924.2473;376.9185;1.514998;True;False
Node;AmplifyShaderEditor.SamplerNode;2;-497.018,-146.6389;Inherit;True;Property;_TexOtroGrass;TexOtroGrass;2;0;Create;True;0;0;False;0;False;-1;1d5f02d33f926cc409fff413116ef590;1d5f02d33f926cc409fff413116ef590;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-342.4883,268.4705;Inherit;True;Property;_MaskGrassShader1;MaskGrassShader 1;1;0;Create;True;0;0;False;0;False;-1;54a44123a2113574c9a2824ae0858b79;54a44123a2113574c9a2824ae0858b79;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ShaderPastoQuemado;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;0;0;2;0
WireConnection;0;9;1;0
WireConnection;0;10;1;0
ASEEND*/
//CHKSM=13662623ED0E9F0D79379EF11AF774AD2DBFC3CA