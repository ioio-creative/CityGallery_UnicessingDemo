using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRWave : MonoBehaviour
{
    [Serializable]
    public class Harmonic
    {
        public float Amp;
        public float Lambda;
        public float Speed;

        /// <summary>
        /// Radian increment
        /// </summary>
        public float DeltaRadian
        {
            get;set;
        }
        public float PhaseShift
        {
            get;set;
        }
    }

    [Range(0f,4096f)]
    [SerializeField]
    private int samples = 1024;
    [SerializeField]
    private float length = 25;

    /// <summary>
    /// Domain dimension spacing
    /// </summary>
    private static float DomainIncrement;
    private float timeShift = 0f;

    [SerializeField]
    private Harmonic[] harmonics;
    private Vector3[] superpositionPoints;

    [SerializeField]
    private LineRenderer line;

    private void Awake()
    {
        if (line == null)
        {
            line = GetComponent<LineRenderer>();
        }

        //superpositionPoints = new List<Vector3>(samples);
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
        superpositionPoints = new Vector3[samples];
        DomainIncrement = length / samples;

        foreach (var harmonic in harmonics)
        {
            harmonic.DeltaRadian = harmonic.Lambda == 0 ? float.NaN : Mathf.PI * 2 * (DomainIncrement / harmonic.Lambda);
            if (harmonic.Lambda != 0)
            {
                harmonic.PhaseShift += Time.deltaTime * harmonic.Speed / harmonic.Lambda;
            }
            harmonic.PhaseShift %= (2 * Mathf.PI);
        }

        //Increment theta with time
        //timeShift += (Time.deltaTime * speed);
        //timeShift %= (2 * Mathf.PI);

        //float x = timeShift;

        for (int i = 0; i < superpositionPoints.Length; i++)
        {
            float superposedValue = 0;
            foreach (var component in harmonics)
            {
                if ( !float.IsNaN(component.DeltaRadian))
                {
                    superposedValue += Mathf.Sin(i * component.DeltaRadian + component.PhaseShift) * component.Amp;
                }
            }

            superpositionPoints[i] = new Vector3(i * DomainIncrement, superposedValue);
        }


    }

    private void RenderWave()
    {
        line.positionCount = samples;
        line.SetPositions(superpositionPoints);
    }
}
