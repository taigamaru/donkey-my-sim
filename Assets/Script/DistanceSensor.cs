using UnityEngine;
using System.Collections;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;
using RosMessageTypes.Std;

// レーザー距離計 https://ftvoid.com/blog/post/1005
public class DistanceSensor : MonoBehaviour {
    
    ROSConnection ros;
    public string topicName = "distance";

    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 0.02f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;
    
    public const float NOTHING = -1;    // 計測不能

    public float maxDistance = 30;      // 計測可能な最大距離
    public float distance;              // 計測距離

    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<Float32Msg>(topicName);
    }

    // 距離計測
    void Update() {
        // 前方ベクトル計算
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        // 距離計算
        RaycastHit hit;
        if ( Physics.Raycast(transform.position, fwd, out hit, maxDistance) ) {
            distance = hit.distance;
        } else {
            distance = NOTHING;
        }
        
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
         
            Float32Msg distance_pub = new Float32Msg(
                distance
            );

            // Finally send the message to server_endpoint.py running in ROS
            ros.Publish(topicName, distance_pub);

            timeElapsed = 0;
        }
    }
}
