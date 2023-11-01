using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer[] wall_renderer;

    private Material wall_material;
    private float r = 0;
    private float g = 0;
    private float b = 0;

    private bool green = true;
    private bool blue;
    private bool red;




    // Start is called before the first frame update
    void Start()
    {
        wall_material = new Material(wall_renderer[0].material);

        foreach (MeshRenderer renderer in wall_renderer)
        {
            renderer.material = wall_material;
        }
        InvokeRepeating(nameof(gradiant), 0.1f, 0.01f);
    }

    // Update is called once per frame
    private void gradiant()
    {
        if (green)
        {
            if(b == 241)
            {
                g -= 1;
                if(g <= 0)
                {
                    green = false;
                    blue = true;
                    g = 0;
                }
            }
            else
            {
                b += 1;
            }
        }
        else if (blue)
        {
            if(r == 241)
            {
                b -= 1;
                if(b <= 0)
                {
                    blue = false;
                    red = true;
                    b = 0;
                }
            }
            else
            {
                r += 1;
            }
        }
        else if (red)
        {
            if(g == 241)
            {
                r -= 1;
                if(r <= 0)
                {
                    red = false;
                    green = true;
                    r = 0;
                }
            }
            else
            {
                g += 1;
            }
        }

        wall_material.SetColor("_EmissionColor", new Color(r, g, b) * 0.002f);
    }
}
