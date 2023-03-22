using UnityEngine;

//[System.Serializable, CreateAssetMenu(fileName = "BlendMode")]
public class BlendModeMaterial : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    
    //---Accessing the data from the Pass---
    static BlendModeMaterial _instance;

    public static BlendModeMaterial Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

        
            Material customEffect = Resources.Load<Material>("BlendMode");
         
            //convert the material to a CustomPostProcessingMaterials
            _instance = CreateInstance<BlendModeMaterial>();

            //set the material to the instance
            _instance.customEffect = customEffect;

         
            return _instance;
        }
    }

    public BlendModeMaterial(Material customEffect)
    {
        this.customEffect = customEffect;
    }
} 