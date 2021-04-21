
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Networking;
using UnityEngine.Video;
using Newtonsoft.Json.Linq;

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

        public Texture2D placeholder;

        Sprite card_sprite;
        
        public Sprite card { get { return card_sprite; } set { card_sprite = value; } }

        public string slug;
        public string tokenID;

        public string name;
        public string animation;

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
                if (animation != ""){
                  var placementPlayer = placementObject.GetComponentInChildren<VideoPlayer>(false);
                  placementPlayer.enabled = true;
                  placementPlayer.url = animation;
                }
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

        public void setCard(string message){
            JObject m = JObject.Parse(message);
            tokenID =  m.GetValue("tokenIDOverlay").Value<string>();
            name = m.GetValue("nameOverlay").Value<string>();
            slug = m.GetValue("nameOverlay").Value<string>();
            animation = m.GetValue("animation").Value<string>();
            string URI = m.GetValue("URI").Value<string>();
            StartCoroutine(GetCardImage(URI));
        }
          IEnumerator GetCardImage(string uri){
    using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri))
    {
      // Request and wait for the desired page.
      yield return webRequest.SendWebRequest();
      if (webRequest.isNetworkError)
      {
        Debug.Log(webRequest.error);
      }
      else
      {
        Texture2D webTexture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture as Texture2D;
        if (isBogus(webTexture))
        {
          Sprite webSpriteBogus = SpriteFromTexture2D(placeholder);
          card = webSpriteBogus;
        } else {
        Sprite webSprite = SpriteFromTexture2D(webTexture);
        card = webSprite;
        }
      }
    }
  }
   public bool isBogus(Texture tex)
  {
    if (!tex) return true;

    byte[] png1 = (tex as Texture2D).EncodeToPNG();
    byte[] questionMarkPNG = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 8, 0, 0, 0, 8, 8, 2, 0, 0, 0, 75, 109, 41, 220, 0, 0, 0, 65, 73, 68, 65, 84, 8, 29, 85, 142, 81, 10, 0, 48, 8, 66, 107, 236, 254, 87, 110, 106, 35, 172, 143, 74, 243, 65, 89, 85, 129, 202, 100, 239, 146, 115, 184, 183, 11, 109, 33, 29, 126, 114, 141, 75, 213, 65, 44, 131, 70, 24, 97, 46, 50, 34, 72, 25, 39, 181, 9, 251, 205, 14, 10, 78, 123, 43, 35, 17, 17, 228, 109, 164, 219, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130, };

    return Equivalent(png1, questionMarkPNG);
  }

  public bool Equivalent(byte[] bytes1, byte[] bytes2)
  {
    if (bytes1.Length != bytes2.Length) return false;
    for (int i = 0; i < bytes1.Length; i++)
      if (!bytes1[i].Equals(bytes2[i])) return false;
    return true;
  }
   Sprite SpriteFromTexture2D(Texture2D texture)
  {

    return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
  }
    }
}