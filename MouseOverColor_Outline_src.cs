using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverColor_Outline_src : MonoBehaviour
{
    public GameObject rend_obj;//★MeshRendererを持つオブジェクトを手動で設定する

    [ColorUsage(false, true)] public Color color1;
    [ColorUsage(false, true)] public Color color2;

    Color m_MouseOverColor = Color.red;
    Color m_OriginalColor;

    MeshRenderer m_Renderer;

    void Start()
    {
        m_Renderer = rend_obj.GetComponent<MeshRenderer>();
        m_OriginalColor = m_Renderer.material.color;
        
        // マテリアルの変更するパラメータを事前に知らせる
        m_Renderer.material.EnableKeyword("_EMISSION");//必須

    }

    void OnMouseOver()
    {
        m_Renderer.material.SetColor("_EmissionColor", color1);
        //アウトラインを表示させる（別のフリーアセットをつけている）
        this.gameObject.GetComponent<cakeslice.Outline>().eraseRenderer = false;

    }

    void OnMouseExit()
    {
        m_Renderer.material.SetColor("_EmissionColor", color2);
        //アウトラインを非表示（別のフリーアセットをつけている）
        this.gameObject.GetComponent<cakeslice.Outline>().eraseRenderer = true;
    }
}