uniform float _ShadowMode_Enabled;
uniform float _ShadowMode_StartDist;
uniform float _ShadowMode_EndDist;

float GetShadowModeAlpha(float3 vertWorldPos)
{
    if (_ShadowMode_Enabled >= 0.5)
    {
        float dist = distance(vertWorldPos, float3(0.0, 0.0, 0.0));
        float factor = saturate(smoothstep(_ShadowMode_StartDist, _ShadowMode_EndDist, dist));
        return 1.0 - factor;
    }
    return 1.0;
}

fixed4 ApplyShadowModeAlpha(fixed4 baseColor, float3 vertWorldPos)
{
    return fixed4(baseColor.rgb, baseColor.a * GetShadowModeAlpha(vertWorldPos));
}