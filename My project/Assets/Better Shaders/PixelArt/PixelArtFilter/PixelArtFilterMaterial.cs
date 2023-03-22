using UnityEngine;

//[System.Serializable, CreateAssetMenu(fileName = "PixelArtFilterMaterials", menuName = "PixelArtFilterMaterials")]
public class PixelArtFilterMaterials : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    
    //---Accessing the data from the Pass---
    static PixelArtFilterMaterials _instance;

    public static PixelArtFilterMaterials Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

            //get the material called "DoG" from the resources folder
            Material customEffect = Resources.Load<Material>("PixelArtFilter");
       
            //convert the material to a CustomPostProcessingMaterials
            _instance = CreateInstance<PixelArtFilterMaterials>();

            //set the material to the instance
            _instance.customEffect = customEffect;

    
            return _instance;
        }
    }

    public PixelArtFilterMaterials(Material customEffect)
    {
        this.customEffect = customEffect;
    }
} 