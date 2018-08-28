using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StlRender;

namespace ModelRenderer.STL
{
	public class STLRenderer : IRenderer
	{
		private Color _color = Color.Black;

		public Color Color
		{
			get { return _color; }
			set
			{
				_color = value;
				BuildBuffer();
			}
		}

		private STLDocument Document { get; }
		private VertexBuffer Buffer { get; set; } = null;
		private GraphicsDevice Graphics { get; }
		private BasicEffect _effect;
		public STLRenderer(GraphicsDevice graphics, STLDocument document)
		{
			Graphics = graphics;
			Document = document;

			BuildBuffer();

			_effect = new BasicEffect(graphics);
			_effect.VertexColorEnabled = true;

			_effect.EnableDefaultLighting();
			_effect.Alpha = 1f;
			_effect.PreferPerPixelLighting = true;
		}

		private void BuildBuffer()
		{
			var vertices = Document.GetVertices()
				.Select(source => new VertexPositionColorNormal(source.Position, Color, source.Normal)).ToArray();

			if (Buffer == null || vertices.Length > Buffer.VertexCount)
			{
				var oldBuffer = Buffer;
				var buffer = new VertexBuffer(Graphics, VertexPositionColorNormal.VertexDeclaration, vertices.Length,
					BufferUsage.WriteOnly);
				buffer.SetData(vertices);

				Buffer = buffer;
				oldBuffer?.Dispose();
			}
			else
			{
				Buffer.SetData(vertices);
			}
		}

		public void Render(GraphicsDevice graphics)
		{
			graphics.SetVertexBuffer(Buffer);

			graphics.RasterizerState = RasterizerState.CullClockwise;
			graphics.DepthStencilState = DepthStencilState.Default;
			//_effect.World = worldMatrix;

			foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
			{
				pass.Apply();

				graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, Buffer.VertexCount / 3);

			}
		}

		public void Update(GameTime gameTime, Matrix world, Matrix view, Matrix projection)
		{
			_effect.World = world;
			_effect.View = view;
			_effect.Projection = projection;
		}
	}
}
