using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveManager : MonoBehaviour
{

    public Material waveMaterial;
    public ComputeShader waveComput;
    public RenderTexture NState, Nm1State, Np1State;
    public Vector2Int resolution;

    public Vector3 effect; //x cord, y cord, strength
    public float dispersion;

    // Start is called before the first frame update
    void Start()
    {
        InitilizeTexture(ref NState);
        InitilizeTexture(ref Nm1State);
        InitilizeTexture(ref Np1State);

        waveMaterial.mainTexture = NState;
    }

    void InitilizeTexture(ref RenderTexture tex)

    {
        tex = new RenderTexture(resolution.x, resolution.y, 1, UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_SNorm);
        tex.enableRandomWrite = true;
        tex.Create();
    }

    // Update is called once per frame
    void Update()
    {
        Graphics.CopyTexture(NState, Nm1State);
        Graphics.CopyTexture(Np1State, NState);

        waveComput.SetTexture(0, "NState", NState);
        waveComput.SetTexture(0, "Nm1State", NState);
        waveComput.SetTexture(0, "Np1State", NState);
        waveComput.SetVector("effect", effect);
        waveComput.SetVector("resolution", new Vector2(resolution.x, resolution.y));
        //waveComput.SetInts("resolution", new int[] { resolution.x, resolution.y });

        waveComput.Dispatch(0, resolution.x / 8, resolution.y / 8, 1);
    }
}

