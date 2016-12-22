using UnityEngine;

namespace ScrollMesh
{
    public class TerrainData : MonoBehaviour
    {
        public float _mHeight = 1f;

        public float GetHeight(float x)
        {
            x = x * 0.1f;
            return _mHeight + _mHeight * Mathf.Lerp(Mathf.Cos(x), Mathf.Sin(x), Mathf.Cos(10 * x));
        }
    }
}
