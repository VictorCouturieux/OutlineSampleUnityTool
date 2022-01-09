using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class OutlineCommandBufferWithLayers : MonoBehaviour {
//    [HideInInspector] //a commenter pour custom shader
//    public Material mat = null;

    public List<OutLineParams> objOutlines = new List<OutLineParams>();

    CommandBuffer buffer;
    Camera cam;
    Renderer target;

    [HideInInspector] public bool isRunTime;

    void Start() {
        isRunTime = true;
        buffer = new CommandBuffer();

        foreach (OutLineParams outline in objOutlines) {
            outline.SetOcbw(this);
            //material set
//            if (mat == null) {
            outline.mat = new Material(Shader.Find("Custom/OutlineTwoPass"));
//            }
//            else {
//                outline.mat = new Material(mat.shader);
//            }

            outline.OnValidate();
        }

        //set render loop timing
        cam = GetComponent<Camera>();
        var camEvent = CameraEvent.AfterForwardOpaque;
        cam.AddCommandBuffer(camEvent, buffer);
    }

    void OnPreRender() {
        foreach (OutLineParams outline in objOutlines) {
            //will return an array of all GameObjects in the scene
            GameObject[] gos = FindObjectsOfType(
                typeof(GameObject)) as GameObject[];
            foreach (GameObject go in gos) {
//                Debug.Log("obj : " + LayerMask.LayerToName( layermask_to_layer(outline.layer)));
                
                if (go.layer == layermask_to_layer(outline.layer) ) {
//                    Debug.Log("titi");
                    Transform obj = go.transform;
                    if (!obj) {
                        buffer.Clear();
                        target = null;
                        return;
                    }

                    //Only draw once
                    if (target != obj.GetComponent<Renderer>()) {
                        target = obj.GetComponent<Renderer>();
                        buffer.DrawRenderer(target, outline.mat, 0, 4); //only draw outline pass
                    }
                }
            }
        }
    }

    void OnValidate() {
        foreach (OutLineParams outline in objOutlines) {
            outline.OnValidate();
        }
    }
    
    public static int layermask_to_layer(LayerMask layerMask) {
        int layerNumber = 0;
        int layer = layerMask.value;
        while(layer > 0) {
            layer = layer >> 1;
            layerNumber++;
        }
        return layerNumber - 1;
    }

}