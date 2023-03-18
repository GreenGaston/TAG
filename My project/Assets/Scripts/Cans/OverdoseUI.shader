Shader "Custom/OverdoseUI"
{
    Properties
    {
        
        _Percentage ("Percentage", Range(0,1)) = 0.0
        _Color1 ("Color1", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = (1,1,1,1)
        _Color3 ("Color3", Color) = (1,1,1,1)	
        //boolean
        _Boolean ("Boolean", Float) = 0.0

    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Pass{
            //Blend One One // Additive
            Cull Off
            CGPROGRAM
            #pragma vertex vp
            #pragma fragment fp

            float _Percentage, _Boolean;
            float4 _Color1, _Color2, _Color3;

            #include "UnityCG.cginc"

            struct VertexData {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vp(VertexData v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            float4 fp(v2f i) : SV_TARGET{
                if(_Boolean == 1.0){
                    if(i.uv.x > _Percentage)
                        return float4(0,0,0,0);
                    return _Color3;
                }
                if(i.uv.x > _Percentage)
                    return float4(0,0,0,0);
                else
                    return lerp(_Color1, _Color2, i.uv.x);
            }
            ENDCG

        }
    }
    FallBack "Diffuse"
}
