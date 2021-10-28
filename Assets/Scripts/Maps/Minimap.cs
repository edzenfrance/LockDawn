using UnityEngine;
using UnityEngine.SceneManagement;

public class Minimap : MonoBehaviour
{
    [SerializeField] private RectTransform playerInMap;
    [SerializeField] private RectTransform map2dEnd;
    [SerializeField] private Transform map3dParent;
    [SerializeField] private Transform map3dEnd;

    [Header("ObjectToDisableOnFirstRun")]
    [SerializeField] private GameObject mapViewObject;

    private Vector3 normalized, mapped;

    string sceneName;

    private void Awake()
    {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();
        // Retrieve the name of this scene.
        sceneName = currentScene.name;
        if (sceneName == "Stage1")
        {
            playerInMap = GameObject.Find("Map2DPlayer").GetComponent<RectTransform>();
            map2dEnd = GameObject.Find("Map2DEnd").GetComponent<RectTransform>();
            map3dParent = GameObject.Find("Map3D").GetComponent<Transform>();
            map3dEnd = GameObject.Find("Map3DEnd").GetComponent<Transform>();

            mapViewObject = GameObject.Find("MapView");
            mapViewObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();
        // Retrieve the name of this scene.
        sceneName = currentScene.name;
        if (sceneName == "Stage1")
        {
            normalized = Divide(
                map3dParent.InverseTransformPoint(this.transform.position),
                map3dEnd.position - map3dParent.position
            );
            normalized.y = normalized.z;
            mapped = Multiply(normalized, map2dEnd.localPosition);
            mapped.z = 0;
            playerInMap.localPosition = mapped;
        }
    }

    private static Vector3 Divide(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }

    private static Vector3 Multiply(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
}