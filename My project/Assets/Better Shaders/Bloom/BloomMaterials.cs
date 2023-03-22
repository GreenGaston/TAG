using UnityEngine;

//[System.Serializable, CreateAssetMenu(fileName = "BloomMaterials", menuName = "BloomMaterials")]
public class BloomMaterials : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    
    //---Accessing the data from the Pass---
    static BloomMaterials _instance;

    public static BloomMaterials Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

            //get the material called "DoG" from the resources folder
            Material customEffect = Resources.Load<Material>("Bloom");
           
            //convert the material to a CustomPostProcessingMaterials
            _instance = CreateInstance<BloomMaterials>();

            //set the material to the instance
            _instance.customEffect = customEffect;

            

            return _instance;
        }
    }

    public BloomMaterials(Material customEffect)
    {
        this.customEffect = customEffect;
    }
} 