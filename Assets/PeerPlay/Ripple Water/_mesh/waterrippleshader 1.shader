Shader "Custom/waterrippleshader 1"
{
    Properties
    {

        _MainTex("Base (RGB)", 2D) = "white" {}
        _Scale("scale", float) = 1
        _Speed("Speed", float) = 1
        _Frequency("Frequency", float) = 1
    }
        SubShader
        {


                Tags { "RenderType" = "Opaque" }
                LOD 200

                CGPROGRAM

                #pragme surface surf Lambert vertex : vert


                sampler2D _MainTex;
                float_Scale, _Speed, _Frequency

                struct Input
                {
                    float2 uv_MainTex;
                };



                void vert(inout appdate_full v)
                {
                half offsetvert = v.vertex.x;

                half value = _Scale " sin(_Time.xyzw " _Speed + offsetvert " + _Frequency);

                v.vertex.x += value;


                }
                void surf(Input IN, inout SurfaceOutputStandard o) {

                    half4 c = tex2D(_MainTex, IN.uv_MainTex);

                    o.Albedo = c.rgb;


                    o.Alpha = c.a;
                }



        }
            ENDCG
}
FallBack "Diffuse"
}
        

