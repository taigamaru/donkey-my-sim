using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;

public class wheel_car_node : MonoBehaviour
{
     public List<AxleInfo2> axleInfos;
     public float maxMotorTorque;
     public float maxSteeringAngle;

     const float LINEAR_X_GAIN = 3;
     const float LINEAR_Y_GAIN = 1;
     const float ANGULAR_GAIN  = 8;

     public Rigidbody cube;

          
     public void ApplyLocalPositionToVisuals(WheelCollider collider) {
         Transform visualWheel = collider.transform.GetChild(0);
         Vector3 position;
         Quaternion rotation;
         collider.GetWorldPose(out position, out rotation);
         visualWheel.transform.position = position;
         visualWheel.transform.rotation = rotation;
     }
     public void FixedUpdate() {
         //float motor = maxMotorTorque * Input.GetAxis("Vertical");
         //float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
         foreach (AxleInfo2 axleInfo in axleInfos) {
             if (axleInfo.steering) {
                 //axleInfo.leftWheel.steerAngle = steering;
                 //axleInfo.rightWheel.steerAngle = steering;
             }
             if (axleInfo.motor) {
                // axleInfo.leftWheel.motorTorque = motor;
                 //axleInfo.rightWheel.motorTorque = motor;
             }
             //ApplyLocalPositionToVisuals(axleInfo.rightWheel);
             //ApplyLocalPositionToVisuals(axleInfo.leftWheel);
         }
     }

     void Start()
     {
       ROSConnection.instance.Subscribe<TwistMsg>("/cmd_vel", wheelCar_move);
     }

     void wheelCar_move(TwistMsg Msg)
     {
         cube.velocity = transform.forward * (float)Msg.linear.x * LINEAR_X_GAIN + transform.right * (float)Msg.linear.y * LINEAR_Y_GAIN;
         cube.angularVelocity = new Vector3(0, (float)Msg.angular.z * ANGULAR_GAIN, 0);

         float motor = maxMotorTorque * (float)Msg.linear.x * (float)1.5;
         float steering = maxSteeringAngle * (float)Msg.angular.z * (float)1.2;
         
         foreach (AxleInfo2 axleInfo in axleInfos) {
             if (axleInfo.steering) {
                 axleInfo.leftWheel.steerAngle = steering;
                 axleInfo.rightWheel.steerAngle = steering;
             }
             if (axleInfo.motor) {
                 axleInfo.leftWheel.motorTorque = motor;
                 axleInfo.rightWheel.motorTorque = motor;
             }
             ApplyLocalPositionToVisuals(axleInfo.rightWheel);
             ApplyLocalPositionToVisuals(axleInfo.leftWheel);
         }
     }
}

[System.Serializable]
public class AxleInfo2 {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; 
    public bool steering;
}

