Shader "Tumble/Cel"
{
	Properties
	{
		_MainTex("Detail Map", 2D) = "white" {}
		_Normal("Normal Map", 2D) = "bump" {}
		_PalettePrimary("Palette Primary", 2D) = "white" {}
		_PaletteSecondary("Palette Secondary", 2D) = "black" {}
	}

	SubShader
	{
		Tags
		{
			"Lightmode" = "ForwardBase"
			"RenderQueue" = "Transparent"
			"IgnoreProjector" = "True"
			"CanUseSpriteAtlas" = "True"
		}

		Pass
		{
			ZWrite On
			ZTest On
			AlphaTest NotEqual 0.0
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM

			#pragma shader_feature _NORMAL
			#pragma shader_feature _FLOOR_ALPHA
			#pragma shader_feature _ROUND_ALPHA
			#pragma shader_feature _CEIL_ALPHA
			#pragma shader_feature _POINT_LIGHT

			#pragma vertex vert
			#pragma fragment frag
			
			#include "../Shaders/Includes/CelLib.cginc"
			
			ENDCG
		}
	}

	CustomEditor "CelEditor"
}