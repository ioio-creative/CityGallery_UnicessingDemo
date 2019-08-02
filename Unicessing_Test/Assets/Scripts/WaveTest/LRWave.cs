using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRWave : MonoBehaviour
{
    [Serializable]
    public struct Harmonic
    {
        public float Amp;
        public float Lambda;
        public float Speed;
    }

    [Range(0f,4096f)]
    [SerializeField]
    private int samples = 1024;

    [SerializeField]
    private float length = 25;
    private float increment
    {
        get
        {
            return length / samples;
        }
    }
       
    //dx == xSpacing in radian
    private float dx
    {
        get { return Mathf.PI * 2 * (increment / lambda); }
    }
    private float theta = 0f;

    [SerializeField]
    private Harmonic[] harmonics;

    [SerializeField]
    private float amp = 25f;
    [SerializeField]
    private float lambda;
    [SerializeField]
    private float speed = 1;

    private Vector3[] samplePts;
    [SerializeField]
    private LineRenderer line;

    private void Awake()
    {
        if (line == null)
        {
            line = GetComponent<LineRenderer>();
        }

        samplePts = new Vector3[samples];
    }
    

    private void Start()
    {
        
    }

    private void Update()
    {
        CalcPoints();
        RenderWave();
    }

    private void CalcPoints()
    {
        //Increment theta
        theta += (Time.deltaTime * speed);
        theta %= (2 * Mathf.PI);

        float x = theta;
        for (int i = 0; i < samplePts.Length; i++)
        {
            samplePts[i] = new Vector3(i*increment, Mathf.Sin(x) * amp);
            x += dx;
        }
    }

    private void RenderWave()
    {
        line.positionCount = samples;
        line.SetPositions(samplePts);
    }
}
