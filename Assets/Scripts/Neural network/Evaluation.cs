using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evaluation
{
    private List<float> evaluation;

    public Evaluation()
    {        
        evaluation = new List<float>();
    }


    public Evaluation(List<float> e)
    {
        this.evaluation = new List<float>(e);
    }

    public List<float> GetEvaluation()
    {
        return evaluation;
    }

    public void setEvaluation(List<float> evaluation)
    {
        this.evaluation = evaluation;
    }

    public void AddValue(float v)
    {
        evaluation.Add(v);
    }

    public float GetValue(int pos)
    {
        return evaluation[pos];
    }

    public void SetValue(int pos, float v)
    {
        evaluation[pos] = v;
    }    


    public int Size()
    {

        return evaluation.Count;
    }

    public void Log()
    {
        string str = "Eval: ";
        foreach(float f in evaluation)
        {
            str += f+" - ";
        }
        Debug.Log(str);
    }
}
