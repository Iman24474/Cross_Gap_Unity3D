// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Keystone" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "" {}
    }
   
    CGINCLUDE
   
    #include "UnityCG.cginc"
   
    struct v2f {
        float4 pos : POSITION;
        float2 uv : TEXCOORD0;
    };
   
    sampler2D _MainTex;
   
    float2 scale;

    v2f vert( appdata_img v )
    {
        v2f o;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv = v.texcoord.xy;
        return o;
    }
   
    half4 frag(v2f i) : COLOR
    {
    //float top = 1;
    //float bottom = 0.8;
    float _half = (scale.x + scale.y)* 0.5;
    float _diff = (scale.y - scale.x)* 0.5;
        half2 coords = i.uv;
        coords = (coords - 0.5) * 2.0;       
       
        half2 newCoordOffs;
        newCoordOffs.x = (coords.x) * (_half - _diff * coords.y);
        //newCoordOffs.x = (newCoordOffs.x + 1) * 0.5 ;
        //newCoordOffs.y = 1 + (coords.y - 1)* 0.5;
        newCoordOffs.x = (newCoordOffs.x + 1) * 0.5 ;
        newCoordOffs.y = 1 + (coords.y - 1)* 0.5;
        half4 color = tex2D (_MainTex,  newCoordOffs);     
       
        return color;
    }

    ENDCG
   
Subshader {
 Pass {
      ZTest Always Cull Off ZWrite Off
      Fog { Mode off }     

      CGPROGRAM
      #pragma fragmentoption ARB_precision_hint_fastest
      #pragma vertex vert
      #pragma fragment frag
      ENDCG
  }
 
}

Fallback off
   
} // shader