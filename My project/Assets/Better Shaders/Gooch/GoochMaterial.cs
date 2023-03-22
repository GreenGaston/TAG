using UnityEngine;

//[System.Serializable, CreateAssetMenu(fileName = "GoochMaterial", menuName = "GoochMaterial")]
public class GoochMaterial : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    
    //---Accessing the data from the Pass---
    static GoochMaterial _instance;

    public static GoochMaterial Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

            //get the material called "DoG" from the resources folder
            Material customEffect = Resources.Load<Material>("Gooch");
     
            //convert the material to a CustomPostProcessingMaterials
            _instance = CreateInstance<GoochMaterial>();

            //set the material to the instance
            _instance.customEffect = customEffect;


            return _instance;
        }
    }

    public GoochMaterial(Material customEffect)
    {
        this.customEffect = customEffect;
    }
} 