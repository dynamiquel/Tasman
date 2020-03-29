using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TLight : MonoBehaviour
{
    Color colour;
    public Light[] emissionLights = new Light[6];
    public Light[] lightSources = new Light[6];

    public enum Face
    {
        Top,
        Bottom,
        East,
        West,
        North,
        South,
        All
    }

    public void SetColour(Color colour)
    {
        this.colour = colour;

        for (int i = 0; i < lightSources.Length; i++)
        {
            emissionLights[i].color = colour;
            lightSources[i].color = colour;
        }
    }

    public void SetFace(Face face, bool isActive)
    {
        if (face == Face.All)
        {
            for (int i = 0; i < emissionLights.Length; i++)
                SetFace((Face)i, isActive);
            return;
        }

        emissionLights[(int)face].enabled = isActive;
        lightSources[(int)face].enabled = isActive;      
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void SetSourceIntensity(Face face, float intensity)
    {
        if (face == Face.All)
        {
            for (int i = 0; i < lightSources.Length; i++)
                SetSourceIntensity((Face)i, intensity);
            return;
        }

        lightSources[(int)face].intensity = intensity;
    }

    public void SetEmissionIntensity(Face face, float intensity)
    {
        if (face == Face.All)
        {
            for (int i = 0; i < emissionLights.Length; i++)
                SetEmissionIntensity((Face)i, intensity);
            return;
        }

        emissionLights[(int)face].intensity = intensity;
    }

    public void SetSourceRange(Face face, float range)
    {
        if (face == Face.All)
        {
            for (int i = 0; i < lightSources.Length; i++)
                SetSourceRange((Face)i, range);
            return;
        }

        lightSources[(int)face].range = range;
    }
}
