using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "EDOGMaterial", menuName = "EDOGMaterial")]
public class EDOGMaterial : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    
    //---Accessing the data from the Pass---
    static EDOGMaterial _instance;

    public static EDOGMaterial Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

            //get the material called "DoG" from the resources folder
            Material customEffect = Resources.Load<Material>("EDoG");
            if (customEffect == null)
            {
                Debug.Log("FUCK");
                return null;
            }
            //convert the material to a CustomPostProcessingMaterials
            _instance = CreateInstance<EDOGMaterial>();

            //set the material to the instance
            _instance.customEffect = customEffect;

            if(_instance == null)
                Debug.Log("FUCK2");

            return _instance;
        }
    }

    public EDOGMaterial(Material customEffect)
    {
        this.customEffect = customEffect;
    }
} 