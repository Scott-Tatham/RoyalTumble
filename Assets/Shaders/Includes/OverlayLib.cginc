#ifndef OVERLAYLIB_INCLUDED
#define OVERLAYLIB_INCLUDED

#include "UnityCG.cginc"

uniform int _Index;
uniform fixed4 _Colour;

struct VI
{
	float4 pos : POSITION;
	float3 norm : NORMAL;
	float4 uv : TEXCOORD0;
};

struct VO
{
	float4 pos : SV_POSITION;
	half3 uv : TEXCOORD0;
	float3 wPos : TEXCOORD1;
};

VO vert(VI vi)
{
	VO vo;
	vo.pos = UnityObjectToClipPos(vi.pos.xyz);
	vo.uv = vi.uv;
	vo.wPos = mul(unity_ObjectToWorld, vi.pos);

	return vo;
}

UNITY_DECLARE_TEX2DARRAY(_TexArr);

fixed4 frag(VO i) : SV_Target
{
	i.uv.z = _Index;
	fixed4 tex = UNITY_SAMPLE_TEX2DARRAY(_TexArr, i.uv);
	fixed4 pixel = tex * _Colour;
	//pixel.a = tex.a;

	return pixel;
}

#endif