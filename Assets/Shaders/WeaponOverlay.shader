Shader "Tumble/Weapon Overlay"
{
	Properties
	{
		_TexArr("Noise Textures", 2DArray) = "" {}
		_Colour("Colour", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	
	SubShader
	{
		Tags
		{
			"Lightmode" = "ForwardBase"
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
		}
	
		Pass
		{
			ZWrite Off
			ZTest Off
			AlphaTest NotEqual 0.0
			Blend SrcAlpha OneMinusSrcAlpha
		
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "../Shaders/Includes/OverlayLib.cginc"
	
			ENDCG
		}
	}
}
