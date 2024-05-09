using UnityEngine;


/// <summary>
/// Class that manages the model view
/// </summary>
public class ModelViewManager : MonoBehaviour
{
    #region Class Variables
    #region Plant Instantiation Variables
    /// <summary>
    /// Transform to instantiate the plant prefab when switching to model viewer mode
    /// </summary>
    [Header("Plant Instantiation")]
    [SerializeField]
    [Tooltip("Transform in which the plant prefab will be instantiated at")]
    Transform m_PlantInstantiationPoint;

    /// <summary>
    /// Plant prefab to instantiate when switching to model viewer mode
    /// </summary>
    [HideInInspector]
    public GameObject m_PlantPrefab;

    /// <summary>
    /// Actual instantiated plant prefab, used for resizing and such
    /// </summary>
    [HideInInspector]
    public GameObject m_InstantiatedPlantPrefab;

    /// <summary>
    /// Boolean stating whether or not the model has been set in the model view mode
    /// </summary>
    public bool m_ModelSet;

    /// <summary>
    /// Height of the mesh of the model
    /// </summary>
    [HideInInspector]
    public float modelHeight;

    /// <summary>
    /// Width of the mesh of the model
    /// </summary>
    [HideInInspector]
    public float modelWidth;
    #endregion

    #region Camera and Zoom Variables
    [Header("Camera and Zoom References")]
    /// <summary>
    /// Reference to the model camera object used for the model viewer
    /// </summary>
    [SerializeField]
    [Tooltip("Reference to the model camera, the one that handles the model view")]
    private GameObject m_ModelCamera;

    /// <summary>
    /// Transform used to zoom in to the model, zoom interpolates camera to this inner zoom position
    /// </summary>
    [Tooltip("Reference to the transform representing the inner bound of the camera zoom")]
    public Transform m_CamZoomInnerBound;

    /// <summary>
    /// Transform used to zoom out away from the model, zoom interpolates camera to this outer zoom position
    /// </summary>
    [Tooltip("Reference to the transform representing the outer bound of the camera zoom")]
    public Transform m_CamZoomOuterBound;

    /// <summary>
    /// Distance where the camera begins from the plant
    /// </summary>
    [Tooltip("Distance where the camera begins from the plant")]
    public float m_StartZoomDistance = 2f;

    /// <summary>
    /// Minimum distance the camera can be from the plant
    /// </summary>
    [Tooltip("Minimum distance the camera can be from the plant")]
    public float m_MinZoomDistance = .5f;

    /// <summary>
    /// Maximum distance the camera can be from the plant
    /// </summary>
    [Tooltip("Maximum distance the camera can be from the plant")]
    public float m_MaxZoomDistance = 5f;

    #endregion

    #region Script References
    /// <summary>
    /// Reference to the ViewManager class
    /// </summary>
    [Header("Script References")]
    [SerializeField]
    [Tooltip("Reference to the ViewManager m_Instance")]
    ViewManager m_ViewManager;

    /// <summary>
    /// Reference to the model camera input class
    /// </summary>
    [SerializeField]
    [Tooltip("Reference to the ModelCameraInput m_Instance")]
    private ModelCameraInput m_ModelCameraInput;

    #endregion
    #endregion

    #region Methods
    /// <summary>
    /// Sets the plant prefab in the model viewer and all of the camera/zoom related objects
    /// </summary>
    /// <param name="plantPrefab"></param>
    public void SetModel(GameObject plantPrefab)
    {
        //if (!m_ModelSet)
        //{
        //Get the prefab to set in the model view and instantiate it

        Vector3 offset = Vector3.zero;

        Vector3 rotationOffset = Vector3.zero;

        //NEED TO ADD MORE CONDITIONS FOR THE OTHER PLANTS TO GET THEM IN THE PROPER SPOT
        //if (plantPrefab.name == "SnowTrillium")
        //{
        //    offset = new Vector3(-1.125f, 0, .385f);
        //    //rotationOffset = new Vector3(90, 0, 0);
        //}
        m_PlantPrefab = plantPrefab;
        m_InstantiatedPlantPrefab = Instantiate(m_PlantPrefab, m_PlantInstantiationPoint.position + offset, 
            m_PlantInstantiationPoint.rotation, m_PlantInstantiationPoint);

        m_InstantiatedPlantPrefab.transform.Rotate(rotationOffset);

        MeshRenderer meshRenderer = m_InstantiatedPlantPrefab.GetComponentInChildren<MeshRenderer>();
        modelHeight = meshRenderer.bounds.size.y;
        modelWidth = meshRenderer.bounds.size.x;

        //Set the model camera and the zoom bound transforms according to the height of the prefab mesh
        m_ModelCamera.transform.position = m_InstantiatedPlantPrefab.transform.position - new Vector3(0, -modelHeight, modelHeight * m_StartZoomDistance);
        m_CamZoomInnerBound.position = m_InstantiatedPlantPrefab.transform.position - new Vector3(0, -5, modelHeight * m_MinZoomDistance);
        m_CamZoomOuterBound.position = m_InstantiatedPlantPrefab.transform.position - new Vector3(0, -5, modelHeight * m_MaxZoomDistance);

        m_ModelCamera.transform.rotation = Quaternion.LookRotation(m_InstantiatedPlantPrefab.transform.position -
            m_ModelCamera.transform.position,Vector3.up);

        //Calculate the distances from the prefab for rotation purposes
        m_ModelCameraInput.m_CamDistanceFromModel = Mathf.Abs(m_ModelCamera.transform.position.z - m_InstantiatedPlantPrefab.transform.position.z);
        m_ModelCameraInput.m_InnerBoundDistanceFromModel = Mathf.Abs(m_CamZoomInnerBound.position.z - m_InstantiatedPlantPrefab.transform.position.z);
        m_ModelCameraInput.m_OuterBoundDistanceFromModel = Mathf.Abs(m_CamZoomOuterBound.position.z - m_InstantiatedPlantPrefab.transform.position.z);

        //Last set things
        m_ModelCamera.transform.eulerAngles = new Vector3(0, 0, 5);
        m_ModelSet = true;
        //}
    }

    /// <summary>
    /// Clears the model set in model viewer mode
    /// </summary>
    public void ClearModel()
    {
        if (m_ModelSet && m_ViewManager.m_ViewState == ViewManager.ViewState.AR)
        {
            //Sets values to null and destroys them
            Destroy(m_InstantiatedPlantPrefab);
            m_InstantiatedPlantPrefab = null;
            m_PlantPrefab = null;
            m_ModelSet = false;
        }
    }
    #endregion

}
