using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UnityEngine.XR.Interaction.Toolkit.AR
{
    /// <summary>
    /// Controls the selection of an object through Tap gesture.
    /// </summary>
    public class ShowDeleteButton : ARBaseGestureInteractable
    {

        bool m_GestureSelected;

        /// <summary>
        /// The visualization game object that will become active when the object is selected.
        /// </summary>        
        [SerializeField, Tooltip("The GameObject that will become active when the object is selected.")]
        GameObject m_SelectionVisualization;
        public GameObject selectionVisualization { get { return m_SelectionVisualization; } set { m_SelectionVisualization = value; } }

        public SpriteRenderer card;
        public GameObject deleteButton;
        public GameObject okButton;

        public Material materialDefault;

         public Material materialSelected;
        
        /// <summary>
        /// Determines if this interactable can be selected by a given interactor.
        /// </summary>
        /// <param name="interactor">Interactor to check for a valid selection with.</param>
        /// <returns>True if selection is valid this frame, False if not.</returns>
        public override bool IsSelectableBy(XRBaseInteractor interactor)
        {
            if (!(interactor is ARGestureInteractor))
                return false;
            
            return m_GestureSelected;
        }

        /// <summary>
        /// Returns true if the manipulation can be started for the given gesture.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        /// <returns>True if the manipulation can be started.</returns>
        protected override bool CanStartManipulationForGesture(TapGesture gesture)
        {
            return true;
        }

        /// <summary>
        /// Function called when the manipulation is ended.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        protected override void OnEndManipulation(TapGesture gesture)
        {
            if (gesture.WasCancelled)
                return;
            if (gestureInteractor == null)
                return;

            if (gesture.TargetObject == gameObject)
            {
                // Toggle selection
                m_GestureSelected = !m_GestureSelected;
            }
            else
                m_GestureSelected = false;
        }

        /// <summary>This method is called by the interaction manager 
        /// when the interactor first initiates selection of an interactable.</summary>
        /// <param name="interactor">Interactor that is initiating the selection.</param>
        protected override void OnSelectEnter(XRBaseInteractor interactor) 
        {
            base.OnSelectEnter(interactor);
            
            if (m_SelectionVisualization != null)
                m_SelectionVisualization.SetActive(true);

            if (card != null)
                card.material = materialSelected;


            if (deleteButton != null)
                deleteButton.SetActive(true);
                DeleteButtonLogic deleteButtonLogic = deleteButton.GetComponent<DeleteButtonLogic>();
                deleteButtonLogic.Card = m_SelectionVisualization.transform.parent.gameObject;

            if (okButton != null)
                okButton.SetActive(true);
        }

        /// <summary>This method is called by the interaction manager 
        /// when the interactor ends selection of an interactable.</summary>
        /// <param name="interactor">Interactor that is ending the selection.</param>
        protected override void OnSelectExit(XRBaseInteractor interactor) 
        {
            base.OnSelectExit(interactor);
            
            if (m_SelectionVisualization != null)
                m_SelectionVisualization.SetActive(false);

            if (card != null)
                card.material = materialDefault;

            if (deleteButton != null)
                deleteButton.SetActive(false);

            if (okButton != null)
                okButton.SetActive(false);
        }
    }
}
