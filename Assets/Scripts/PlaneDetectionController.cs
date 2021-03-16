using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using TMPro;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// This example demonstrates how to toggle plane detection,
    /// and also hide or show the existing planes.
    /// </summary>
    [RequireComponent(typeof(ARPlaneManager))]
    public class PlaneDetectionController : MonoBehaviour
    {
        [Tooltip("The UI Text element used to display plane detection messages.")]
        [SerializeField]
        TextMeshProUGUI m_TogglePlaneDetectionText;

        /// <summary>
        /// The UI Text element used to display plane detection messages.
        /// </summary>
        public TextMeshProUGUI togglePlaneDetectionText
        {
            get { return m_TogglePlaneDetectionText; }
            set { m_TogglePlaneDetectionText = value; }
        }

        public Image ButtonImage;

        public Sprite ButtonSpriteOn;
        public Sprite ButtonSpriteOff;

        /// <summary>
        /// Toggles plane detection and the visualization of the planes.
        /// </summary>
        public void TogglePlaneDetection()
        {
            m_ARPlaneManager.enabled = !m_ARPlaneManager.enabled;

            string planeDetectionMessage = "";
            Color32 planeDetectionColor;
            Sprite planeDetectionSprite;
            if (m_ARPlaneManager.enabled)
            {
                Debug.Log("m_ARPlaneManager.enabled");
                planeDetectionMessage = "ON";
                SetAllPlanesActive(true);
                planeDetectionColor = new Color32(255, 0, 0, 255);
                planeDetectionSprite = ButtonSpriteOn;
            }
            else
            {
                planeDetectionMessage = "OFF";
                SetAllPlanesActive(false);
                planeDetectionColor = new Color32(255, 255, 255, 255);
                planeDetectionSprite = ButtonSpriteOff;
            }

            if (togglePlaneDetectionText != null)
                togglePlaneDetectionText.SetText(planeDetectionMessage);
                togglePlaneDetectionText.color = planeDetectionColor;
                ButtonImage.sprite = planeDetectionSprite;
        }

        /// <summary>
        /// Iterates over all the existing planes and activates
        /// or deactivates their <c>GameObject</c>s'.
        /// </summary>
        /// <param name="value">Each planes' GameObject is SetActive with this value.</param>
        void SetAllPlanesActive(bool value)
        {
            foreach (var plane in m_ARPlaneManager.trackables)
                plane.gameObject.SetActive(value);
        }

        void Awake()
        {
            m_ARPlaneManager = GetComponent<ARPlaneManager>();
        }

        ARPlaneManager m_ARPlaneManager;
    }
}