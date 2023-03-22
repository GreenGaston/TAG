using UnityEngine;

//[System.Serializable, CreateAssetMenu(fileName = "HueShiftMaterials", menuName = "HueShiftMaterials")]
public class HueShiftMaterials : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    
    //---Accessing the data from the Pass---
    static HueShiftMaterials _instance;

    public static HueShiftMaterials Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

            //get the material called "DoG" from the resources folder
            Material customEffect = Resources.Load<Material>("HueShift");
        
            //convert the material to a CustomPostProcessingMaterials
            _instance = CreateInstance<HueShiftMaterials>();

            //set the material to the instance
            _instance.customEffect = customEffect;

        
            return _instance;
        }
    }

    public HueShiftMaterials(Material customEffect)
    {
        this.customEffect = customEffect;
    }
} 