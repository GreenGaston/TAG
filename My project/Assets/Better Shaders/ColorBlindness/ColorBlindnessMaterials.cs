using UnityEngine;

//[System.Serializable, CreateAssetMenu(fileName = "ColorBlindnessMaterials")]
public class  ColorBlindnessMaterials : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    
    //---Accessing the data from the Pass---
    static ColorBlindnessMaterials _instance;

    public static ColorBlindnessMaterials Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

            //get the material called "DoG" from the resources folder
            Material customEffect = Resources.Load<Material>("ColorBlindness");
         
            //convert the material to a CustomPostProcessingMaterials
            _instance = CreateInstance<ColorBlindnessMaterials>();

            //set the material to the instance
            _instance.customEffect = customEffect;

       
            return _instance;
        }
    }

    public ColorBlindnessMaterials(Material customEffect)
    {
        this.customEffect = customEffect;
    }
} 