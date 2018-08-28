using Microsoft.Xna.Framework;

namespace ModelRenderer.WaveFront.Model
{
    /// <summary>
    /// Represents a material.
    /// </summary>
    public class Material
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the ambient.
        /// </summary>
        public Color Ambient { get; internal set; }

        /// <summary>
        /// Gets the diffuse.
        /// </summary>
        public Color Diffuse { get; internal set; }
        /// <summary>
        /// Gets the specular.
        /// </summary>
        public Color Specular { get; internal set; }
        /// <summary>
        /// Gets the shininess.
        /// </summary>
        public float Shininess { get; internal set; }
        /// <summary>
        /// Gets the transparency.
        /// </summary>
        public float? Transparency { get; internal set; }

        /// <summary>
        /// Gets the ambient texture map.
        /// </summary>
        public TextureMap TextureMapAmbient { get; internal set; }

        /// <summary>
        /// Gets the diffuse texture map.
        /// </summary>
        public TextureMap TextureMapDiffuse { get; internal set; }

        /// <summary>
        /// Gets the specular texture map.
        /// </summary>
        public TextureMap TextureMapSpecular { get; internal set; }
        
        /// <summary>
        /// Gets the specular highlight texture map.
        /// </summary>
        public TextureMap TextureMapSpecularHighlight { get; internal set; }

        /// <summary>
        /// Gets the alpha texture map.
        /// </summary>
        public TextureMap TextureMapAlpha { get; internal set; }

        /// <summary>
        /// Gets the bump texture map.
        /// </summary>
        public TextureMap TextureMapBump { get; internal set; }
        
        /// <summary>
        /// Gets the illumination model.
        /// See the specification at http://paulbourke.net/dataformats/mtl/ for details on this value.
        /// </summary>
        /// <value>
        /// The illumination model.
        /// </value>
        public int IlluminationModel { get; internal set; }

        /// <summary>
        /// Gets the optical density, also known as the Index of Refraction.
        /// </summary>
        public float? OpticalDensity { get; internal set; }

        /// <summary>
        /// Gets the occasionally used bump strength.
        /// </summary>
        public float? BumpStrength { get; internal set; }
    }
}