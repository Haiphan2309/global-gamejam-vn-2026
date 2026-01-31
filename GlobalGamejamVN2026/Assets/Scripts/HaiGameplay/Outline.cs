using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    [SerializeField] private Renderer m_renderer;

    public void Setup()
    {
        m_renderer.material = new Material(m_renderer.material);
    }

    public void SetThickness(float value)
    {
        m_renderer.material.SetFloat("_OutlineThickness", value);
    }
}
