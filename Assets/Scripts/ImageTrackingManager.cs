using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackingManager : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject prefab1; // Prefab for Image1
    public GameObject prefab2; // Prefab for Image2

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdatePrefab(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            UpdatePrefab(trackedImage);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            if (spawnedPrefabs.ContainsKey(trackedImage.referenceImage.name))
            {
                Destroy(spawnedPrefabs[trackedImage.referenceImage.name]);
                spawnedPrefabs.Remove(trackedImage.referenceImage.name);
            }
        }
    }

    void UpdatePrefab(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;
        GameObject prefab = null;

        switch (imageName)
        {
            case "HaruUrara":
                prefab = prefab1;
                break;
            case "ResqueRabbit":
                prefab = prefab2;
                break;
        }

        if (prefab != null)
        {
            if (!spawnedPrefabs.ContainsKey(imageName))
            {
                spawnedPrefabs[imageName] = Instantiate(prefab, trackedImage.transform.position, trackedImage.transform.rotation);
            }
            else
            {
                spawnedPrefabs[imageName].transform.position = trackedImage.transform.position;
                spawnedPrefabs[imageName].transform.rotation = trackedImage.transform.rotation;
            }
        }
    }
}