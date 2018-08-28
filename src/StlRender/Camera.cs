using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ModelRenderer
{
	public class Camera
	{
		public Matrix Projection { get; set; }
		public Matrix View { get; set; } = Matrix.Identity;
		public Matrix World { get; set; } = Matrix.Identity;
		public Vector3 Position { get; set; } = new Vector3(0, 0, 0);

		private GraphicsDevice Graphics { get; }
		public Camera(GraphicsDevice graphics)
		{
			Graphics = graphics;

			UpdateViewMatrix();
			Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.Viewport.AspectRatio, 0.1f, 1000f);

			Mouse.SetPosition(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2);
			originalMouseState = Mouse.GetState();
		}

		float leftrightRot = MathHelper.PiOver2;
		float updownRot = -MathHelper.Pi / 10.0f;
		const float rotationSpeed = 0.3f;
		const float moveSpeed = 10.0f;
		private void UpdateViewMatrix()
		{
			Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);

			Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
			Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
			Vector3 cameraFinalTarget = Position + cameraRotatedTarget;

			Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);
			Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

			View = Matrix.CreateLookAt(Position, cameraFinalTarget, cameraRotatedUpVector);
		}

		private void AddToCameraPosition(Vector3 vectorToAdd)
		{
			Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);
			Vector3 rotatedVector = Vector3.Transform(vectorToAdd, cameraRotation);
			Position += moveSpeed * rotatedVector;
			UpdateViewMatrix();
		}

		KeyboardState currentKeys;
		private MouseState originalMouseState;
		public void ProcessInput(GameTime gameTime)
		{
			float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;
			Vector3 moveVector = new Vector3(0, 0, 0);
			KeyboardState keyState = Keyboard.GetState();

			if (keyState.IsKeyDown(Keys.W))
				moveVector += new Vector3(0, 0, -1);
			if (keyState.IsKeyDown(Keys.S))
				moveVector += new Vector3(0, 0, 1);
			if (keyState.IsKeyDown(Keys.D))
				moveVector += new Vector3(1, 0, 0);
			if (keyState.IsKeyDown(Keys.A))
				moveVector += new Vector3(-1, 0, 0);
			if (keyState.IsKeyDown(Keys.Space))
				moveVector += new Vector3(0, 1, 0);
			if (keyState.IsKeyDown(Keys.LeftShift))
				moveVector += new Vector3(0, -1, 0);

			AddToCameraPosition(moveVector * dt);

			moveVector = Vector3.Zero;

			Vector3 worldChangeFactor = Vector3.Zero;

			if (keyState.IsKeyDown(Keys.Up))
				worldChangeFactor += new Vector3(-1, 0, 0);// Matrix.CreateRotationX(-0.05f);
			if (keyState.IsKeyDown(Keys.Down))
				worldChangeFactor += new Vector3(1, 0, 0);//Matrix.CreateRotationX(0.05f);
			if (keyState.IsKeyDown(Keys.Left))
				worldChangeFactor += new Vector3(0, -1, 0);//Matrix.CreateRotationY(-0.05f);
			if (keyState.IsKeyDown(Keys.Right))
				worldChangeFactor += new Vector3(0, 1, 0);//Matrix.CreateRotationY(0.05f);

			if (keyState.IsKeyDown(Keys.Q))
				moveVector += new Vector3(0, 1, 0);
			if (keyState.IsKeyDown(Keys.Z))
				moveVector += new Vector3(0, -1, 0);

			worldChangeFactor *= dt;

			World *= Matrix.CreateRotationX(worldChangeFactor.X);
			World *= Matrix.CreateRotationY(worldChangeFactor.Y);
			//worldMatrix *= Matrix.CreateRotationZ(worldChangeFactor.Z);
			World *= Matrix.CreateTranslation(moveVector * dt);

			MouseState currentMouseState = Mouse.GetState();
			if (currentMouseState != originalMouseState)
			{
				float xDifference = currentMouseState.X - originalMouseState.X;
				float yDifference = currentMouseState.Y - originalMouseState.Y;
				leftrightRot -= rotationSpeed * xDifference * dt;
				updownRot -= rotationSpeed * yDifference * dt;
				Mouse.SetPosition(Graphics.Viewport.Width / 2, Graphics.Viewport.Height / 2);
				UpdateViewMatrix();
			}
		}
	}
}
