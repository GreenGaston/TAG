using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfacePainter : MonoBehaviour
{
    //store the 3 prefab objects that are used to paint the surface
    public GameObject paintPrefab1;
    public GameObject paintPrefab2;
    public GameObject paintPrefab3;
    private StarterAssetsInputs input;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if the player is pressing the left mouse button
        if(Input.GetMouseButton(0))
        {
            //create a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //check if the ray hits something
            if(Physics.Raycast(ray, out hit))
            {
                //check if the object hit has a tag of "Paintable"
                if(hit.collider.tag == "Paintable")
                {
                    //get the mesh renderer of the object hit
                    MeshRenderer meshRenderer = hit.collider.GetComponent<MeshRenderer>();
                    //get the material of the object hit
                    Material material = meshRenderer.material;
                    //get the texture of the material
                    Texture2D texture = material.mainTexture as Texture2D;
                    //get the UV coordinates of the object hit
                    Vector2 pixelUV = hit.textureCoord;
                    //get the pixel coordinates of the texture
                    pixelUV.x *= texture.width;
                    pixelUV.y *= texture.height;
                    //set the pixel color to red
                    texture.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.red);
                    //apply the changes to the texture
                    texture.Apply();
                }
            }
        }
    }
}
