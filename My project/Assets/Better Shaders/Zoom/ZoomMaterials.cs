using UnityEngine;

//[System.Serializable, CreateAssetMenu(fileName = "ZoomMaterials", menuName = "ZoomMaterials")]
public class ZoomMaterials : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    
    //---Accessing the data from the Pass---
    static ZoomMaterials _instance;

    public static ZoomMaterials Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

            //get the material called "DoG" from the resources folder
            Material customEffect = Resources.Load<Material>("Zoom");
     
            //convert the material to a CustomPostProcessingMaterials
            _instance = CreateInstance<ZoomMaterials>();

            //set the material to the instance
            _instance.customEffect = customEffect;


            return _instance;
        }
    }

    public ZoomMaterials(Material customEffect)
    {
        this.customEffect = customEffect;
    }
} 