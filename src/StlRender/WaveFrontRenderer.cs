using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModelRenderer.WaveFront;
using ModelRenderer.WaveFront.Model;

namespace ModelRenderer
{
	public class WaveFrontRenderer : IRenderer
	{
		private class Areas
		{
			public int Start;
			public int Length;
		//	public BasicEffect Effect;
			public BoundingSphere Box;
			public bool HasTransparency = false;
			public bool Render = true;
			public Material Material { get; set; }
		}

		private class Group
		{
			public string Name;
			public Areas[] Areas;

			public VertexBuffer Buffer;
		}

		private Group[] Groups { get; set; }
		//private VertexBuffer Buffer { get; set; }
		private BasicEffect _effect;
		public WaveFrontRenderer(GraphicsDevice device, string file)
		{
			_effect = new BasicEffect(device);
			_effect.EnableDefaultLighting();
			_effect.PreferPerPixelLighting = false;

			var result = FileFormatObj.Load(device, file, true);
			foreach (var msg in result.Messages)
			{
				if (msg.Exception == null)
				{
					Console.WriteLine($"File: {msg.FileName} - Details: {msg.Details}");
				}
				else
				{
					Console.WriteLine($"File: {msg.FileName} - Exception: {msg.Exception.ToString()}");
				}
				
			}
			var vertices = result.Model.Vertices;
			var normals = result.Model.Normals;
			var uvs = result.Model.Uvs;

			/*Dictionary<string, BasicEffect> materials = new Dictionary<string, BasicEffect>();
			foreach (var mat in result.Model.Materials)
			{
				if (materials.ContainsKey(mat.Name)) continue;

				var effect = new BasicEffect(device);
				{
					SetBasicEffect(ref effect, mat);
				}

				materials.Add(mat.Name, effect);
			}*/

			List<Group> groups = new List<Group>();

			foreach (var g in result.Model.Groups)
			{
				int textureCount = 0;
				List<VertexPositionNormalTexture> textures = new List<VertexPositionNormalTexture>();
				Group gr = new Group();
				gr.Name = "";
				gr.Areas = new Areas[g.Faces.Count];

				for (var i = 0; i < g.Faces.Count; i++)
				{
					var face = g.Faces[i];
					
					VertexPositionNormalTexture[] faceVertices = new VertexPositionNormalTexture[face.Indices.Count];
					for (var index = 0; index < face.Indices.Count; index++)
					{
						var indice = face.Indices[index];

						var vert = vertices[indice.Vertex];

						var v = new VertexPositionNormalTexture()
						{
							Position = vert
						};

						if (indice.Normal.HasValue)
						{
							var normal = normals[indice.Normal.Value];
							v.Normal = normal;
						}

						if (indice.Uv.HasValue)
						{
							var uv = uvs[indice.Uv.Value];
							v.TextureCoordinate = uv;
						}

						faceVertices[index] = v;
					}

					int startIndex = textureCount;
					textures.AddRange(faceVertices);
					textureCount += faceVertices.Length;

				//	BasicEffect effect;
					//if (!materials.TryGetValue(face.Material.Name, out effect))
					//{
					//	effect = new BasicEffect(device);
					//	SetBasicEffect(ref effect, face.Material);
					//}

					Areas a = new Areas();
					a.Material = face.Material;
					//a.Effect = effect;
					a.Length = faceVertices.Length;
					a.Start = startIndex;
					a.Box = BoundingSphere.CreateFromPoints(faceVertices.Select(x => x.Position));

					if (face.Material.Transparency.HasValue)
					{
						if (face.Material.Transparency.Value < 1f)
						{
							a.HasTransparency = true;
						}
					}

					gr.Areas[i] = a;
				}

				gr.Buffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, textures.Count, BufferUsage.WriteOnly);
				gr.Buffer.SetData(textures.ToArray());

				groups.Add(gr);
			}

			Groups = groups.ToArray();
		//	Buffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, textures.Count, BufferUsage.WriteOnly);
			//Buffer.SetData(textures.ToArray());
		}

		private void SetBasicEffect(ref BasicEffect effect, Material mat)
		{
			effect.TextureEnabled = true;

			if (mat.Transparency.HasValue)
			{
				effect.Alpha = mat.Transparency.Value;
			}

			if (mat.TextureMapAmbient != null)
			{
				effect.Texture = mat.TextureMapAmbient.Image;
			}

			else if (mat.TextureMapDiffuse != null)
			{
				effect.Texture = mat.TextureMapDiffuse.Image;
			}

			else if (mat.TextureMapSpecular != null)
			{
				effect.Texture = mat.TextureMapSpecular.Image;
			}

			else if (mat.TextureMapSpecularHighlight != null)
			{
				effect.Texture = mat.TextureMapSpecularHighlight.Image;
			}

			else if (mat.TextureMapAlpha != null)
			{
				effect.Texture = mat.TextureMapAlpha.Image;
			}

			else if (mat.TextureMapBump != null)
			{
				effect.Texture = mat.TextureMapBump.Image;
			}
			else
			{
				effect.TextureEnabled = false;
			}

			effect.SpecularColor = mat.Specular.ToVector3();
			effect.AmbientLightColor = mat.Ambient.ToVector3();
			effect.DiffuseColor = mat.Diffuse.ToVector3();
		}

		private IEnumerable<Group> _renderedGroups = new Group[0];
		private IEnumerable<Group> _renderedAlphaGroups = new Group[0];
		//TODO: Actually use raytracing...
		private void Raytrace()
		{
			BoundingFrustum frustum = new BoundingFrustum(_effect.View * _effect.Projection);

			List<Group> renderable = new List<Group>();
			List<Group> renderableAlpha = new List<Group>();
			foreach (var g in Groups)
			{
				bool hasRenderable = false;
				bool transparent = false;
				foreach (var area in g.Areas)
				{
					if (frustum.Contains(area.Box.Transform(_effect.World)) == ContainmentType.Disjoint)
					{
						area.Render = false;
					}
					else
					{
						area.Render = true;
						hasRenderable = true;
					}

					if (area.HasTransparency || area.Material.TextureMapAlpha != null) transparent = true;
				}

				if (hasRenderable)
				{
					if (transparent)
					{
						renderableAlpha.Add(g);
					}
					else
					{
						renderable.Add(g);
					}
				}
			}

			_renderedGroups = renderable;
			_renderedAlphaGroups = renderableAlpha;
		}

		public void Update(GameTime gameTime, Matrix world, Matrix view, Matrix projection)
		{
			_effect.View = view;
			_effect.Projection = projection;
			_effect.World = world;

			Raytrace();
		}

		private void Render(GraphicsDevice device, IEnumerable<Group> groups)
		{
			foreach (var group in groups)
			{
				device.SetVertexBuffer(group.Buffer);

				foreach (var area in group.Areas)
				{
					if (!area.Render) continue;

					SetBasicEffect(ref _effect, area.Material);
					foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
					{
						pass.Apply();
						device.DrawPrimitives(PrimitiveType.TriangleList, area.Start, area.Length);
					}
				}
			}
		}

		public void Render(GraphicsDevice device)
		{
			device.BlendState = BlendState.Opaque;
			device.DepthStencilState = DepthStencilState.Default;
			device.RasterizerState = RasterizerState.CullClockwise;
			
			Render(device, _renderedGroups);

			device.BlendState = BlendState.AlphaBlend;
			Render(device, _renderedAlphaGroups);
		}
	}
}
