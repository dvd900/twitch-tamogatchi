﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEyeEffect : MonoBehaviour
{


    [Range(0.0f, 1.5f)]
    public float strengthX = 0.2f;
    [Range(0.0f, 1.5f)]
    public float strengthY = 0.2f;

    public Shader curShader;
    private Material curMaterial;


    //获取材质，get
    Material material
    {

        get
        {
            if (curMaterial == null)
            {
                curMaterial = new Material(curShader);
                curMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return curMaterial;
        }
    }

    //当材质变为不可用或是非激活状态，调用删除此材质
    void OnDisable()
    {
        if (curMaterial)
        {
            DestroyImmediate(curMaterial);
        }
    }

    //此函数在当完成所有渲染图片后被调用，用来渲染图片后期效果
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (curShader != null)
        {
            float intensity = 0.1f;
            float proportion = (source.width * 1.0f) / (source.height * 1.0f);
            //设置shader 的外部变量
            material.SetVector("intensity", new Vector4(strengthX * proportion * intensity, strengthY * intensity, 0, 0));
            //复制源纹理到目标纹理，加上材质效果
            Graphics.Blit(source, destination, material);
        }
        else
        {
            //直接复制源纹理到目标纹理，不做特效处理
            Graphics.Blit(source, destination);
        }

    }

}
