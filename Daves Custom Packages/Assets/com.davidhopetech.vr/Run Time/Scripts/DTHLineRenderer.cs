using UnityEngine;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    [ExecuteInEditMode]
    public class DTHLineRenderer : MonoBehaviour
    {
        public Vector3[] points;

        internal LineRenderer _lr;

        private void Awake()
        {
            _lr = GetComponent<LineRenderer>();
        }

        void Update()
        {
            _lr.positionCount = points.Length;
            for (var i = 0; i < points.Length; i++)
            {
                var pos = transform.TransformPoint(points[i]);
                _lr.SetPosition(i, pos);
            }
        }
    }
}
