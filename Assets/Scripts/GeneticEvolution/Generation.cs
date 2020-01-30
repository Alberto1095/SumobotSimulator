using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    private List<RobotIAController> robotList;
    
    private GeneticEvolutionConfiguration config;
    private SumobotIAConfiguration configSumobot;

    private bool started = false;   

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            CheckEndGeneration();
        }
    } 

    private void CheckEndGeneration()
    {
        bool end = true;

        foreach(CombatController c in CombatManager.Instance.combatControllers)
        {
            if (!c.finished)
            {
                end = false;
                break;
            }
        }
                
        if (end)
        {
            started = false;
            UpdateBestFitness();
            GeneticEvolutionManager.Instance.SpawnNextGeneration();
        }
    }

    private void SetRobotList()
    {
        robotList = new List<RobotIAController>();
        foreach (CombatController c in CombatManager.Instance.combatControllers)
        {
            robotList.Add(c.robotA as RobotIAController);
            //Robot B is best oponent from previous generation so we dont add to list
            //robotList.Add(c.robotB as RobotIAController);
        }
    }


    public void CreateInitialGeneration(GeneticEvolutionConfiguration geneticConfig, SumobotIAConfiguration iaConfig)
    {      
        this.config = geneticConfig;
        this.configSumobot = iaConfig;        
        CombatManager.Instance.SpawnRandomGeneration(iaConfig, geneticConfig);
        SetRobotList();
        started = true;
    }
    
    public void CreateGenerationFromPrevious(Generation bg)
    {   
        this.config = bg.config;
        this.configSumobot = bg.configSumobot;         
        //Get best fitness eval of previous generation		
        Evaluation bestEval = bg.GetBestFitnessEval();        

        //Selection
        int[] indexParents = null;
        switch (config.selectionFunction)
        {
            case SelectionFunction.Tournament:
                indexParents = TournamentSelection(bg.GetFitnessList(),config.tournamentSize);
                break;
        }
        //Cross
        List<Evaluation> hijosEvaluation = null;
        switch (config.crossFunction)
        {
            case CrossFunction.BLX:
                hijosEvaluation = CruceBLX(bg.GetEvaluationList(),indexParents,config.alphaBLX,config.crossChance);
                break;
        }

        //Mutation
        if (config.useMutation)
        {
            switch (config.mutationFunction)
            {
                case MutationFunction.Normal:
                    hijosEvaluation = Mutacion(hijosEvaluation, config.mutationChance);
                    break;
            }
        }

        //Elitismo
        if (config.useElitismo)
        {
            hijosEvaluation.RemoveAt(0);
            hijosEvaluation.Add(bestEval);
        }
        /*
        foreach(Evaluation v in hijosEvaluation)
        {
            v.Log();
        }
        */
        //Spawn new generation       
        CombatManager.Instance.SpawnGeneration(hijosEvaluation,configSumobot,config,GeneticEvolutionManager.Instance.bestEvaluation);
        SetRobotList();
        started = true;
    }

    public float[] GetFitnessList()
    {
        float[] f = new float[config.population];
        for (int i = 0; i < config.population; i++)
        {
            f[i] = robotList[i].GetFitness();           
        }

        return f;
    }

    public Evaluation GetBestFitnessEval()
    {
        Evaluation eval = null;
        int index = 0;
        int bestIndex = 0;
        float best = 0;
        foreach (RobotIAController r in robotList)
        {
            if (r.GetFitness() >= best)
            {
                best = r.GetFitness();
                eval = r.GetEvaluation();
                bestIndex = index;
            }
            index++;
        }
        Debug.Log("BEST FITNESS: " + best);
        return eval;
    }

    private void UpdateBestFitness()
    {
        Evaluation eval = null;
        int index = 0;
        int bestIndex = 0;
        float best = 0;
        foreach (RobotIAController r in robotList)
        {
            if (r.GetFitness() >= best)
            {
                best = r.GetFitness();
                eval = r.GetEvaluation();
                bestIndex = index;
            }
            index++;
        }
       
        if(best > GeneticEvolutionManager.Instance.bestFitness)
        {
            GeneticEvolutionManager.Instance.bestFitness = best;
            GeneticEvolutionManager.Instance.bestEvaluation = eval;
        }
    }
    

    public List<Evaluation> GetEvaluationList()
    {
        List<Evaluation> l = new List<Evaluation>();
        foreach (RobotIAController r in robotList)
        {
            l.Add(r.GetEvaluation());
        }

        return l;
    }

    private int[] TournamentSelection(float[] fitnessList, int tournamentSize)
    {      
       
        int[] indexParents = new int[fitnessList.Length];
        int[] tournamentIndex = new int[tournamentSize];
        int max = fitnessList.Length;
        int min = 0;
        int bestIndex;
        for (int i = 0; i < fitnessList.Length; i++)
        {
            //Cojemos indices random
            tournamentIndex = new int[tournamentSize];
            
            for (int j = 0; j < tournamentSize; j++)
            {
                tournamentIndex[j] = Random.Range(min, max);               
            }
           
            //Elejimos el mejor 
            bestIndex = tournamentIndex[0];
            for (int x = 0; x < tournamentSize; x++)
            {
                if (fitnessList[bestIndex] < fitnessList[tournamentIndex[x]])
                {
                    bestIndex = tournamentIndex[x];
                }
            }
            //Añadimos el index	          
            indexParents[i] = bestIndex;
        }
        return indexParents;
    }

    private List<Evaluation> CruceBLX(List<Evaluation> population,int[] indexParents,float alpha,float pcruce)
    {        
        List<Evaluation> childsEvaluation = new List<Evaluation>();
        Evaluation padre1Ev;
        Evaluation padre2Ev;
        Evaluation hijo1Ev;
        Evaluation hijo2Ev;
        float paramHijo1;
        float paramHijo2;
        float paramPadre1;
        float paramPadre2;
        float random;
        float distance;

        for (int i = 0; i < indexParents.Length; i += 2)
        {
            padre1Ev = population[i];
            padre2Ev = population[i + 1];
            random = Random.Range(0f,1f);            
            if (random < pcruce)
            {              
                hijo1Ev = new Evaluation();
                hijo2Ev = new Evaluation();

                for (int j = 0; j < padre1Ev.Size(); j++)
                {
                    paramPadre1 = padre1Ev.GetValue(j);
                    paramPadre2 = padre2Ev.GetValue(j);

                    distance = Mathf.Abs(paramPadre1 - paramPadre2);
                    
                    paramHijo1 = Mathf.Min(paramPadre2, paramPadre1) - alpha * distance;
                    paramHijo2 = Mathf.Max(paramPadre2, paramPadre1) + alpha * distance;

                    float i1 = Random.Range(Mathf.Min(paramHijo1, paramHijo2), Mathf.Max(paramHijo1, paramHijo2));
                    float i2 = Random.Range(Mathf.Min(paramHijo1, paramHijo2), Mathf.Max(paramHijo1, paramHijo2));
                    hijo1Ev.AddValue(i1);
                    hijo2Ev.AddValue(i2);
                }

                childsEvaluation.Add(hijo1Ev);
                childsEvaluation.Add(hijo2Ev);

            }
            else
            {
                //To do si no se cruzan añadir evaluacion padre               
                childsEvaluation.Add(padre1Ev);
                childsEvaluation.Add(padre2Ev);
            }

        }
        return childsEvaluation;
    }

    public List<Evaluation> Mutacion(List<Evaluation> hijosEvaluation, double pmutacion)
    {
        float maxValue = 1;
        float minValue = -1;
        float randomValue = 0;
        int maxParam = 0;
        int minParam = 0;
        int randomParam = 0;
        float random = 0;
      
        for (int i = 0; i < hijosEvaluation.Count; i++)
        {
            random = Random.Range(0f, 1f);
            if (random < pmutacion)
            {                
                maxParam = hijosEvaluation[i].Size();
                randomParam = Random.Range(minParam, maxParam);               
                randomValue = (float)(minValue + Random.value * (maxValue - minValue));               
                hijosEvaluation[i].SetValue(randomParam, randomValue);
            }           
        }
        return hijosEvaluation;
    }
    
}
