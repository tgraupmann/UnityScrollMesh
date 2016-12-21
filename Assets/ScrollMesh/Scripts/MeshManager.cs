using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScrollMesh
{
    public class MeshManager : MonoBehaviour
    {
        const float WIDTH = 0.25f;
        const float HEIGHT = 2;

        public Material _mGroundMaterial = null;
        public Material _mSnowMaterial = null;

        private class MeshInstance
        {
            public GameObject _mGameObject = null;
            public MeshFilter _mMeshFilter = null;
            public Mesh _mMesh = null;
            public MeshRenderer _mMeshRenderer = null;
            public List<Vector3> _mVerts = new List<Vector3>();
            public List<int> _mFaces = new List<int>();
            public List<Vector2> _mUvs = new List<Vector2>();

            public void Init()
            {
                _mGameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                _mMeshFilter = _mGameObject.GetComponent<MeshFilter>();
                _mMesh = Object.Instantiate(_mMeshFilter.sharedMesh);
                _mMeshFilter.sharedMesh = _mMesh;

                // clear faces
                _mMesh.triangles = new int[0];
            }

            public void Apply(Material material)
            {
                _mMesh.vertices = _mVerts.ToArray();
                _mMesh.triangles = _mFaces.ToArray();
                _mMesh.uv = _mUvs.ToArray();

                _mMeshRenderer = _mGameObject.GetComponent<MeshRenderer>();
                _mMeshRenderer.sharedMaterial = material;
            }
        }

        private MeshInstance _mGroundMesh = new MeshInstance();
        private MeshInstance _mSnowMesh = new MeshInstance();

        float GetHeight(float x)
        {
            return HEIGHT + HEIGHT * Mathf.Cos(x);
        }

        void CreateFaces(MeshInstance meshInstance, int index)
        {
            int f1 = index + 0;
            int f2 = index + 1;
            int f3 = index + 2;
            int f4 = index + 3;
            // face 1
            meshInstance._mFaces.Add(f3);
            meshInstance._mFaces.Add(f2);
            meshInstance._mFaces.Add(f1);
            // face 2
            meshInstance._mFaces.Add(f1);
            meshInstance._mFaces.Add(f4);
            meshInstance._mFaces.Add(f3);
            // barycentric uvs
            meshInstance._mUvs.Add(new Vector2(0, 0));
            meshInstance._mUvs.Add(new Vector2(1, 0));
            meshInstance._mUvs.Add(new Vector2(1, 1));
            meshInstance._mUvs.Add(new Vector2(0, 1));
        }

        void CreateGround(MeshInstance meshInstance, float x, int index)
        {
            float height1 = GetHeight(x);
            float height2 = GetHeight(x + WIDTH);

            // test slope
            meshInstance._mVerts.Add(new Vector3(x, 0, 0));
            meshInstance._mVerts.Add(new Vector3(x + WIDTH, 0, 0));
            meshInstance._mVerts.Add(new Vector3(x + WIDTH, height2, 0));
            meshInstance._mVerts.Add(new Vector3(x, height1, 0));

            CreateFaces(meshInstance, index);
        }

        void CreateSnow(MeshInstance meshInstance, float x, int index)
        {
            const float coverage = 0.5f;
            float height1 = GetHeight(x) + coverage;
            float height2 = GetHeight(x + WIDTH) + coverage;
            float base1 = height1 - coverage;
            float base2 = height2 - coverage;

            // test slope
            float depth = -0.1f;
            meshInstance._mVerts.Add(new Vector3(x, base1, depth));
            meshInstance._mVerts.Add(new Vector3(x + WIDTH, base2, depth));
            meshInstance._mVerts.Add(new Vector3(x + WIDTH, height2, depth));
            meshInstance._mVerts.Add(new Vector3(x, height1, depth));

            CreateFaces(meshInstance, index);
        }

        // Use this for initialization
        void Start()
        {
            _mGroundMesh.Init();
            _mSnowMesh.Init();

            int index = 0;
            for (int i = -36; i < 36; ++i)
            {
                float x = i * WIDTH;

                CreateGround(_mGroundMesh, x, index);
                CreateSnow(_mSnowMesh, x, index);

                index += 4;
            }

            _mGroundMesh.Apply(_mGroundMaterial);
            _mSnowMesh.Apply(_mSnowMaterial);
        }
    }
}
