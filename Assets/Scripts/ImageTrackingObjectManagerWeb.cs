using System;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class ImageTrackingObjectManagerWeb : MonoBehaviour
{
    #region Class Variables
    
    #region Prefab References
    [Header("Prefab References")]
    /// <summary>
    /// Name of the plant that is active
    /// </summary>
    [ReadOnly]
    [Tooltip("Active plant that helps update text")]
    public string m_ActivePlant;
    #endregion

    #region AR Canvas
    /// <summary>
    /// AR Canvas Object
    /// </summary>
    [Header("AR Canvas")]
    [SerializeField]
    [Tooltip("GameObject that is the AR Canvas")]
    private GameObject m_ARCanvas;

    public TextMeshProUGUI testText;
    #endregion


    #region Script References
    /// <summary>
    /// ViewManager reference
    /// </summary>
    [Header("Script References")]
    [SerializeField]
    [Tooltip("Reference to the View Manager")]
    private ViewManager m_ViewManager;

    /// <summary>
    /// ModelViewManager reference
    /// </summary>
    [SerializeField]
    [Tooltip("Reference to the Model View Manager")]
    private ModelViewManager m_ModelViewManager;

    /// <summary>
    /// ResourceManager reference
    /// </summary>
    [SerializeField]
    private ResourceManager resourceManager;
    #endregion
    #endregion

    #region Methods
    #region Unity Methods

    /// <summary>
    /// Unity method called on object's enabling/scene start
    /// </summary>
    private void Start()
    {
        DisableARCanvas();
    }

    #endregion

    #region Custom Methods
    /// <summary>
    /// Function that enables the AR Canvas
    /// </summary>
    public void EnableARCanvas()
    {
        m_ARCanvas.SetActive(true);
    }

    /// <summary>
    /// Function that disables the AR Canvas and resets the test text
    /// </summary>
    public void DisableARCanvas()
    {
        m_ARCanvas.SetActive(false);
        testText.text = "";
    }

    /// <summary>
    /// Function that only disables the AR Canvas if it is in AR mode
    /// </summary>
    public void DisableARCanvasIfARMode()
    {
        if (m_ViewManager.m_ViewState == ViewManager.ViewState.AR)
        {
            DisableARCanvas();
        }
    }

    /// <summary>
    /// Function that updates info when an image is tracked
    /// </summary>
    public void UpdateInfo()
    {
        GameObject arObject = resourceManager.imageTracker.GetTrackedObject();

        GameObject modelObject = resourceManager.modelPairs[arObject.name];
        string plantName = modelObject.name;
        if (plantName != null)
        {
            resourceManager.UpdateMapLink(plantName);
            resourceManager.UpdatePlantDialogue(plantName);
            m_ModelViewManager.SetModel(modelObject, resourceManager.rotationOffsets[plantName]);
        }
    }

    public void SetTestText(string text)
    {
        testText.text = text;
    }
    #endregion
    #endregion
}
