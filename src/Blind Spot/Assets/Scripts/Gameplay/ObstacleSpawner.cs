using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlindSpot
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _obstaclePrefabs;

        [Header("Количество укрытий")]
        [SerializeField, Min(0)] private int _minObstacles = 3;
        [SerializeField, Min(0)] private int _maxObstacles = 8;

        [Header("Границы генерации")]
        [SerializeField] private Vector2 _xRange = new(-6.5f, 6.5f);
        [SerializeField] private Vector2 _zRange = new(-6.5f, 6.5f);
        [SerializeField] private float _ySpawn = 0;

        [Header("Проверка свободного места")]
        [SerializeField, Min(0.1f)] private float _checkRadius = 1.2f;
        [SerializeField, Min(1)] private int _maxAttempts = 30;
        [SerializeField] private LayerMask _blockingMask;

        private void Start()
        {
            SpawnObstacles();
        }

        private void SpawnObstacles()
        {
            if(_obstaclePrefabs == null || _obstaclePrefabs.Length == 0)
            {
                return;
            }

            int obstacleLayer = LayerMask.NameToLayer("Obstacle");

            if (obstacleLayer == -1)
            {
                Debug.Log("Слой Obstacle не найден");
                return;
            }

            List<GameObject> validPrefabs = new();

            foreach (GameObject prefab in _obstaclePrefabs)
            {
                if(prefab != null)
                {
                    validPrefabs.Add(prefab);
                }
            }

            if (validPrefabs.Count == 0)
            {
                return;
            }

            int min = Mathf.Min(_minObstacles, _maxObstacles);
            int max = Mathf.Max(_minObstacles, _maxObstacles);

            int obstacleCount = Random.Range(min, max + 1);

            for (int i = 0; i< obstacleCount; i++)
            {
                bool spawned = false;

                for (int attempt = 0; attempt < _maxAttempts; attempt++)
                {
                    Vector3 position = new(
                        Random.Range(_xRange.x, _xRange.y),
                        _ySpawn,
                        Random.Range(_zRange.x, _zRange.y));

                    Vector3 checkPosition = position + Vector3.up;

                    if (Physics.CheckSphere(
                        checkPosition,
                        _checkRadius,
                        _blockingMask,
                        QueryTriggerInteraction.Collide))
                    {
                        continue;
                    }

                    GameObject prefab = validPrefabs[Random.Range(0, validPrefabs.Count)];

                    GameObject instance = Instantiate(
                        prefab,
                        position,
                        prefab.transform.rotation,
                        transform);

                    ConfigureObstacles(instance, obstacleLayer);

                    spawned = true;
                    break;
                }

                if (spawned == false)
                {
                    Debug.LogWarning("Не удалось найти место для укрытия");
                }
            }
        }

        private void ConfigureObstacles(GameObject obstacle, int obstacleLayer)
        {
            Transform[] children = obstacle.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in children)
            {
                child.gameObject.layer = obstacleLayer;
            }

            Collider[] colliders = obstacle.GetComponentsInChildren<Collider>(true);

            if (colliders.Length == 0)
            {
                obstacle.AddComponent<BoxCollider>();
            }
        }
    }
}
