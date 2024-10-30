using UnityEngine;
using System.IO;
using System.Text;

public class CameraLogger : MonoBehaviour
{
    public float logInterval = 0.1f;
    private float timer = 0f;
    private StringBuilder logBuilder;
    private string filePath;
    private Camera mainCamera;

    void Start()
    {
        logBuilder = new StringBuilder();
        filePath = Path.Combine(Application.persistentDataPath, "CameraTransformLog.txt");
        Debug.Log($"Log file will be saved at: {filePath}");

        // Find the camera in the scene
        mainCamera = FindObjectOfType<Camera>();
        if (mainCamera == null)
        {
            Debug.LogError("No camera found in the scene!");
        }
    }

    void Update()
    {
        if (mainCamera == null) return;

        timer += Time.deltaTime;
        if (timer >= logInterval)
        {
            LogCameraTransform();
            timer = 0f;
        }
    }

    void LogCameraTransform()
    {
        Transform cameraTransform = mainCamera.transform;
        string logEntry = $"{Time.time},{cameraTransform.position.x},{cameraTransform.position.y},{cameraTransform.position.z}," +
                          $"{cameraTransform.rotation.eulerAngles.x},{cameraTransform.rotation.eulerAngles.y},{cameraTransform.rotation.eulerAngles.z}\n";
        logBuilder.Append(logEntry);
    }

    void OnApplicationQuit()
    {
        File.WriteAllText(filePath, logBuilder.ToString());
        Debug.Log($"Camera transform log saved to: {filePath}");
    }
}