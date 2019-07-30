using System.Collections;
using System.Collections.Generic;
using Unicessing;
using UnityEngine;

public class SineWave : UGraphics
{
    [SerializeField]
    private Color bg;

    [SerializeField]
    private int xSpacing = 32;
    public int Width = Screen.width;

    float theta = 0f;
    float amp = 250f;
    float lambda = 500f;  
    float dx;
    float[] yDis;

    protected override void Setup()
    {
        size(Screen.width, Screen.height, P2D, 0.01f);
        blendMode(UMaterials.BlendMode.Add);
        
        //dx == xSpacing in radian
        dx = TWO_PI * (xSpacing/lambda);
        yDis = new float[Width / xSpacing];
    }

    protected override void Draw()
    {
        //background(bg);
        calcDis();
        renderWave();
    }

    private void calcDis()
    {
        //Increment theta
        theta += 0.02f;

        float x = theta;
        for (int i = 0; i < yDis.Length; i++)
        {
            yDis[i] = sin(x) * amp;
            x += dx;
        }
    }

    private void renderWave()
    {
        //noStroke();
        //fill(255);
        stroke(255);
        
        //for (int x = 0; x < yDis.Length; x++)
        //{
        //    ellipse(x * xSpacing, yDis[x]+height/2, 32f, 32f);
        //}
        beginShape(UShape.VertexType.CURVE_LINE_STRIP);
        for (int x = 0; x < yDis.Length; x++)
        {
            curveVertex(x * xSpacing, yDis[x] + height / 2);
        }
        endShape();
    }
}
