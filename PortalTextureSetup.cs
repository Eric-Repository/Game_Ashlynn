using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    public Camera exitPortalCamera;
    public Camera enterPortalCamera;
    public Material enterPortalMaterial;
    public Material exitPortalMaterial;

    // Start is called before the first frame update
    void Start()
    {
        if (enterPortalCamera.targetTexture != null)
        {
            enterPortalCamera.targetTexture.Release();
        }
        // Creates a new rendure texture that is the size of the screen
        enterPortalCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

        // Sets the texture to a material
        exitPortalMaterial.mainTexture = enterPortalCamera.targetTexture;

        if (exitPortalCamera.targetTexture != null)
        {
            exitPortalCamera.targetTexture.Release();
        }
        // Creates a new rendure texture that is the size of the screen
        exitPortalCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

        // Sets the texture to a material
        enterPortalMaterial.mainTexture = exitPortalCamera.targetTexture;
    }
}
