using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;
using RosMessageTypes.Geometry;
/// <summary>
///
/// </summary>
public class RosRotPublisher : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "rot";

    // The game object
    public GameObject cube;
    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 0.02f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<Vector3Msg>(topicName);
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
         
            Vector3Msg cubeRos = new Vector3Msg(
                cube.transform.localEulerAngles.x,
                cube.transform.localEulerAngles.y,
                cube.transform.localEulerAngles.z
            );

            // Finally send the message to server_endpoint.py running in ROS
            ros.Publish(topicName, cubeRos);

            timeElapsed = 0;
        }
    }
}
