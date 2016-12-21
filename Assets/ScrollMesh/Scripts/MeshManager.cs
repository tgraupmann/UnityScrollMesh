using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScrollMesh
{
    public class MeshManager : MonoBehaviour
    {
        const float WIDTH = 0.25f;
        const float HEIGHT = 2;

        public Material _mMaterial = null;
        private Mesh _mMesh = null;
        private List<Vector3> _mVerts = new List<Vector3>();
        private List<int> _mFaces = new List<int>();
        private List<Vector2> _mUvs = new List<Vector2>();

        float GetHeight(float x)
        {
            return HEIGHT + HEIGHT * Mathf.Cos(x);
        }

        void CreateFaces(int index)
        {
            int f1 = index + 0;
            int f2 = index + 1;
            int f3 = index + 2;
            int f4 = index + 3;
            // face 1
            _mFaces.Add(f3);
            _mFaces.Add(f2);
            _mFaces.Add(f1);
            // face 2
            _mFaces.Add(f1);
            _mFaces.Add(f4);
            _mFaces.Add(f3);
            // barycentric uvs
            _mUvs.Add(new Vector2(0, 0));
            _mUvs.Add(new Vector2(1, 0));
            _mUvs.Add(new Vector2(1, 1));
            _mUvs.Add(new Vector2(0, 1));
        }

        void CreateGround(float x, int index)
        {
            float height1 = GetHeight(x);
            float height2 = GetHeight(x + WIDTH);

            // test slope
            _mVerts.Add(new Vector3(x, 0, 0));
            _mVerts.Add(new Vector3(x + WIDTH, 0, 0));
            _mVerts.Add(new Vector3(x + WIDTH, height2, 0));
            _mVerts.Add(new Vector3(x, height1, 0));

            CreateFaces(index);
        }

        void CreateSnow(float x, int index)
        {
            float height1 = GetHeight(x);
            float height2 = GetHeight(x + WIDTH);

            // test slope
            _mVerts.Add(new Vector3(x, 0, 0));
            _mVerts.Add(new Vector3(x + WIDTH, 0, 0));
            _mVerts.Add(new Vector3(x + WIDTH, height2, 0));
            _mVerts.Add(new Vector3(x, height1, 0));

            CreateFaces(index);
        }

        // Use this for initialization
        void Start()
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
            _mMesh = meshFilter.sharedMesh;

            // clear faces
            _mMesh.triangles = new int[0];     

            int index = 0;
            for (int i = -12; i < 24; ++i)
            {
                float x = i * WIDTH;

                CreateGround(x, index);
                CreateSnow(x, index);

                index += 4;
            }

            _mMesh.vertices = _mVerts.ToArray();
            _mMesh.triangles = _mFaces.ToArray();
            _mMesh.uv = _mUvs.ToArray();

            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            mr.sharedMaterial = _mMaterial;
        }
    }
}
