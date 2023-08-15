using UnityEngine;
using Unity.Robotics.ROSTCPConnector;

using RosMessageTypes.Geometry;



public class simple_car_node : MonoBehaviour
{
     const float LINEAR_X_GAIN = 3;
     const float LINEAR_Y_GAIN = 1;
     const float ANGULAR_GAIN  = 6;

     public Rigidbody cube;

     void Start()
     {
       ROSConnection.instance.Subscribe<TwistMsg>("/cmd_vel", simpleCar_move);
     }

     void simpleCar_move(TwistMsg Msg)
     {
        cube.velocity = transform.forward * (float)Msg.linear.x * LINEAR_X_GAIN + transform.right * (float)Msg.linear.y * LINEAR_Y_GAIN;
        cube.angularVelocity = new Vector3(0, (float)Msg.angular.z * ANGULAR_GAIN, 0);
     }
}
