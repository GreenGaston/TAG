using UnityEngine;

//[System.Serializable, CreateAssetMenu(fileName = "CustomMaterials", menuName = "CustomMaterials")]
public class CustomMaterials : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    
    //---Accessing the data from the Pass---
    static CustomMaterials _instance;

    public static CustomMaterials Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

            //get the material called "DoG" from the resources folder
            Material customEffect = Resources.Load<Material>("DoG");

            //convert the material to a CustomPostProcessingMaterials
            _instance = CreateInstance<CustomMaterials>();

            //set the material to the instance
            _instance.customEffect = customEffect;

            return _instance;
        }
    }

    public CustomMaterials(Material customEffect)
    {
        this.customEffect = customEffect;
    }
} 