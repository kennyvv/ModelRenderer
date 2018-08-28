using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace ModelRenderer.WaveFront.Model
{
    /// <summary>
    /// Represent a scene of data loaded from an *.obj file.
    /// </summary>
    public class Scene
    {
        private readonly List<Vector3> vertices;
        private readonly List<Vector2> uvs;
        private readonly List<Vector3> normals;
        private readonly List<Face> ungroupedFaces;
        private readonly List<Group> groups; 
        private readonly List<Material> materials;
        private readonly string objectName;

        internal Scene(List<Vector3> vertices, List<Vector2> uvs, List<Vector3> normals, List<Face> ungroupedFaces, List<Group> groups, List<Material> materials,
            string objectName)
        {
            this.vertices = vertices;
            this.uvs = uvs;
            this.normals = normals;
            this.ungroupedFaces = ungroupedFaces;
            this.groups = groups;
            this.materials = materials;
            this.objectName = objectName;
        }

        /// <summary>
        /// Gets the vertices.
        /// </summary>
        public ReadOnlyCollection<Vector3> Vertices
        {
            get { return vertices.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the uvs.
        /// </summary>
        public ReadOnlyCollection<Vector2> Uvs
        {
            get { return uvs.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the normals.
        /// </summary>
        public ReadOnlyCollection<Vector3> Normals
        {
            get { return normals.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the faces which don't belong to any groups.
        /// </summary>
        public ReadOnlyCollection<Face> UngroupedFaces
        {
            get { return ungroupedFaces.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the groups.
        /// </summary>
        public ReadOnlyCollection<Group> Groups
        {
            get { return groups.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the materials.
        /// </summary>
        public ReadOnlyCollection<Material> Materials
        {
            get { return materials.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the name of the object in the file. This can be (and in many cases will be) null. 
        /// </summary>
        public string ObjectName { get { return objectName; } }
    }
}
