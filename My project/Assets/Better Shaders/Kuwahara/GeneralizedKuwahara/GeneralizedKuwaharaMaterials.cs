using UnityEngine;

//[System.Serializable, CreateAssetMenu(fileName = "GeneralizedKuwaharaMaterials", menuName = "GeneralizedKuwaharaMaterials")]
public class GeneralizedKuwaharaMaterials : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    
    //---Accessing the data from the Pass---
    static GeneralizedKuwaharaMaterials _instance;

    public static GeneralizedKuwaharaMaterials Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

            //get the material called "DoG" from the resources folder
            Material customEffect = Resources.Load<Material>("GeneralizedKuwahara");
            if (customEffect == null)
            {
                Debug.Log("FUCK");
                return null;
            }
            //convert the material to a CustomPostProcessingMaterials
            _instance = CreateInstance<GeneralizedKuwaharaMaterials>();

            //set the material to the instance
            _instance.customEffect = customEffect;

            if(_instance == null)
                Debug.Log("FUCK2");

            return _instance;
        }
    }

    public GeneralizedKuwaharaMaterials(Material customEffect)
    {
        this.customEffect = customEffect;
    }
} 