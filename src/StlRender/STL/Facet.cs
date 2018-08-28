using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;

namespace ModelRenderer.STL
{
	/// <summary>A representation of a facet which is defined by its location (<see cref="Vertices"/>) and directionality (<see cref="Normal"/>).</summary>
	public class Facet : IEquatable<Facet>, IEnumerable<VertexPositionNormal>
	{
		/// <summary>Indicates the directionality of the <see cref="Facet"/>.</summary>
		public Vector3 Normal { get; set; }

		/// <summary>Indicates the location of the <see cref="Facet"/>.</summary>
		public IList<VertexPositionNormal> Vertices { get; set; }

		/// <summary>Additional data attached to the facet.</summary>
		/// <remarks>Depending on the source of the STL, this could be used to indicate such things as the color of the <see cref="Facet"/>. This functionality only exists in binary STLs.</remarks>
		public UInt16 AttributeByteCount { get; set; }

		/// <summary>Creates a new, empty <see cref="Facet"/>.</summary>
		public Facet()
		{
			this.Vertices = new List<VertexPositionNormal>();
		}

		/// <summary>Creates a new <see cref="Facet"/> using the provided parameters.</summary>
		/// <param name="normal">The directionality of the <see cref="Facet"/>.</param>
		/// <param name="vertices">The location of the <see cref="Facet"/>.</param>
		/// <param name="attributeByteCount">Additional data to attach to the <see cref="Facet"/>.</param>
		public Facet(Vector3 normal, IEnumerable<VertexPositionNormal> vertices, UInt16 attributeByteCount)
			: this()
		{
			this.Normal = normal;
			this.Vertices = vertices.ToList();
			this.AttributeByteCount = attributeByteCount;
		}

		/// <summary>Writes the <see cref="Facet"/> as text to the <paramref name="writer"/>.</summary>
		/// <param name="writer">The writer to which the <see cref="Facet"/> will be written at the current position.</param>
		public void Write(StreamWriter writer)
		{
			writer.Write("\t");
			writer.WriteLine(this.ToString());
			writer.WriteLine("\t\touter loop");

			//Write each vertex.
			this.Vertices.ForEach(o =>
			{
				WriteVertex(writer, o.Position);
			});

			writer.WriteLine("\t\tendloop");
			writer.WriteLine("\tendfacet");
		}

		/// <summary>Writes the <see cref="Facet"/> as binary to the <paramref name="writer"/>.</summary>
		/// <param name="writer">The writer to which the <see cref="Facet"/> will be written at the current position.</param>
		public void Write(BinaryWriter writer)
		{
			//Write the normal.
			WriteVertex(writer, Normal);

			//Write each vertex.
			this.Vertices.ForEach(o => WriteVertex(writer, o.Position));

			//Write the attribute byte count.
			writer.Write(this.AttributeByteCount);
		}

		/// <summary>Writes the <see cref="Vertex"/> as text to the <paramref name="writer"/>.</summary>
		/// <param name="writer">The writer to which the <see cref="Vertex"/> will be written at the current position.</param>
		public void WriteVertex(StreamWriter writer, Vector3 v)
		{
			writer.WriteLine("\t\t\t{0}".Interpolate(v.ToString()));
		}

		/// <summary>Writes the <see cref="Vertex"/> as binary to the <paramref name="writer"/>.</summary>
		/// <param name="writer">The writer to which the <see cref="Vertex"/> will be written at the current position.</param>
		public void WriteVertex(BinaryWriter writer, Vector3 v)
		{
			writer.Write(v.X);
			writer.Write(v.Y);
			writer.Write(v.Z);
		}

		/// <summary>Returns the string representation of this <see cref="Facet"/>.</summary>
		public override string ToString()
		{
			return "facet {0}".Interpolate(this.Normal);
		}

		/// <summary>Determines whether or not this instance is the same as the <paramref name="other"/> instance.</summary>
		/// <param name="other">The <see cref="Facet"/> to which to compare.</param>
		public bool Equals(Facet other)
		{
			return (this.Normal.Equals(other.Normal)
					&& this.Vertices.Count == other.Vertices.Count
					&& this.Vertices.All((i, o) => o.Equals(other.Vertices[i])));
		}

		/// <summary>Iterates through the <see cref="Vertices"/> collection.</summary>
		public IEnumerator<VertexPositionNormal> GetEnumerator()
		{
			return this.Vertices.GetEnumerator();
		}

		/// <summary>Iterates through the <see cref="Vertices"/> collection.</summary>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>Reads a single <see cref="Facet"/> from the <paramref name="reader"/>.</summary>
		/// <param name="reader">The reader which contains a <see cref="Facet"/> to be read at the current position</param>
		public static Facet Read(StreamReader reader)
		{
			if (reader == null)
				return null;

			//Create the facet.
			Facet facet = new Facet();

			//Read the normal.
			if (TryReadVector3(reader, out var normal))
			{
				facet.Normal = normal;
			}
			else
			{
				return null;
			}

			//Skip the "outer loop".
			reader.ReadLine();

			//Read 3 vertices.
			facet.Vertices = Enumerable.Range(0, 3).Select(o =>
			{
				if (!TryReadVector3(reader, out var vector))
				{

				}

				return new VertexPositionNormal(vector
					, facet.Normal);
			}).ToList();

			//Read the "endloop" and "endfacet".
			reader.ReadLine();
			reader.ReadLine();

			return facet;
		}

		/// <summary>Reads a single <see cref="Facet"/> from the <paramref name="reader"/>.</summary>
		/// <param name="reader">The reader which contains a <see cref="Facet"/> to be read at the current position</param>
		public static Facet Read(BinaryReader reader)
		{
			if (reader == null)
				return null;

			//Create the facet.
			Facet facet = new Facet();

			//Read the normal.
			if (TryReadVector3(reader, out var normal))
			{
				facet.Normal = normal;
			}
			else
			{
				return null;
			}

			//Read 3 vertices.
			facet.Vertices = Enumerable.Range(0, 3).Select(o =>
			{
				if (!TryReadVector3(reader, out var vector))
				{
					
				}
				
				return new VertexPositionNormal(vector
					, facet.Normal);
			}).ToList();

			//Read the attribute byte count.
			facet.AttributeByteCount = reader.ReadUInt16();

			return facet;
		}

		private static bool TryReadVector3(StreamReader reader, out Vector3 normal)
		{
			normal = Vector3.Zero;
			const string regex = @"\s*(facet normal|vertex)\s+(?<X>[^\s]+)\s+(?<Y>[^\s]+)\s+(?<Z>[^\s]+)";
			const NumberStyles numberStyle = (NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);

			string data = null;
			float x, y, z;
			Match match = null;

			if (reader == null)
				return false;

			//Read the next line of data.
			data = reader.ReadLine();

			if (data == null)
				return false;

			//Ensure that the data is formatted correctly.
			match = Regex.Match(data, regex, RegexOptions.IgnoreCase);

			if (!match.Success)
				return false;

			//Parse the three coordinates.
			if (!float.TryParse(match.Groups["X"].Value, numberStyle, CultureInfo.InvariantCulture, out x))
				throw new FormatException("Could not parse X coordinate \"{0}\" as a decimal.".Interpolate(match.Groups["X"]));

			if (!float.TryParse(match.Groups["Y"].Value, numberStyle, CultureInfo.InvariantCulture, out y))
				throw new FormatException("Could not parse Y coordinate \"{0}\" as a decimal.".Interpolate(match.Groups["Y"]));

			if (!float.TryParse(match.Groups["Z"].Value, numberStyle, CultureInfo.InvariantCulture, out z))
				throw new FormatException("Could not parse Z coordinate \"{0}\" as a decimal.".Interpolate(match.Groups["Z"]));

			normal = new Vector3(x, y, z);
			return true;
		}

		public static bool TryReadVector3(BinaryReader reader, out Vector3 vector)
		{
			vector = default(Vector3);

			const int floatSize = sizeof(float);
			const int vertexSize = (floatSize * 3);

			if (reader == null)
				return false;

			//Read 3 floats.
			byte[] data = new byte[vertexSize];
			int bytesRead = reader.Read(data, 0, data.Length);

			//If no bytes are read then we're at the end of the stream.
			if (bytesRead == 0)
				return false;

			else if (bytesRead != data.Length)
				throw new FormatException("Could not convert the binary data to a vertex. Expected {0} bytes but found {1}.".Interpolate(vertexSize, bytesRead));

			//Convert the read bytes to their numeric representation.
			vector = new Vector3()
			{
				X = BitConverter.ToSingle(data, 0),
				Y = BitConverter.ToSingle(data, floatSize),
				Z = BitConverter.ToSingle(data, (floatSize * 2))
			};

			return true;
		}

		public IEnumerable<VertexPositionNormal> GetVertices()
		{
			return Vertices.ToArray();
		}
	}
}
