using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ModelRenderer
{
	/// <summary>
	/// Custom vertex type for vertices that have just a
	/// position and a normal, without any texture coordinates.
	/// 
	/// This struct is borrowed from the Primitives3D sample.
	/// </summary>
	public struct VertexPositionNormal : IVertexType
	{
		public Vector3 Position;
		public Vector3 Normal;


		/// <summary>
		/// Constructor.
		/// </summary>
		public VertexPositionNormal(Vector3 position, Vector3 normal)
		{
			Position = position;
			Normal = normal;
		}

		/// <summary>
		/// A VertexDeclaration object, which contains information about the vertex
		/// elements contained within this struct.
		/// </summary>
		public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
		(
			new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
			new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
		);

		VertexDeclaration IVertexType.VertexDeclaration
		{
			get { return VertexPositionNormal.VertexDeclaration; }
		}

	}

	public struct VertexPositionColorNormal : IVertexType
	{
		public Vector3 Position;
		public Color Color;
		public Vector3 Normal;

		public readonly static VertexDeclaration VertexDeclaration
			= new VertexDeclaration(
				new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
				new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
				new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
			);

		public VertexPositionColorNormal(Vector3 pos, Color c, Vector3 n)
		{
			Position = pos;
			Color = c;
			Normal = n;
		}

		VertexDeclaration IVertexType.VertexDeclaration
		{
			get { return VertexDeclaration; }
		}
	}
}
