// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Boosting effect"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Scale ("Scale", Range(0,100)) = 1
        _Empty ("Space between", Range(0,5)) = 1
        _Speed ("Speed", Range(0,5)) = 1
    }
   
   SubShader {

        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Pass {

            Blend One One // Additive
            Cull Off
            //ZWrite Off

            CGPROGRAM
            float _Scale;
            float3 _Color;
            float _Empty;
            float _Speed;
            #pragma vertex vert
            #pragma fragment frag

            float InverseLerp(float a, float b, float value) {
                return (value - a) / (b - a);
            }

            struct meshData {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 uv0 : TEXCOORD0;
            };

            struct Interpolator {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            #define TAU 6.28318530718
            //float TAU = 6.28318530718;
            Interpolator vert(meshData v) {
                //make the vertexes sway in the back and forth
                //it should sway in the x direction and it should be based on the y position 

               
                Interpolator o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                o.uv = v.uv0;
                return o;
            }

            fixed4 frag(Interpolator i) : SV_Target {
                //float xOffset= cos(i.uv.y*TAU*8)*0.1;
                
                float yOffset= cos(i.uv.y*TAU*8)*0.01;
                float TimeOffset= -(_Time.y*_Speed);
                float t = (cos((i.uv.y+TimeOffset)*TAU*_Scale)*0.5+0.5);

                t = (1-frac((i.uv.y+TimeOffset)*_Scale));
                //cast the range from 0-1 to -_Empty to 1
                float OldRange = 1 - 0;
                float NewRange = 1 - (-_Empty);
                t = (((t - 0) * NewRange) / OldRange) + (-_Empty);
                t=clamp(t,0,1);
                //if
                return fixed4(_Color,1)*t*!(abs(i.normal.y)>0.999);
            }

            ENDCG
        }
    }
}