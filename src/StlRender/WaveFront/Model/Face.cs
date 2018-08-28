using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModelRenderer.WaveFront.Model
{
    /// <summary>
    /// Represents a face.
    /// </summary>
    public class Face
    {
        private readonly List<Index> _indices;

		internal Face(Material material, List<Index> indices)
        {
            this.Material = material;
            this._indices = indices;
        }

		/// <summary>
		/// Gets the material.
		/// </summary>
		public Material Material { get; }


		/// <summary>
		/// Gets the indices.
		/// </summary>
		public ReadOnlyCollection<Index> Indices => _indices.AsReadOnly();
    }
}