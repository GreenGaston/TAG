using UnityEngine;

//[System.Serializable, CreateAssetMenu(fileName = "GammaMaterials", menuName = "GammaMaterials")]
public class GammaMaterials : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    
    //---Accessing the data from the Pass---
    static GammaMaterials _instance;

    public static GammaMaterials Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

            //get the material called "DoG" from the resources folder
            Material customEffect = Resources.Load<Material>("Gamma");
          
            //convert the material to a CustomPostProcessingMaterials
            _instance = CreateInstance<GammaMaterials>();

            //set the material to the instance
            _instance.customEffect = customEffect;


            return _instance;
        }
    }

    public GammaMaterials(Material customEffect)
    {
        this.customEffect = customEffect;
    }
} 