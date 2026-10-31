using System.Collections.Generic;
using UnityEngine;

namespace BlindSpot
{
    public class SecurityCameraSpawner : MonoBehaviour
    {
        [System.Serializable]
        private class CameraSpawn
        {
            [field: SerializeField] public GameObject Prefab { get; private set; }
            [field: SerializeField] public Transform SpawnPoint { get; private set; }
        }

        [SerializeField] private CameraSpawn[] _cameras;
        [SerializeField, Min(0)] private int _minCameras = 2;
        [SerializeField, Min(0)] private int _maxCameras = 5;


        private void Start()
        {
            SpawnCameras();
        }

        private void SpawnCameras()
        {
            if (_cameras == null || _cameras.Length == 0)
            {
                return;
            }

            List<CameraSpawn> availableCameras = new List<CameraSpawn>();

            foreach (CameraSpawn camera in _cameras)
            {
                if (camera != null &&
                    camera.Prefab != null &&
                    camera.SpawnPoint != null)
                {
                    availableCameras.Add(camera);
                }
            }

            if (availableCameras.Count == 0)
            {
                return;
            }
            
            // Перемешиваем точки алгоритмом Фишера - Йетса
            for (int i = availableCameras.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);

                (availableCameras[i], availableCameras[randomIndex]) =
                    (availableCameras[randomIndex], availableCameras[i]);
            }

            int min = Mathf.Clamp(_minCameras, 0, availableCameras.Count);
            int max = Mathf.Clamp(_maxCameras, min, availableCameras.Count);

            // для int верхняя граница Random.Range исключается
            int cameraCount = Random.Range(min, max + 1);

            for (int i = 0; i < cameraCount; i++)
            {
                CameraSpawn camera = availableCameras[i];

                Instantiate(
                    camera.Prefab,
                    camera.SpawnPoint.position,
                    camera.SpawnPoint.rotation,
                    transform);
            }
        }
    }
}
