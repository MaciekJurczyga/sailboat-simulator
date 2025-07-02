Shader "Custom/DepthMask"
{
    SubShader
    {
        Tags {"Queue" = "Geometry-10"}
        ColorMask 0
        ZWrite On
        ZTest LEqual
        Pass {}
    }
}
