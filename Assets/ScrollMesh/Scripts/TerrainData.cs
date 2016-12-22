using UnityEngine;

namespace ScrollMesh
{
    public class TerrainData : MonoBehaviour
    {
        public float _mHeight = 1f;

        public AnimationCurve _mAnimationCurve = null;

        public float GetHeight(float x)
        {
            float t = x / (float)MeshManager.WORLD_SIZE * 0.5f + 0.5f;
            float val = _mAnimationCurve.Evaluate(t);
            return _mHeight * val;
        }
    }
}
