using Microsoft.Xna.Framework.Graphics;

namespace ModelRenderer.WaveFront.Model
{
    /// <summary>
    /// Represents a texture map.
    /// </summary>
    public class TextureMap
    {
        /// <summary>
        /// Gets the texture image.
        /// Note that this is only set if the file is loaded with the 'loadTextureImages' option set to <c>true</c>.
        /// </summary>
        public Texture2D Image { get; internal set; } 
    }
}