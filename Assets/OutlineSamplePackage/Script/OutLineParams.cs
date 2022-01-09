using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OutLineParams {
    public LayerMask layer;
    public Color OutlineColor = Color.black;
    [Range(0, 2)] public float OutlineWidth = 1.026f;
    public bool isNormalExtrude = false;
    [HideInInspector]
    public Material mat;

    private OutlineCommandBufferWithLayers ocbw;

    public OutLineParams() {
    }
    
    public void OnValidate()
    {
        if (ocbw == null) return;
        if (!ocbw.isRunTime) return;
        mat.SetColor("_OutlineColor", OutlineColor);
        mat.SetFloat("_Outline", OutlineWidth);
        mat.SetFloat("_isNormalExtrude", isNormalExtrude ? 1 : 0);
    }
    
    public void SetOcbw(OutlineCommandBufferWithLayers ocbw) {
        this.ocbw = ocbw;
    }
}
