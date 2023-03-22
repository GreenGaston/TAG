using UnityEngine;

//[System.Serializable, CreateAssetMenu(fileName = "CustomPostProcessingMaterials", menuName = "CustomPostProcessingMaterials")]
public class DoGMaterial : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    
    //---Accessing the data from the Pass---
    static DoGMaterial _instance;

    public static DoGMaterial Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

            //get the material called "DoG" from the resources folder
            Material customEffect = Resources.Load<Material>("DoG");
        
            //convert the material to a DoGMaterial
            _instance = CreateInstance<DoGMaterial>();

            //set the material to the instance
            _instance.customEffect = customEffect;


            return _instance;
        }
    }

    public DoGMaterial(Material customEffect)
    {
        this.customEffect = customEffect;
    }
} 