using System.IO;
using Microsoft.Xna.Framework;
using ModelRenderer.STL;
using StlRender;

namespace ModelRenderer
{
	public class GameWindow : Game
	{
		GraphicsDeviceManager graphics;

		private Camera Camera { get; set; }
		public GameWindow()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			base.IsFixedTimeStep = false;
		}

		private IRenderer ObjectRenderer;
		protected override void LoadContent()
		{
			Camera = new Camera(GraphicsDevice);

			STLDocument document;
			using (FileStream fs = new FileStream("D:\\Downloads\\Strengthed_Pikachu\\files\\pikachu_edited.stl", FileMode.Open))
			{
				document = STLDocument.Read(fs);
			}

			ObjectRenderer = new STLRenderer(GraphicsDevice, document)
			{
				Color = Color.Yellow
			};

			//ObjectRenderer = new WaveFrontRenderer(GraphicsDevice, "D:\\Downloads\\4vzuq0i60k-BMW850\\OBJ\\Castelia City.obj");
			//ObjectRenderer = new WaveFrontRenderer(GraphicsDevice, "D:\\Downloads\\4vzuq0i60k-BMW850\\Center city Sci-Fi\\Center City Sci-Fi.obj");
			//ObjectRenderer = new WaveFrontRenderer(GraphicsDevice, "D:\\Downloads\\4vzuq0i60k-BMW850\\Luke_skywalkers_landspeeder\\Luke Skywalkers landspeeder.obj");
			//ObjectRenderer = new WaveFrontRenderer(GraphicsDevice, "D:\\Downloads\\4vzuq0i60k-BMW850\\BMW850\\BMW850.obj");
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
			GraphicsDevice.Clear(Color.CornflowerBlue);

			ObjectRenderer.Render(GraphicsDevice);
			base.Draw(gameTime);
		}
	}
}
