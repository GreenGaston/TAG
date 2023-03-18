using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "PaletteSwapMaterials", menuName = "PaletteSwapMaterials")]
public class PaletteSwapMaterials : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    
    //---Accessing the data from the Pass---
    static PaletteSwapMaterials _instance;

    public static PaletteSwapMaterials Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

            //get the material called "DoG" from the resources folder
            Material customEffect = Resources.Load<Material>("PaletteSwap");
            if (customEffect == null)
            {
                Debug.Log("FUCK");
                return null;
            }
            //convert the material to a CustomPostProcessingMaterials
            _instance = CreateInstance<PaletteSwapMaterials>();

            //set the material to the instance
            _instance.customEffect = customEffect;

            if(_instance == null)
                Debug.Log("FUCK2");

            return _instance;
        }
    }

    public PaletteSwapMaterials(Material customEffect)
    {
        this.customEffect = customEffect;
    }
} 