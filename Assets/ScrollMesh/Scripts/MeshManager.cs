using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScrollMesh
{
    public class MeshManager : MonoBehaviour
    {
        public Material _mMaterial = null;
        private Mesh _mMesh = null;

        // Use this for initialization
        void Start()
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
            _mMesh = meshFilter.sharedMesh;

            _mMesh.triangles = new int[0];

            List<Vector3> verts = new List<Vector3>();
            List<int> faces = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            const float width = 0.25f;
            int index = 0;
            for (int i = 0; i < 10; ++i)
            {
                float x = i * width;

                // test slope
                float val = i + 1;
                float height = 5f / (float)val;

                Vector3 p1 = new Vector3(x, 0, 0);
                Vector3 p2 = new Vector3(x + width, 0, 0);
                Vector3 p3 = new Vector3(x + width, height, 0);
                Vector3 p4 = new Vector3(x, height, 0);
                verts.Add(p1);
                verts.Add(p2);
                verts.Add(p3);
                verts.Add(p4);
                int f1 = index + 0;
                int f2 = index + 1;
                int f3 = index + 2;
                int f4 = index + 3;
                // face 1
                faces.Add(f3);
                faces.Add(f2);
                faces.Add(f1);
                // face 2
                faces.Add(f1);
                faces.Add(f4);
                faces.Add(f3);
                index += 4;
                // barycentric uvs
                uvs.Add(new Vector2(0,0));
                uvs.Add(new Vector2(1,0));
                uvs.Add(new Vector2(1,1));
                uvs.Add(new Vector2(0,1));
            }

            _mMesh.vertices = verts.ToArray();
            _mMesh.triangles = faces.ToArray();
            _mMesh.uv = uvs.ToArray();

            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            mr.sharedMaterial = _mMaterial;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
