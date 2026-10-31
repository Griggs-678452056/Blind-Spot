using UnityEngine;

namespace BlindSpot
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class FieldOfViewVisualizer : MonoBehaviour
    {
        [SerializeField] private SecurityCameraRotation _securityCamera;
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField, Range(10, 120)] private int _rayCount = 60;
        private Transform _viewPoint;
        private Mesh _mesh;

        private void Awake()
        {
            _mesh = new Mesh
            {
                name = "Field of View"
            };

            GetComponent<MeshFilter>().mesh = _mesh;
        }

        private void Start()
        {
            if (_securityCamera != null)
            {
                _viewPoint = _securityCamera.ViewPoint;
            }
        }

        private void LateUpdate()
        {
            DrawFieldOfView();
        }

        private void DrawFieldOfView()
        {
            if (_securityCamera == null || _viewPoint == null)
            {
                return;
            }

            float viewAngle = _securityCamera.PlayerViewAngle;
            float viewRange = _securityCamera.PlayerViewRange;

            Vector3[] vertices = new Vector3[_rayCount + 2];
            int[] triangles = new int[_rayCount * 3];

            vertices[0] = Vector3.zero;

            Vector3 origin = _viewPoint.position;
            Vector3 center = transform.position;

            for (int i = 0; i <= _rayCount; i++)
            {
                float angle = -viewAngle / 2 + (viewAngle / _rayCount) * i;

                Vector3 horizontalDirection = Quaternion.Euler(0, angle, 0) * transform.forward;

                Vector3 targetPoint = center + horizontalDirection * viewRange;

                Vector3 rayDirection = (targetPoint - origin).normalized;

                float distance = Vector3.Distance(origin, targetPoint);

                Vector3 worldPoint = targetPoint;

                if (Physics.Raycast(origin, rayDirection, out RaycastHit hit, distance, _obstacleMask))
                {
                    worldPoint = new Vector3(hit.point.x, center.y, hit.point.z);
                }

                vertices[i + 1] = transform.InverseTransformPoint(worldPoint);
            }

            for (int i = 0; i < _rayCount; i++)
            {
                int index = i * 3;

                triangles[index] = 0;
                triangles[index + 1] = i + 1;
                triangles[index + 2] = i + 2;
            }

            _mesh.Clear();
            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();
        }

        private void OnDestroy()
        {
            if (_mesh != null)
            {
                Destroy(_mesh);
            }
        }
    }
}
