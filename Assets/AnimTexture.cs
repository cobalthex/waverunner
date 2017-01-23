using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//supports linear strips, not grids
public class AnimTexture : MonoBehaviour
{
    public float speed = 5;

    public int frameCount = 1;

    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        if (renderer == null)
            enabled = false;
    }

    int lastIndex = -1;
    // Update is called once per frame
    void Update()
    {
        // Calculate index
        int index = (int)(Time.time * speed) % frameCount;
        if (index != lastIndex)
        {
            Vector2 offset = new Vector2(index / frameCount, 0);
            renderer.material.SetTextureOffset("_MainTex", offset);

            lastIndex = index;
        }
    }
}
