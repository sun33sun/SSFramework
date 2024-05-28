using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Enviro
{
	public static class GisHelper
	{
		public const float Radius = 6378137.0f * 0.000001f;

		public static Vector3 GisToSphereSpace(double lat, double lon, float radius = Radius)
		{
			float latRad = (float)(Mathf.Deg2Rad * lat);
			float lonRad = (float)(Mathf.Deg2Rad * lon);
			float x = radius * Mathf.Cos(latRad) * Mathf.Cos(lonRad);
			float y = radius * Mathf.Cos(latRad) * Mathf.Sin(lonRad);
			float z = radius * Mathf.Sin(latRad);
			return new Vector3(z, -y, -x);
		}

		public static Vector2 SphereSpacToGis(Vector3 pos, float radius = Radius)
		{
			float latRad = Mathf.Asin(pos.z / radius);
			float lat = latRad / Mathf.Deg2Rad;

			float tanLon = pos.y / pos.z;
			float lonRad = Mathf.Atan(tanLon);
			float lon = lonRad / Mathf.Deg2Rad;

			return new Vector2(lat, lon);
		}

		/// <summary>
		/// 后续添加的函数，经度、纬度与阿联相反
		/// </summary>
		/// <param name="lon">经度，[0,360]</param>
		/// <param name="lat">纬度，[0,180]</param>
		/// <returns></returns>
		private static int GisToIndex(int lon, int lat)
		{
			int index = lon * 180 + lat;

			return index;
		}
	}
}

