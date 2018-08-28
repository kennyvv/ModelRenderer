using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ModelRenderer
{
	public interface IRenderer
	{
		void Render(GraphicsDevice graphics);
		void Update(GameTime gameTime, Matrix world, Matrix view, Matrix projection);
	}
}
