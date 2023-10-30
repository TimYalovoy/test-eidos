using UnityEngine;
using UnityEngine.UI;
using Eyes;
using System.Collections;
using System.Collections.Generic;
using SaveSystem;

namespace Following
{
    public class FollowingSystem : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Button stopFollowing;
        [Header("Target")]
        [SerializeField] private Transform target;
        [Header("React objects")]
        [SerializeField] private Eye[] eyes = null;
        [SerializeField] private Head.Head head = null;
        [SerializeField] private Body.Body body = null;

        [Header("Body parts rotation speed")]
        [Range(0f, 50f)]
        [SerializeField] private float headRotationSpeed = 0f;
        [Range(0f, 50f)]
        [SerializeField] private float bodyRotationSpeed = 0f;

        private float generatedRandomTime = .5f;

        private List<Vector3> randomPointsOnSphereSector = new List<Vector3>();

        private Save _saveSystem;

        private void Awake()
        {
            _saveSystem = FindObjectOfType<Save>();
            _saveSystem.SaveIsLoaded += SaveSystem_SaveIsLoaded;

            if (eyes is null)
            {
                throw new System.NullReferenceException("[FollowingSystem] - NullReferenceException: check the FollowingSystem Object in scene, and set links to Eye object");
            }
            foreach (var eye in eyes)
            {
                eye.FollowFor(target);
                stopFollowing.onClick.AddListener(eye.ToggleFollowingLogic);
                eye.BoundsIsReached += Eye_BoundsIsReached;
            }

            if (head is not null)
            {
                head.BoundsIsReached += Head_BoundsIsReached;
                stopFollowing.onClick.AddListener(head.ToggleFollowingLogic);
                head.RotationSpeed = headRotationSpeed;
            }

            if (body is not null)
            {
                stopFollowing.onClick.AddListener(body.ToggleFollowingLogic);
                body.RotationSpeed = bodyRotationSpeed;
            }
        }

        private void SaveSystem_SaveIsLoaded(SaveData obj)
        {
            CreateSphereSectorAndFillingPossibleRandomPointsForEyes();
        }

        private void OnDestroy()
        {
            _saveSystem.SaveIsLoaded -= SaveSystem_SaveIsLoaded;
            
            StopAllCoroutines();

            stopFollowing.onClick.RemoveAllListeners();
            
            foreach (var eye in eyes)
            {
                eye.BoundsIsReached -= Eye_BoundsIsReached;
            }

            if (head is not null)
            {
                head.BoundsIsReached -= Head_BoundsIsReached;
            }
        }

        private void Start()
        {
            CreateSphereSectorAndFillingPossibleRandomPointsForEyes();

            StartCoroutine(GenerateRandomPointForEyes());
        }

        public void CreateSphereSectorAndFillingPossibleRandomPointsForEyes()
        {
            foreach (var eye in eyes)
            {
                randomPointsOnSphereSector.Clear();

                float startAzimuth = 0.0f + eye.transform.rotation.eulerAngles.x;
                float endAzimuth = 180.0f + eye.transform.rotation.eulerAngles.x;
                float startElevation = 120.0f + eye.transform.rotation.eulerAngles.y;
                float endElevation = 240.0f + eye.transform.rotation.eulerAngles.y;

                float startAzimuthRad = startAzimuth * Mathf.Deg2Rad;
                float endAzimuthRad = endAzimuth * Mathf.Deg2Rad;
                float startElevationRad = startElevation * Mathf.Deg2Rad;
                float endElevationRad = endElevation * Mathf.Deg2Rad;

                for (float elevation = startElevationRad; elevation <= endElevationRad; elevation += 0.05f)
                {
                    for (float azimuth = startAzimuthRad; azimuth <= endAzimuthRad; azimuth += 0.05f)
                    {
                        float x = Mathf.Sin(elevation) * Mathf.Cos(azimuth);
                        float y = Mathf.Sin(elevation) * Mathf.Sin(azimuth);
                        float z = Mathf.Cos(elevation);

                        Vector3 point = new Vector3(x, y, z);

                        var corrPoint = eye.transform.position + point;
                        randomPointsOnSphereSector.Add(corrPoint);
                    }
                }
            }
        }

        private IEnumerator GenerateRandomPointForEyes()
        {
            yield return new WaitForSeconds(0.7f);

            while (true)
            {
                generatedRandomTime = Random.Range(0.5f, 3f);

                yield return new WaitForSeconds(generatedRandomTime);

                var randomIndex = Random.Range(0, randomPointsOnSphereSector.Count);
                var rand = randomPointsOnSphereSector[randomIndex];
                foreach (var eye in eyes)
                {
                    eye.RandomPoint = rand;
                }
            }
        }

        public void ToggleFollowing()
        {
            stopFollowing.OnPointerClick(new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current));

            foreach (var eye in eyes)
                if (eye.IsFollowing)
                    eye.BoundsIsReached += Eye_BoundsIsReached;
                else
                    eye.BoundsIsReached -= Eye_BoundsIsReached;
            
        }

        private void Eye_BoundsIsReached(Transform target)
        {
            head.LookAtTarget(target);
        }

        private void Head_BoundsIsReached(Transform target)
        {
            body.LookAtTarget(target);
        }
    }
}
