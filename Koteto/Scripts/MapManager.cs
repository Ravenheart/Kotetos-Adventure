using System;
using System.Xml.Linq;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Koteto
{
	public class MapManager
	{
		public Texture2D TerrainTexture;
		public Layer[] Layers;
		public SpriteBatch spriteBatch;

		public MapManager()
		{
			
		}

		public void Initialize(Texture2D terrain, string mapXml, SpriteBatch spriteBatch)
		{
			this.TerrainTexture = terrain;
			this.spriteBatch = spriteBatch;

			//parse map
			XDocument xml = XDocument.Parse(mapXml);
			var layers = xml.Descendants()
			   .Where(p => p.Name == "layer")
			   .ToList();
			this.Layers = new Layer[layers.Count];
			for (int i = 0; i < layers.Count; i++)
			{
				var dataValue = layers[i].Descendants()
									.Where(p => p.Name == "data")
									.FirstOrDefault()
									.Value;

				var dataRows = dataValue.Split(new string[1] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

				int w = Convert.ToInt32(layers[i].Attribute("width").Value);
				int h = Convert.ToInt32(layers[i].Attribute("height").Value);

				Layer l = new Layer(w, h);
				this.Layers[i] = l;
				for (int row = 0; row < h; row++)
				{
					string[] dataRowIds = dataRows[row].Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					for (int col = 0; col < w; col++)
					{
						l.TileIds[col, row] = Convert.ToInt32(dataRowIds[col].Trim());
					}
				}
			}
		}

		public void Draw(GameTime gameTime)
		{
			for (int lay = 0; lay < this.Layers.Length; lay++)
			{
				var layer = this.Layers[lay];
				for (int row = 0; row < 100; row++)
				{
					for (int col = 0; col < 100; col++)
					{
						int tileId = layer.TileIds[col, row];
						this.spriteBatch.Draw(
							this.TerrainTexture,
							null,
							new Rectangle(col * 32, row * 32, 32, 32),
							GetTileRectangle(tileId));
					}
				}
			}
		}

		private Rectangle GetTileRectangle(int tileId)
		{
			int row = (int)(tileId / 32);
			int col = (tileId % 32) - 1;

			return new Rectangle(col * 32, row * 32, 32, 32);
		}
	}

	public class Layer
	{
		public int[,] TileIds;

		public Layer(int width, int height)
		{
			this.TileIds = new int[width, height];
		}
	}
}
