using UnityEngine;
using System.Collections;
 
namespace RTS {
	public static class RessourceManager {

		//variables camera
		public static int ScrollWidth { get { return 15; } }
		public static float ScrollSpeed { get { return 25; } }
		public static float RotateAmount { get { return 10; } }
		public static float RotateSpeed { get { return 100; } }
		public static float MinCameraHeight { get { return 10; } }
		public static float MaxCameraHeight { get { return 40; } }

		//variables selection
		private static Vector3 invalidPosition = new Vector3 (-9999, -9999, -9999);
		public static Vector3 InvalidPosition { get { return invalidPosition; } }

		//variables boite de selection
		private static GUISkin selectBoxSkin;
		public static GUISkin SelectBoxSkin { get { return selectBoxSkin; } }
		public static void StoreSelectBoxItems(GUISkin skin)
		{
			selectBoxSkin = skin;
		}
		private static Bounds invalidBounds = new Bounds(new Vector3(-9999, -9999, -9999), new Vector3(0, 0, 0));
		public static Bounds InvalidBounds { get { return invalidBounds; } } 
	}

}


