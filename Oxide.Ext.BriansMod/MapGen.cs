namespace Oxide.Ext.BriansMod
{
	using System;
	using Services;
	using UnityEngine;

	public class MapGen
	{
		public static Color GetColorAt(Vector3 worldPos)
		{
			var color = TerrainMeta.ColorMap.GetColor(worldPos);
			float interpolatedSplat1;
			if ((interpolatedSplat1 = TerrainMeta.SplatMap.GetSplat(worldPos, 7)) > 0.0)
			{
				color = Color.Lerp(color, PathColor, interpolatedSplat1);
			}
			else
			{
				float interpolatedSplat2;
				if ((interpolatedSplat2 = TerrainMeta.SplatMap.GetSplat(worldPos, 6)) > 0.0)
					color = Color.Lerp(color, SnowColor, interpolatedSplat2);
			}
			return color;
		}


		private static Color Rgb(int rgb)
		{
			return new Color((rgb >> 16 & 255)/255f, (rgb >> 8 & 255)/255f, (rgb & 255)/255f, 1f);
		}

		private static Color NormalToColor(Vector3 normal)
		{
			return new Color(normal.x, normal.y, normal.z);
		}

		public static Texture2D[] RenderHeightMap(int outSize)
		{
			// the x and z are the height and width of the map, as determined by the server.mapsize
			// y is always 1000 (ground level being 500?)

			if (outSize < 1)
			{
				throw new Exception("invalid out size");
			}
			var x = (int) TerrainMeta.Size.x;
			var z = (int) TerrainMeta.Size.z;
			int halfOutSize = outSize/2;
			int factorX = x/outSize;
			int factorY = z/outSize;

			var heightMap = new Texture2D(outSize, outSize, TextureFormat.RGB24, false);
			var colorMap = new Texture2D(outSize, outSize, TextureFormat.RGB24, false);
			var normalMap = new Texture2D(outSize, outSize, TextureFormat.RGB24, false);

			for (var y = 0; y < outSize; y++)
			{
				for (var index = 0; index < outSize; ++index)
				{
					var worldPos = new Vector3((index - halfOutSize)*factorX, 0.0f, (y - halfOutSize)*factorY);
					float depthAtPos = TerrainMeta.HeightMap.GetHeight(worldPos) + 100f;
					var colorAtPos = GetColorAt(worldPos);
					var normalAtPos = TerrainMeta.NormalMap.GetNormal(worldPos);
					var colorVal = Convert.ToInt32(depthAtPos * 10000f);
					var depthColor = Rgb(colorVal);
					var normalColor = NormalToColor(normalAtPos);
					heightMap.SetPixel(index, y, depthColor);
					colorMap.SetPixel(index, y, colorAtPos);
					normalMap.SetPixel(index, y, normalColor);
				}
			}
			return new [] { heightMap, colorMap, normalMap } ;
		}

		public static Texture2D Render2D(int outSize)
		{
			if (outSize < 1)
				return null;
			var tex = new Texture2D(outSize, outSize, (TextureFormat) 3, false);
			var imgWidth = (int) TerrainMeta.Size.x;
			var imgHeight = (int) TerrainMeta.Size.z;
			int halfOutSize = outSize/2;
			int factorX = imgWidth/outSize;
			int factorY = imgHeight/outSize;
			float smoothStep = imgWidth/4000f;
			float dSmoothStep = smoothStep + smoothStep;
			var sun = new Vector3(1f, -1f, -1f).Inverse();
			Parallel.For(0, outSize, y =>
			{
				for (var index = 0; index < outSize; ++index)
				{
					var worldPos = new Vector3((index - halfOutSize)*factorX, 0.0f, (y - halfOutSize)*factorY);
					float posDepth = TerrainMeta.HeightMap.GetHeight(worldPos) - 0.5f;
					var depthBaseColor = GetColorAt(worldPos);
					if (posDepth > 0.0)
						depthBaseColor =
							Mix(
								Mix(depthBaseColor, 0.5f, GetColorAt(new Vector3(worldPos.x - smoothStep, 0.0f, worldPos.z - smoothStep)),
									GetColorAt(new Vector3(worldPos.x + smoothStep, 0.0f, worldPos.z - smoothStep)),
									GetColorAt(new Vector3(worldPos.x - smoothStep, 0.0f, worldPos.z + smoothStep)),
									GetColorAt(new Vector3(worldPos.x + smoothStep, 0.0f, worldPos.z + smoothStep))), 0.2f,
								GetColorAt(new Vector3(worldPos.x - dSmoothStep, 0.0f, worldPos.z - dSmoothStep)),
								GetColorAt(new Vector3(worldPos.x + dSmoothStep, 0.0f, worldPos.z - dSmoothStep)),
								GetColorAt(new Vector3(worldPos.x - dSmoothStep, 0.0f, worldPos.z + dSmoothStep)),
								GetColorAt(new Vector3(worldPos.x + dSmoothStep, 0.0f, worldPos.z + dSmoothStep)));
					float shade = sun.DotRadians(TerrainMeta.NormalMap.GetNormal(worldPos));
					var color2 = Lerp3(Color.Lerp(depthBaseColor, Color.white, 0.7f), depthBaseColor,
						Color.Lerp(depthBaseColor, Color.black, 0.8f),
						(float) (Math.Sin(shade) + 1.0)/2f);
					if (posDepth <= 0.0)
					{
						var color3 = Color.Lerp(WaterColor, WaterDeepColor, posDepth < -100.0 ? 1f : posDepth/-100f);
						color2 = Color.Lerp(color2, color3, posDepth < -1.0 ? 1f : -posDepth);
					}
					tex.SetPixel(index, y, color2);
				}
			});
			tex.Apply(false, false);
			return tex;
		}

		private static Color Mix(Color main, float amount, params Color[] mix)
		{
			float r = mix[0].r;
			float g = mix[0].g;
			float b = mix[0].b;
			for (var i = 1; i < mix.Length; ++i)
			{
				r = (float) (((double) r + mix[i].r)/2.0);
				g = (float) (((double) g + mix[i].g)/2.0);
				b = (float) (((double) b + mix[i].b)/2.0);
			}
			return Color.Lerp(main, new Color(r, g, b, 1f), amount);
		}

		private static Color Lerp3(Color a, Color b, Color c, float delta)
		{
			if (delta < 0.5)
				return Color.Lerp(a, b, delta*2f);
			return Color.Lerp(b, c, (float) ((delta - 0.5)*2.0));
		}

		private static readonly Color WaterColor = Rgb(1324614);
		private static readonly Color WaterDeepColor = Rgb(927796);
		private static readonly Color SnowColor = Rgb(16777215);
		private static readonly Color PathColor = Rgb(3552822);
	}
}