using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	[ExecuteInEditMode]
	[RequireComponent (typeof(Camera))]
	[AddComponentMenu ("Image Effects/Displacement/Keystone")]
	public class Keystone : PostEffectsBase
	{
		[Range(0f, 2f)]
		public float topX = 1.00f;
		[Range(0f, 2f)]
		public float bottomX = 1.00f;

		public Shader keystoneShader = null;
		private Material keystoneMaterial = null;


		public override bool CheckResources ()
		{
			CheckSupport (false);
			keystoneMaterial = CheckShaderAndCreateMaterial(keystoneShader,keystoneMaterial);

			if (!isSupported)
				ReportAutoDisable ();
			return isSupported;
		}

		void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
			if (CheckResources()==false)
			{
				Graphics.Blit (source, destination);
				return;
			}

			keystoneMaterial.SetVector ("scale", new Vector4 (topX,bottomX,0,0));

			Graphics.Blit (source, destination, keystoneMaterial);
		}
	}
}
