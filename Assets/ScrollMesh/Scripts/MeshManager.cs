using System.Collections.Generic;
using UnityEngine;

namespace ScrollMesh
{
    public class MeshManager : MonoBehaviour
    {
        private const int SIZE = 4096;
        public const float WIDTH = 0.25f;
        private const float HEIGHT = 2f;
        private const float BOTTOM = -20f;

        public const float WORLD_SIZE = SIZE * WIDTH;

        public Material _mGroundMaterial = null;
        public Material _mGroundDistantMaterial = null;
        public Material _mSnowMaterial = null;
        public Material _mPhysicsMaterial = null;

        private class MeshInstance
        {
            public GameObject _mGameObject = null;
            public MeshFilter _mMeshFilter = null;
            public Mesh _mMesh = null;
            public MeshRenderer _mMeshRenderer = null;
            public MeshCollider _mMeshCollider = null;
            public List<Vector3> _mVerts = new List<Vector3>();
            public List<Vector3> _mNormals = new List<Vector3>();
            public List<int> _mFaces = new List<int>();
            public List<Vector2> _mUvs = new List<Vector2>();

            public void Init()
            {
                _mGameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                _mMeshFilter = _mGameObject.GetComponent<MeshFilter>();
                _mMeshCollider = _mGameObject.GetComponent<MeshCollider>();
                _mMesh = new Mesh();
            }

            public void Apply(Material material, Material material2)
            {
                _mMesh.vertices = _mVerts.ToArray();
                _mMesh.normals = _mNormals.ToArray();
                _mMesh.uv = _mUvs.ToArray();
                _mMesh.triangles = _mFaces.ToArray();

                _mMeshRenderer = _mGameObject.GetComponent<MeshRenderer>();
                if (null != material2)
                {
                    _mMeshRenderer.materials = new Material[]
                        {
                        material,
                        material2,
                    };
                }
                else
                {
                    _mMeshRenderer.sharedMaterial = material;
                }

                _mMeshCollider.sharedMesh = _mMesh;
                _mMeshFilter.mesh = _mMesh;
            }
        }

        private MeshInstance _mGroundMesh = new MeshInstance();
        private MeshInstance _mSnowMesh = new MeshInstance();
        private MeshInstance _mPhysicsMesh = new MeshInstance();

        public TerrainData _mTerrain = null;

        private List<GameObject> _mFloor = new List<GameObject>();

        void CreateFaces(MeshInstance meshInstance, int index, bool useBarycentric)
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
            if (useBarycentric)
            {
                // barycentric uvs
                meshInstance._mUvs.Add(new Vector2(0, 0));
                meshInstance._mUvs.Add(new Vector2(1, 0));
                meshInstance._mUvs.Add(new Vector2(1, 1));
                meshInstance._mUvs.Add(new Vector2(0, 1));
            }
            else
            {
                // experiment
                const float scaleX = 25f / WORLD_SIZE;
                const float scaleY = 0.125f / HEIGHT;
                meshInstance._mUvs.Add(new Vector2(meshInstance._mVerts[f1].x * scaleX, meshInstance._mVerts[f1].y * scaleY));
                meshInstance._mUvs.Add(new Vector2(meshInstance._mVerts[f2].x * scaleX, meshInstance._mVerts[f2].y * scaleY));
                meshInstance._mUvs.Add(new Vector2(meshInstance._mVerts[f3].x * scaleX, meshInstance._mVerts[f3].y * scaleY));
                meshInstance._mUvs.Add(new Vector2(meshInstance._mVerts[f4].x * scaleX, meshInstance._mVerts[f4].y * scaleY));
            }

            Vector3 dir = 0.5f * (Vector3.up + Vector3.right);
            meshInstance._mNormals.Add(dir);
            meshInstance._mNormals.Add(dir);
            meshInstance._mNormals.Add(dir);
            meshInstance._mNormals.Add(dir);
        }

        void CreateGround(MeshInstance meshInstance, float x, int index)
        {
            float height1 = _mTerrain.GetHeight(x);
            float height2 = _mTerrain.GetHeight(x + WIDTH);

            // test slope
            meshInstance._mVerts.Add(new Vector3(x, BOTTOM, 0));
            meshInstance._mVerts.Add(new Vector3(x + WIDTH, BOTTOM, 0));
            meshInstance._mVerts.Add(new Vector3(x + WIDTH, height2, 0));
            meshInstance._mVerts.Add(new Vector3(x, height1, 0));

            CreateFaces(meshInstance, index, false);
        }

        void CreateSnow(MeshInstance meshInstance, float x, int index)
        {
            const float coverage = 0.5f;
            float height1 = _mTerrain.GetHeight(x) + coverage;
            float height2 = _mTerrain.GetHeight(x + WIDTH) + coverage;
            float base1 = height1 - coverage;
            float base2 = height2 - coverage;

            // test slope
            float depth = -0.01f;
            meshInstance._mVerts.Add(new Vector3(x, base1, depth));
            meshInstance._mVerts.Add(new Vector3(x + WIDTH, base2, depth));
            meshInstance._mVerts.Add(new Vector3(x + WIDTH, height2, depth));
            meshInstance._mVerts.Add(new Vector3(x, height1, depth));

            CreateFaces(meshInstance, index, false);
        }

        void CreatePhysics(MeshInstance meshInstance, float x, int index)
        {
            float base1 = _mTerrain.GetHeight(x);
            float base2 = _mTerrain.GetHeight(x + WIDTH);

            float height = Mathf.Min(base1, base2);
            GameObject floor = new GameObject("Floor");
            floor.AddComponent<Floor>()._mHeight = Mathf.Max(base1, base2);
            _mFloor.Add(floor);
            BoxCollider boxCollider = floor.AddComponent<BoxCollider>();
            Vector3 pos = new Vector3(x + WIDTH * 0.5f, height * 0.5f - 0.1f, 0);
            boxCollider.transform.position = pos;
            boxCollider.transform.localScale = new Vector3(WIDTH * 0.1f, height, 1);

            // test slope
            float depth1 = -WIDTH;
            float depth2 = WIDTH;

            meshInstance._mVerts.Add(new Vector3(x, base1, depth1));
            meshInstance._mVerts.Add(new Vector3(x + WIDTH, base2, depth1));
            meshInstance._mVerts.Add(new Vector3(x + WIDTH, base2, depth2));
            meshInstance._mVerts.Add(new Vector3(x, base1, depth2));

            CreateFaces(meshInstance, index, true);
        }

        // Use this for initialization
        void Start()
        {
            _mGroundMesh.Init();
            _mSnowMesh.Init();
            _mPhysicsMesh.Init();

            int index = 0;
            for (int i = -SIZE; i < SIZE; ++i)
            {
                float x = (i + 2) * WIDTH;

                CreateGround(_mGroundMesh, x, index);
                CreateSnow(_mSnowMesh, x, index);
                CreatePhysics(_mPhysicsMesh, x, index);

                index += 4;
            }

            _mGroundMesh.Apply(_mGroundMaterial, _mGroundDistantMaterial);
            _mSnowMesh.Apply(_mSnowMaterial, null);
            _mPhysicsMesh.Apply(_mPhysicsMaterial, null);
        }
    }
}
