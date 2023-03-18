Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _Color1 ("Color1", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = (1,1,1,1)
        _Color3 ("Color3", Color) = (1,1,1,1)
        _Speed  ("Speed", Float) = 1
    }
    SubShader
    {
        Pass{
            CGPROGRAM
            #pragma vertex vp
            #pragma fragment fp

            float4 _Color1, _Color2, _Color3;
            float _Speed;

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
                //lerp between 3 color based on time it should go from color one to color 2 then to color 3 and back to color 1
                float t= frac(_Time.y*_Speed);
                if(t<0.33){
                    return lerp(_Color1,_Color2,t/0.33);
                }else if(t<0.66){
                    return lerp(_Color2,_Color3,(t-0.33)/0.33);
                }else{
                    return lerp(_Color3,_Color1,(t-0.66)/0.33);
                }

            }
            ENDCG
        }
    }
}
