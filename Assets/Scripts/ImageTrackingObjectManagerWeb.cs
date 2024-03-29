using System;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class ImageTrackingObjectManagerWeb : MonoBehaviour
{
    #region Class Variables
    #region Image Related Information
    [Header("Image Related Info")]
    //[SerializeField]
    //[Tooltip("Image manager on the AR Session Origin")]
    //ARTrackedImageManager m_ImageManager;

    ///// <summary>
    ///// Get the <c>ARTrackedImageManager</c>
    ///// </summary>
    //public ARTrackedImageManager ImageManager
    //{
    //    get => m_ImageManager;
    //    set => m_ImageManager = value;
    //}

    //[SerializeField]
    //[Tooltip("Reference Image Library")]
    //private XRReferenceImageLibrary m_ImageLibrary;

    ///// <summary>
    ///// Get the <c>XRReferenceImageLibrary</c>
    ///// </summary>
    //public XRReferenceImageLibrary ImageLibrary
    //{
    //    get => m_ImageLibrary;
    //    set => m_ImageLibrary = value;
    //}

    private int m_NumberOfTrackedImages;

    private static Guid s_FirstImageGUID;
    private static Guid s_SecondImageGUID;
    #endregion

    #region Prefab References
    [Header("Prefab References")]
    [SerializeField]
    [Tooltip("Prefab for tracked 1 m_SpriteImage")]
    private GameObject m_PlantPrefab;

    /// <summary>
    /// Get the one prefab
    /// </summary>
    public GameObject PlantPrefab
    {
        get => m_PlantPrefab;
        set => m_PlantPrefab = value;
    }

    /// <summary>
    /// Spawned plant prefab
    /// </summary>
    private GameObject m_SpawnedPlantPrefab;

    /// <summary>
    /// get the spawned one prefab
    /// </summary>
    public GameObject SpawnedPlantPrefab
    {
        get => m_SpawnedPlantPrefab;
        set => m_SpawnedPlantPrefab = value;
    }

    [SerializeField]
    [Tooltip("Prefab for tracked 2 m_SpriteImage")]
    private GameObject m_FrogPrefab;

    /// <summary>
    /// get the two prefab
    /// </summary>
    public GameObject FrogPrefab
    {
        get => m_FrogPrefab;
        set => m_FrogPrefab = value;
    }

    /// <summary>
    /// Spawned frog prefab
    /// </summary>
    private GameObject m_SpawnedFrogPrefab;

    /// <summary>
    /// get the spawned two prefab
    /// </summary>
    public GameObject SpawnedFrogPrefab
    {
        get => m_SpawnedFrogPrefab;
        set => m_SpawnedFrogPrefab = value;
    }

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

    private void Start()
    {
        DisableARCanvas();
    }

    #endregion

    #region Custom Methods
    public void EnableARCanvas()
    {
        m_ARCanvas.SetActive(true);
    }

    public void DisableARCanvas()
    {
        m_ARCanvas.SetActive(false);
        testText.text = "";
    }

    public void DisableARCanvasIfARMode()
    {
        if (m_ViewManager.m_ViewState == ViewManager.ViewState.AR)
        {
            DisableARCanvas();
        }
    }

    public void UpdateInfo()
    {
        GameObject arObject = resourceManager.imageTracker.GetTrackedObject();

        GameObject modelObject = resourceManager.modelPairs[arObject.name];
        string plantName = modelObject.name;
        if (plantName != null)
        {
            resourceManager.UpdateMapLink(plantName);
            resourceManager.UpdatePlantDialogue(plantName);
            m_ModelViewManager.SetModel(modelObject);
        }
    }
    #endregion
    #endregion
}
