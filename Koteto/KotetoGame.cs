using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Koteto
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class KotetoGame : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		Camera2D camera;
		MapManager map;

		Texture2D terrainAtlas;

		public KotetoGame()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			Window.AllowUserResizing = true;
			Window.Position = Point.Zero;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			camera = new Camera2D(new BoxingViewportAdapter(Window, GraphicsDevice, 800, 450));
			map = new MapManager();
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			terrainAtlas = this.Content.Load<Texture2D>("Materials/Sprites/terrain_atlas");
			string mapXml = File.ReadAllText(Path.Combine("Content","Maps","test.tmx"));

			map.Initialize(terrainAtlas, mapXml, spriteBatch);
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			var kb = Keyboard.GetState();
			var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			float movementSpeed = 200;

			if (kb.IsKeyDown(Keys.Escape))
				Exit();

			var pos = camera.Position;
			if (kb.IsKeyDown(Keys.W))
			{
				camera.Position += new Vector2(0, -movementSpeed) * deltaTime;
				//camera.Move(new Vector2(0, -movementSpeed) * deltaTime);
			}

			if (kb.IsKeyDown(Keys.A))
			{
				camera.Position += new Vector2(-movementSpeed, 0) * deltaTime;
				//camera.Move(new Vector2(-movementSpeed, 0) * deltaTime);
			}

			if (kb.IsKeyDown(Keys.S))
			{
				camera.Position += new Vector2(0, movementSpeed) * deltaTime;
				//camera.Move(new Vector2(0, movementSpeed) * deltaTime);
			}

			if (kb.IsKeyDown(Keys.D))
			{
				camera.Position += new Vector2(movementSpeed, 0) * deltaTime;
				//camera.Move(new Vector2(movementSpeed, 0) * deltaTime);
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			Matrix transformMatrix = camera.GetViewMatrix(Vector2.Zero);
			spriteBatch.Begin(transformMatrix: transformMatrix);

			this.map.Draw(gameTime);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
