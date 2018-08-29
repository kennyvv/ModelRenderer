using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModelRenderer.Properties;
using ModelRenderer.STL;
using ModelRenderer.Util;
using StlRender;

namespace ModelRenderer
{
	public class GameWindow : Game
	{
		GraphicsDeviceManager graphics;
		private SpriteBatch SpriteBatch;
		private Camera Camera { get; set; }
		private FrameCounter FrameCounter { get; }
		public SpriteFont Font { get; set; }
		public GameWindow()
		{
			graphics = new GraphicsDeviceManager(this);
			FrameCounter = new FrameCounter();

			Content.RootDirectory = "Content";
			base.IsFixedTimeStep = false;
		}

		private IRenderer ObjectRenderer;
		protected override void LoadContent()
		{
			Camera = new Camera(GraphicsDevice);
			SpriteBatch = new SpriteBatch(GraphicsDevice);

			Font = Content.Load<SpriteFont>("test");
		/*	STLDocument document;
			using (FileStream fs = new FileStream("D:\\Downloads\\Strengthed_Pikachu\\files\\pikachu_edited.stl", FileMode.Open))
			{
				document = STLDocument.Read(fs);
			}

			ObjectRenderer = new STLRenderer(GraphicsDevice, document)
			{
				Color = Color.Yellow
			};
			*/

		//	ObjectRenderer = new WaveFrontRenderer(GraphicsDevice, "D:\\Downloads\\4vzuq0i60k-BMW850\\OBJ\\Castelia City.obj");
			//ObjectRenderer = new WaveFrontRenderer(GraphicsDevice, "D:\\Downloads\\4vzuq0i60k-BMW850\\Center city Sci-Fi\\Center City Sci-Fi.obj")
			//ObjectRenderer = new WaveFrontRenderer(GraphicsDevice, "D:\\Downloads\\4vzuq0i60k-BMW850\\Luke_skywalkers_landspeeder\\Luke Skywalkers landspeeder.obj");
			//ObjectRenderer = new WaveFrontRenderer(GraphicsDevice, "D:\\Downloads\\4vzuq0i60k-BMW850\\BMW850\\BMW850.obj");
			ObjectRenderer = new WaveFrontRenderer(GraphicsDevice, "D:\\Downloads\\4vzuq0i60k-BMW850\\DPV\\dpv.obj");
		}

		protected override void Update(GameTime gameTime)
		{
			if (IsActive)
			{
				Camera.ProcessInput(gameTime);
			}

			ObjectRenderer.Update(gameTime, Camera.World, Camera.View, Camera.Projection);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			FrameCounter.Update((float) gameTime.ElapsedGameTime.TotalSeconds);

			GraphicsDevice.Clear(Color.CornflowerBlue);

			ObjectRenderer.Render(GraphicsDevice);

			BlendState blendState = GraphicsDevice.BlendState;
			var depthState = GraphicsDevice.DepthStencilState;
			var rasterizerState = GraphicsDevice.RasterizerState;

			SpriteBatch.Begin();
			SpriteBatch.DrawString(Font, $"FPS: {FrameCounter.CurrentFramesPerSecond:F0}", Vector2.One, Color.White);
			SpriteBatch.End();

			GraphicsDevice.BlendState = blendState;
			GraphicsDevice.DepthStencilState = depthState;
			GraphicsDevice.RasterizerState = rasterizerState;

			base.Draw(gameTime);
		}
	}
}
