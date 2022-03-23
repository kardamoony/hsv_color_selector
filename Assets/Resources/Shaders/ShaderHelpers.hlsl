#ifndef SHADER_HELPERS_INCLUDED
#define SHADER_HELPERS_INCLUDED

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

    #define TRANSFORM_TEX_ANIMATE(tex,name,speed) (tex.xy * name##_ST.xy + name##_ST.zw + _Time.x * speed)

    float Remap(float value, float2 fromRange, float2 toRange)
    {
        return (toRange.y - toRange.x) * ((value - fromRange.x) / (fromRange.y - fromRange.x)) + toRange.x;
    }

    float Remap01(float value, float2 fromRange)
    {
        return (value - fromRange.x) / (fromRange.y - fromRange.x);
    }

    half Circle(float2 uv, float size)
    {
        float xdist = 0.5 - uv.x;
        float ydist = 0.5 - uv.y;

        float dist = sqrt(xdist * xdist + ydist * ydist);
        return step(dist, size);
    }

    half Circle(float2 uv, float size, float2 center)
    {
        float xdist = center.x - uv.x;
        float ydist = center.y - uv.y;

        float dist = sqrt(xdist * xdist + ydist * ydist);
        return step(dist, size);
    }

    half SmoothCircle(float2 uv, float size, float size0)
    {
        float xdist = 0.5 - uv.x;
        float ydist = 0.5 - uv.y;

        float dist = sqrt(xdist * xdist + ydist * ydist);
        return smoothstep(size0, size, dist);
    }

    float2 Rotate(float2 uv, float2 center, float angle)
    {
        float2x2 rotate = float2x2(cos(angle), -sin(angle), sin(angle), cos(angle));
        return mul(rotate, uv - center) + center;
    }

    float GetCosine(float2 direction1, float2 direction2)
    {
        float magnitude1 = sqrt(pow(direction1.x, 2) + pow(direction1.y, 2));
        float magnitude2 = sqrt(pow(direction2.x, 2) + pow(direction2.y, 2));
        return dot(direction1, direction2) / (magnitude1 * magnitude2);
    }

    float GetAngle(float2 direction1, float2 direction2)
    {
        return acos(GetCosine(direction1, direction2));
    }

    float2 Rotate(float2 vec, float rot) //rotation in radians
    {
        return float2(vec.x * cos(rot) + vec.y * sin(rot), -vec.x * sin(rot) + vec.y * cos(rot));
    }

    float2 ToPolar(float2 cartesian)
    {
        float distance = length(cartesian);
        float angle = atan2(cartesian.y, cartesian.x);
        return float2(angle / TWO_PI, distance);
    }

    float3 Hsv_to_rgb(float3 HSV)
    {
        float3 RGB = HSV.z;
       
        float var_h = HSV.x * 6;
        float var_i = floor(var_h);
        float var_1 = HSV.z * (1.0 - HSV.y);
        float var_2 = HSV.z * (1.0 - HSV.y * (var_h-var_i));
        float var_3 = HSV.z * (1.0 - HSV.y * (1-(var_h-var_i)));
        if      (var_i == 0) { RGB = float3(HSV.z, var_3, var_1); }
        else if (var_i == 1) { RGB = float3(var_2, HSV.z, var_1); }
        else if (var_i == 2) { RGB = float3(var_1, HSV.z, var_3); }
        else if (var_i == 3) { RGB = float3(var_1, var_2, HSV.z); }
        else if (var_i == 4) { RGB = float3(var_3, var_1, HSV.z); }
        else                 { RGB = float3(HSV.z, var_1, var_2); }
       
        return RGB;
    }


#endif