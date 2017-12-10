using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereComponent : MonoBehaviour 
{	
    public void OnPreRender()
    {
        GL.invertCulling = true;
        Debug.LogFormat("<size=18><color=olive>{0}</color></size>", "hi!");
    }

//    public void OnPostRender()
//    {
//        GL.invertCulling = false;
//    }
}
