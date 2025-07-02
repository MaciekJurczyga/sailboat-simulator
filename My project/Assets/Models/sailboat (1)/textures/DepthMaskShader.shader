Shader "Custom/DepthMask" {
    SubShader {
        // Render before geometry to update depth buffer
        Tags { "Queue" = "Geometry-10" }
        // Turn off color rendering
        ColorMask 0
        ZWrite On
        Pass {}
    }
}
