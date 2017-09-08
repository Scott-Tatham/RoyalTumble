#ifndef EFFECTLIB_INCLUDED
#define EFFECTLIB_INCLUDED

#include "UnityCG.cginc"

uniform float _RefIndex;
uniform samplerCUBE _Cube;

struct VI
{
	float4 pos : POSITION;
	float3 norm : NORMAL;
};

struct VO
{
	float4 pos : SV_POSITION;
	float3 wNorm : NORMAL;
	half3 wView : TEXCOORD0;
};

VO vert(VI vi)
{
	VO vo;
	vo.pos = UnityObjectToClipPos(vi.pos.xyz);
	vo.wNorm = UnityObjectToWorldNormal(vi.norm);
	vo.wView = WorldSpaceViewDir(vi.pos);

	return vo;
}

fixed4 frag(VO i) : SV_Target
{
	float3 refractedDir = refract(normalize(i.wView), normalize(i.wNorm), 1.0 / _RefIndex);
	return texCUBE(_Cube, refractedDir);
}

#endif