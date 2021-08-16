//cameraFacingBillboard.cs v02
//by Neil Carter (NCarter)
//modified by Juan Castaneda (juanelo)
//
//added in-between GRP object to perform rotations on
//added auto-find main camera
//added un-initialized state, where script will do nothing
using UnityEngine;
using System.Collections;


public class CameraFacingBillboard : MonoBehaviour
{

  public Camera m_Camera;
  void Awake()
  {
      m_Camera = Camera.main;
  }

  //Orient the camera after all movement is completed this frame to avoid jittering
  void LateUpdate()
  {
     transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);
  }
}