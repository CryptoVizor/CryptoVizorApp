
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.Interaction.Toolkit.AR
{

    /// <summary>
    /// UnityEvent that responds to changes of hover and selection by this interactor.
    /// </summary>
    [Serializable]
    public class CustomPlacingEvent : UnityEvent<ARPlacementInteractable, GameObject> { }
    
    /// <summary>
    /// Controls the placement of Andy objects via a tap gesture.
    /// </summary>
    public class CustomPlacing : ARBaseGestureInteractable
    {
        [SerializeField]
        [Tooltip("A GameObject to place when a raycast from a user touch hits a plane.")]
        GameObject m_PlacementPrefab;
        /// <summary>
        /// A GameObject to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject placementPrefab { get { return m_PlacementPrefab; } set { m_PlacementPrefab = value; } }

        public GameObject deleteButton;

        public GameObject okButton;

        Sprite card_sprite;
        
        public Sprite card { get { return card_sprite; } set { card_sprite = value; } }

        public string slug;
        public string tokenID;
        

        public string name;

        public GetTokensData canvasData;

        [SerializeField, Tooltip("Called when the this interactable places a new GameObject in the world.")]
        ARObjectPlacedEvent m_OnObjectPlaced = new ARObjectPlacedEvent();
        /// <summary>Gets or sets the event that is called when the this interactable places a new GameObject in the world.</summary>
        public ARObjectPlacedEvent onObjectPlaced { get { return m_OnObjectPlaced; } set { m_OnObjectPlaced = value; } }
        
        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
        static GameObject s_TrackablesObject;

        /// <summary>
        /// Returns true if the manipulation can be started for the given gesture.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        /// <returns>True if the manipulation can be started.</returns>
        protected override bool CanStartManipulationForGesture(TapGesture gesture)
        {

            if (card == null)
            {
                return false;
            }

            if (gesture.StartPosition.IsPointOverUIObject())
                return false;
            
            // Allow for test planes
            if (gesture.TargetObject == null || gesture.TargetObject.layer == 9)
                return true;

            return false;
        }

        /// <summary>
        /// Function called when the manipulation is ended.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        protected override void OnEndManipulation(TapGesture gesture)
        {
            if (gesture.WasCancelled)
                return;

            // If gesture is targeting an existing object we are done.
            // Allow for test planes
            if (gesture.TargetObject != null && gesture.TargetObject.layer != 9)
                return;
            
            // Raycast against the location the player touched to search for planes.
            if (GestureTransformationUtility.Raycast(gesture.StartPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                var hit = s_Hits[0];
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if (Vector3.Dot(Camera.main.transform.position - hit.pose.position,
                        hit.pose.rotation * Vector3.up) < 0)
                    return;

                // Instantiate placement prefab at the hit pose.

                GameObject transformObject =  new GameObject();
                transformObject.transform.position = hit.pose.position;
                Vector3 targetPostition = new Vector3(Camera.main.transform.position.x,
                                                    transformObject.transform.position.y,
                                                    Camera.main.transform.position.z);
                transformObject.transform.LookAt(targetPostition);
                transformObject.transform.Rotate(0,180,0);

                var placementObject = Instantiate(placementPrefab, hit.pose.position, transformObject.transform.rotation);
                var placementSprite = placementObject.GetComponentInChildren<SpriteRenderer>(false);
                placementSprite.sprite = card;
                var placementMetadata = placementObject.GetComponentInChildren<cardMetadata>(false);
                placementMetadata.tokenID = tokenID;
                placementMetadata.name = name;
                placementMetadata.slug = slug;
                ShowDeleteButton ShowDeleteButtonScript = placementObject.GetComponent<ShowDeleteButton>();
                ShowDeleteButtonScript.deleteButton = deleteButton;
                ShowDeleteButtonScript.okButton = okButton;
                if (!canvasData.placedTokens.ContainsKey(slug))
                {
                    Debug.Log("dictionary added");
                    canvasData.placedTokens.Add(slug, new Dictionary<string, bool>());
                }
                canvasData.placedTokens[slug][tokenID] = true;
                
                // Create anchor to track reference point and set it as the parent of placementObject.
                // TODO: this should update with a reference point for better tracking.
                var anchorObject = new GameObject("PlacementAnchor");
                anchorObject.transform.position = hit.pose.position;
                anchorObject.transform.rotation = transformObject.transform.rotation;
                placementObject.transform.parent = anchorObject.transform;

                // Find trackables object in scene and use that as parent
                if (s_TrackablesObject == null)
                    s_TrackablesObject = GameObject.Find("Trackables");
                if (s_TrackablesObject != null)
                    anchorObject.transform.parent = s_TrackablesObject.transform;

                card = null;
            }
        }
    }
}