Shader "Hidden/Shader/Sobel"
{
    Properties
    {
        // This property is necessary to make the CommandBuffer.Blit bind the source texture to _MainTex
        _MainTex("Main Texture", 2DArray) = "grey" {}
    }

    HLSLINCLUDE

    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/FXAA.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"

    struct Attributes
    {
        uint vertexID : SV_VertexID;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float2 texcoord   : TEXCOORD0;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    Varyings Vert(Attributes input)
    {
        Varyings output;
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
        output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
        return output;
    }

    // List of properties to control your post process effect
    float _Intensity;
    float4 _Color;
    float _Thickness;
    float _DepthMultiplier;
    float _DepthBias;
    TEXTURE2D_X(_MainTex);

    float Sobel_Basic(
        float topLeft, float top, float topRight,
        float centerLeft, float centerRight,
        float bottomLeft, float bottom, float bottomRight)
    {
        float x = topLeft + 2*centerLeft + bottomLeft - topRight - 2*centerRight - bottomRight;
        float y = -topLeft - 2*top - topRight + bottomLeft + 2*bottom + bottomRight;

        return sqrt(x * x + y * y);
    }
    
    float Sobel_Scharr(
        float topLeft, float top, float topRight,
        float centerLeft, float centerRight,
        float bottomLeft, float bottom, float bottomRight)
    {
        float x = -3 * topLeft - 10*centerLeft - 3 * bottomLeft + 3*topRight + 10*centerRight + 3*bottomRight;
        float y = 3 * topLeft + 10*top + 3*topRight - 3*bottomLeft - 10*bottom - bottomRight;

        return sqrt(x * x + y * y);
    }
    
    float SobelSampleDepth(float2 uv, float offsetU, float offsetV)
    {
        float topLeft =         SampleCameraDepth(uv + float2(-offsetU, offsetV));
        float top =             SampleCameraDepth(uv + float2(0, offsetV));
        float topRight =        SampleCameraDepth(uv + float2(offsetU, offsetV));

        float centerLeft =     SampleCameraDepth(uv + float2(-offsetU, 0));
        float center =         SampleCameraDepth(uv + float2(0, 0));
        float centerRight =    SampleCameraDepth(uv + float2(offsetU, 0));
        
        float bottomLeft =     SampleCameraDepth(uv + float2(-offsetU, -offsetV));
        float bottom =         SampleCameraDepth(uv + float2(0, -offsetV));
        float bottomRight =    SampleCameraDepth(uv + float2(offsetU, -offsetV));
        
        return Sobel_Basic(
            abs(topLeft - center),
            abs(top - center),
            abs(topRight - center),
            abs(centerLeft - center),
            abs(centerRight - center),
            abs(bottomLeft - center),
            abs(bottom - center),
            abs(bottomRight - center));
    }
    
    float4 CustomPostProcess(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        // Note that if HDUtils.DrawFullScreen is used to render the post process, use ClampAndScaleUVForBilinearPostProcessTexture(input.texcoord.xy) to get the correct UVs

        float3 sourceColor = SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, input.texcoord).xyz;

        // Determine co-ordinates
        float offsetU = _Thickness / _ScreenSize.x;
        float offsetV = _Thickness / _ScreenSize.y;

        // Run the depth sobel sampling
        float sobelDepth = SobelSampleDepth(input.texcoord.xy, offsetU, offsetV);
        sobelDepth = pow(abs(saturate(sobelDepth)) * _DepthMultiplier, _DepthBias);
        
        // Apply sobel effect
        float3 color = lerp(sourceColor, _Color, sobelDepth);
        
        //return float4(sobelDepth, sobelDepth, sobelDepth, 1);
        return float4(color, 1);
    }

    ENDHLSL

    SubShader
    {
        Tags{ "RenderPipeline" = "HDRenderPipeline" }
        Pass
        {
            Name "Sobel"

            ZWrite Off
            ZTest Always
            Blend Off
            Cull Off

            HLSLPROGRAM
                #pragma fragment CustomPostProcess
                #pragma vertex Vert
            ENDHLSL
        }
    }
    Fallback Off
}
