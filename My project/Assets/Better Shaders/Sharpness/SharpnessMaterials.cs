using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "SharpnessMaterials", menuName = "SharpnessMaterials")]
public class SharpnessMaterials : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    public Material customEffect2;
    
    //---Accessing the data from the Pass---
    static SharpnessMaterials _instance;
    

    public static SharpnessMaterials Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

            //get the material called "DoG" from the resources folder
            Material customEffect = Resources.Load<Material>("Sharpness");
            Material  customEffect2 = Resources.Load<Material>("ContrastSharpness");
            if (customEffect == null|| customEffect2 == null)
            {
                Debug.Log("FUCK");
                return null;
            }
            //convert the material to a CustomPostProcessingMaterials
            _instance = CreateInstance<SharpnessMaterials>();

            //set the material to the instance
            _instance.customEffect = customEffect;
            _instance.customEffect2 = customEffect2;

            if(_instance == null || _instance.customEffect2 == null){
                Debug.Log("FUCK2");
            }

            return _instance;
        }
    }

    public SharpnessMaterials(Material customEffect)
    {
        this.customEffect = customEffect;
    }
} 