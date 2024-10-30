using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class TrajectoryManager : MonoBehaviour
{
    public CharacterController playerController;
    public Camera playerCamera;
    private string filePath;
    private bool isReplaying = false;
    private List<PlayerState> trajectory = new List<PlayerState>();
    private int currentReplayIndex = 0;

    [System.Serializable]
    private class PlayerState
    {
        public Vector3 position;
        public Quaternion rotation;
        public Quaternion cameraRotation;

        public PlayerState(Vector3 pos, Quaternion rot, Quaternion camRot)
        {
            position = pos;
            rotation = rot;
            cameraRotation = camRot;
        }
    }

    void Start()
    {
        filePath = Application.persistentDataPath + "/trajectory.txt";

        if (File.Exists(filePath))
        {
            LoadTrajectory();
            isReplaying = true;
        }
    }

    void Update()
    {
        if (isReplaying)
        {
            ReplayTrajectory();
        }
        else
        {
            RecordTrajectory();
        }
    }

    void RecordTrajectory()
    {
        trajectory.Add(new PlayerState(playerController.transform.position, playerController.transform.rotation, playerCamera.transform.rotation));
    }

    void SaveTrajectory()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (PlayerState state in trajectory)
            {
                writer.WriteLine($"{state.position.x},{state.position.y},{state.position.z}|{state.rotation.x},{state.rotation.y},{state.rotation.z},{state.rotation.w}|{state.cameraRotation.x},{state.cameraRotation.y},{state.cameraRotation.z},{state.cameraRotation.w}");
            }
        }
    }

    void LoadTrajectory()
    {
        trajectory.Clear();
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 3)
                {
                    string[] positionParts = parts[0].Split(',');
                    string[] rotationParts = parts[1].Split(',');
                    string[] cameraRotationParts = parts[2].Split(',');

                    Vector3 position = new Vector3(
                        float.Parse(positionParts[0]),
                        float.Parse(positionParts[1]),
                        float.Parse(positionParts[2]));

                    Quaternion rotation = new Quaternion(
                        float.Parse(rotationParts[0]),
                        float.Parse(rotationParts[1]),
                        float.Parse(rotationParts[2]),
                        float.Parse(rotationParts[3]));

                    Quaternion cameraRotation = new Quaternion(
                        float.Parse(cameraRotationParts[0]),
                        float.Parse(cameraRotationParts[1]),
                        float.Parse(cameraRotationParts[2]),
                        float.Parse(cameraRotationParts[3]));

                    trajectory.Add(new PlayerState(position, rotation, cameraRotation));
                }
            }
        }
    }

    void ReplayTrajectory()
    {
        if (currentReplayIndex < trajectory.Count)
        {
            PlayerState state = trajectory[currentReplayIndex];
            playerController.enabled = false;
            playerController.transform.position = state.position;
            playerController.transform.rotation = state.rotation;
            playerCamera.transform.rotation = state.cameraRotation;
            playerController.enabled = true;
            currentReplayIndex++;
        }
        else
        {
            isReplaying = false;
            currentReplayIndex = 0;
        }
    }

    void OnApplicationQuit()
    {
        if (!isReplaying)
        {
            SaveTrajectory();
        }
    }
}