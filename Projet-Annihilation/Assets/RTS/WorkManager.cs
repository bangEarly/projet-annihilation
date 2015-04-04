using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RTS 
{
	public class WorkManager 
	{

		public static Rect CalculateSelectionBox(Bounds selectionBounds, Rect playingArea)
		{
			//coordonnee du centre des "bornes de la selection"
			float cx = selectionBounds.center.x;
			float cy = selectionBounds.center.y;
			float cz = selectionBounds.center.z;

			//coordonnee des extensions des "bornes de la selection"
			float ex = selectionBounds.extents.x;
			float ey = selectionBounds.extents.y;
			float ez = selectionBounds.extents.z;

			//coordonee d'ecran pour les coin des limites de la selection
			List< Vector3 > corners = new List< Vector3 > ();
			corners.Add (Camera.main.WorldToScreenPoint (new Vector3 (cx + ex, cy + ey, cz + ez)));
			corners.Add (Camera.main.WorldToScreenPoint (new Vector3 (cx + ex, cy + ey, cz - ez)));
			corners.Add (Camera.main.WorldToScreenPoint (new Vector3 (cx + ex, cy - ey, cz + ez)));
			corners.Add (Camera.main.WorldToScreenPoint (new Vector3 (cx - ex, cy + ey, cz - ez)));
			corners.Add (Camera.main.WorldToScreenPoint (new Vector3 (cx + ex, cy - ey, cz - ez)));
			corners.Add (Camera.main.WorldToScreenPoint (new Vector3 (cx - ex, cy - ey, cz + ez)));
			corners.Add (Camera.main.WorldToScreenPoint (new Vector3 (cx - ex, cy + ey, cz - ez)));
			corners.Add (Camera.main.WorldToScreenPoint (new Vector3 (cx - ex, cy - ey, cz - ez)));

			//recherche des bords a l'ecran pour les bords de la selection
			Bounds screenBounds = new Bounds (corners [0], Vector3.zero);
			for (int i = 1; i < corners.Count; i++) 
			{
				screenBounds.Encapsulate(corners[i]);
			}

			float selectBoxTop = playingArea.height - (screenBounds.center.y + screenBounds.extents.y);
			float selectBoxLeft = screenBounds.center.x - screenBounds.extents.x;
			float selectBoxWidth = 2 * screenBounds.extents.x;
			float selectBoxHeight = 2 * screenBounds.extents.y;

			return new Rect(selectBoxLeft, selectBoxTop, selectBoxWidth, selectBoxHeight);

		}

		public static GameObject FindHitObject(Vector3 origin) //recherche de l'objet sur lequel le joueur a clique
		{
			Ray ray = Camera.main.ScreenPointToRay (origin);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) 
			{
				return hit.collider.gameObject;
			} 
			else 
			{
				return null;
			}
			
		}
		
		public static Vector3 FindHitPoint(Vector3 origin) //verification que le point clique vise un "point cliquable"
		{
			Ray ray = Camera.main.ScreenPointToRay (origin);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) 
			{
				return hit.point;
			} 
			else 
			{
				return RessourceManager.InvalidPosition;
			}
		}

	}
}
