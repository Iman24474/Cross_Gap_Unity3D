// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Pincushion" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "" {}
	}
	
	// Shader code pasted into all further CGPROGRAM blocks
	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};
	
	sampler2D _MainTex;
	
	float2 leftIntensity;
	float2 rightIntensity;
	float2 Center;
	v2f vert( appdata_img v ) 
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	} 
	
	half4 frag(v2f i) : SV_Target 
	{
		half2 coords = i.uv;
		coords = (coords - 0.5) * 2.0;		

		half2 realCoordOffs;
		if (coords.x<0)
		{
		coords.y=coords.y-Center.x;
		realCoordOffs.x = ((1-coords.y * coords.y) * leftIntensity.y * coords.x); 
		realCoordOffs.y = ((1-coords.x * coords.x) * leftIntensity.x* coords.y);
		}
		else
		{
		coords.y=coords.y-Center.y;
		realCoordOffs.x = ((1-coords.y * coords.y) * rightIntensity.y * coords.x); 
		realCoordOffs.y = ((1-coords.x * coords.x) * rightIntensity.x* coords.y);
		}
		
		half4 color = tex2D (_MainTex, i.uv - realCoordOffs);	 
		
		return color;
	}

	ENDCG 
	
Subshader {
 Pass {
	  ZTest Always Cull Off ZWrite Off

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      ENDCG
  }
  
}

Fallback off
	
} // shader
