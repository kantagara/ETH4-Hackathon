using UnityEngine;

public class HeightToShader : MonoBehaviour
{
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateShaderHeight();
    }

    void UpdateShaderHeight()
    {
        float objectHeight = rend.bounds.size.y;
        rend.material.SetFloat("_ObjectHeight", objectHeight);
    }
}