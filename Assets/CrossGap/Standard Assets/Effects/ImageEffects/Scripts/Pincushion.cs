using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [RequireComponent (typeof(Camera))]
    [AddComponentMenu ("Image Effects/Displacement/Pincushion")]
    public class Pincushion : PostEffectsBase
	{

        [Range(-1.5f, 1.5f)]
        public float strengthLeft = 0.00f;
		[Range(-1.5f, 1.5f)]
		public float strengthRight = 0.00f;
		[Range(-2f, 2f)]
		public float centerX = 1.00f;
		[Range(-2f, 2f)]
		public float centerY = 1.00f;

		public string ScreenName;
		private int screenID;
		private string screenPicked;
		private string sidePicked;
		private float leftVal, rightVal, xVal, yVal=0;

        public Shader PincushionShader = null;
        private Material PincushionMaterial = null;


        public override bool CheckResources ()
		{
            CheckSupport (false);
            PincushionMaterial = CheckShaderAndCreateMaterial(PincushionShader,PincushionMaterial);

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

            float oneOverBaseSize = 80.0f / 512.0f; // to keep values more like in the old version of Pincushion

            float ar = (source.width * 1.0f) / (source.height * 1.0f);

			PincushionMaterial.SetVector ("leftIntensity", new Vector4 (0, strengthLeft * oneOverBaseSize, 0, strengthLeft * oneOverBaseSize));
			PincushionMaterial.SetVector ("rightIntensity", new Vector4 (0, strengthRight * oneOverBaseSize, 0, strengthRight * oneOverBaseSize));
			PincushionMaterial.SetVector ("Center", new Vector4 (centerX,centerY,0f,0f));
            Graphics.Blit (source, destination, PincushionMaterial);
        }
		/*
		void OnGUI()
		{
			GUI.Label (new Rect (200, 320, 600, 20), "Current Screen:"+screenPicked);
			GUI.Label (new Rect (200, 350, 600, 20), "L/R: "+sidePicked);
			GUI.Label (new Rect (200, 370, 600, 20), "CenterX:"+Math.Round((double) centerX,3).ToString ());
			GUI.Label (new Rect (200, 390, 600, 20), "Left:"+Math.Round((double) strengthLeft,3).ToString ());
			GUI.Label (new Rect (200, 410, 600, 20), "CenterY:"+Math.Round((double) centerY,3).ToString ());
			GUI.Label (new Rect (200, 430, 600, 20), "Right:"+Math.Round((double) strengthRight,3).ToString ());
		}

		void Update()
		{
			if (Input.GetKey ("l")) {
				sidePicked = "l";
			} else if (Input.GetKey ("r")) {
				sidePicked = "r";
			}
			
			if (sidePicked == "l") {
				float l = Input.GetAxis ("HorizontalLeft");
				leftVal += l;
				float vl = Input.GetAxis ("VerticalLeft");
				xVal += vl;
			} else {
				float r = Input.GetAxis ("HorizontalLeft");
				rightVal += r;
				float vr = Input.GetAxis ("VerticalLeft");
				yVal += vr;
			}

			if (screenPicked == ScreenName) {
				strengthLeft = leftVal;
				strengthRight = rightVal;
				centerX = xVal;
				centerY = yVal;
				
			}

			if (Input.GetKey ("1")) {
				screenPicked = "Left";
				leftVal = strengthLeft;
				rightVal = strengthRight;
				centerX = xVal;
				centerY = yVal;
			}

			if (Input.GetKey ("2")) {
				screenPicked = "Front";
				leftVal = strengthLeft;
				rightVal = strengthRight;
				centerX = xVal;
				centerY = yVal;
			}
			if (Input.GetKey ("3")) {
				screenPicked = "Right";
				leftVal = strengthLeft;
				rightVal = strengthRight;
				centerX = xVal;
				centerY = yVal;
			}
			if (Input.GetKey ("s")) {
				System.IO.StreamWriter sw = new System.IO.StreamWriter (screenPicked+".txt", true);
				sw.WriteLine ("----------------------------------------------------------------------------");
				sw.WriteLine ("Current Screen:"+screenPicked);
				sw.WriteLine ("L/R: "+sidePicked);
				sw.WriteLine ("CenterX:"+Math.Round((double) centerX,3).ToString ());
				sw.WriteLine ("Left:"+Math.Round((double) strengthLeft,3).ToString ());
				sw.WriteLine ("CenterY:"+Math.Round((double) centerY,3).ToString ());
				sw.WriteLine ("Right:"+Math.Round((double) strengthRight,3).ToString ());

				sw.Flush ();
				sw.Close ();
			}

		}
		*/
			
    }
}
