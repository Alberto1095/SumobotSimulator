using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    private List<RobotIAController> robotList;
    private List<CombatController> combatControllers;    
    private GeneticEvolutionConfiguration config;
    private SumobotIAConfiguration configSumobot;

    private bool started = false;


    public void DestroyGeneration()
    {
        foreach(CombatController cc in combatControllers)
        {
            Destroy(cc.gameObject);
        }
    }

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

        foreach(CombatController c in combatControllers)
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
            Debug.Log("ENDED GEENRATION: "+robotList.Count);
            GeneticEvolutionManager.Instance.SpawnNextGeneration();
        }
    }

    private void SetRobotList()
    {
        robotList = new List<RobotIAController>();
        foreach(CombatController c in combatControllers)
        {
            robotList.Add(c.robotA as RobotIAController);
            robotList.Add(c.robotB as RobotIAController);
        }       
    }


    public void CreateInitialGeneration(GeneticEvolutionConfiguration config1, SumobotIAConfiguration config2)
    {
        this.config = config1;
        this.configSumobot = config2;        
        this.combatControllers = new List<CombatController>();
        SpawnRandom();
        SetRobotList();
        started = true;
    }

    private void SpawnRandom()
    {
        int startX = 0;
        int startY = 0;
        int offset = 25;
        int maxRow = 6;
        int rowCount = 0;
        Vector3 pos;
        int count = config.population / 2;
        for (int i = 0; i < count; i++)
        {
            pos = new Vector3(startX, startY, 0);
            startX += offset;
            rowCount++;
            if (rowCount >= maxRow)
            {
                rowCount = 0;
                startY -= offset;
                startX = 0;
            }

            CombatController match = Spawner.Instance.CreateIAvsIACombat(pos, SumobotIAConfiguration.Copy(configSumobot), SumobotIAConfiguration.Copy(configSumobot));
            combatControllers.Add(match);
        }
    }

    private void Spawn(List<Evaluation> list)
    {
        int startX = 0;
        int startY = 0;
        int offset = 20;
        int maxRow = 6;
        int rowCount = 0;
        Vector3 pos;
        int count = config.population;        
        SumobotIAConfiguration c1, c2;
        for (int i = 0; i < count; i+=2)
        {
            pos = new Vector3(startX, startY, 0);
            startX += offset;
            rowCount++;
            if (rowCount >= maxRow)
            {
                rowCount = 0;
                startY -= offset;
                startX = 0;
            }
            c1 = SumobotIAConfiguration.Copy(configSumobot);
            c1.weights = list[i].GetEvaluation();
            c2 = SumobotIAConfiguration.Copy(configSumobot);
            c2.weights = list[i+1].GetEvaluation();

            CombatController match = Spawner.Instance.CreateIAvsIACombat(pos, c1,c2);
            combatControllers.Add(match);
        }
    }

    private Evaluation GetBestFitnessEval()
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

    public List<Evaluation> GetEvaluationList()
    {
        List<Evaluation> l = new List<Evaluation>();
        foreach(RobotIAController r in robotList)
        {
            l.Add(r.GetEvaluation());
        }

        return l;
    }

    public void CreateGenerationFromPrevious(Generation bg)
    {   
        this.config = bg.config;
        this.configSumobot = bg.configSumobot;
        this.combatControllers = new List<CombatController>();
        this.robotList = new List<RobotIAController>();
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

        //Spawn new generation
        bg.DestroyGeneration();
        Spawn(hijosEvaluation);
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

    private int[] TournamentSelection(float[] fitnessList, int tournamentSize)
    {        
        /*
        foreach(float f in fitnessList)
        {
            Debug.Log("FITNESS: "+ f);
        }
        */
        int[] indexParents = new int[fitnessList.Length];
        int[] tournamentIndex = new int[tournamentSize];
        int max = fitnessList.Length - 1;
        int min = 0;
        int bestIndex;
        for (int i = 0; i < fitnessList.Length; i++)
        {
            //Cojemos indices random
            tournamentIndex = new int[tournamentSize];
            
            for (int j = 0; j < tournamentSize; j++)
            {
                tournamentIndex[j] = Random.Range(min, max+1);               
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
           //Debug.Log("BEST INDEX SEELCTED: "+bestIndex);
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
            if (random < pcruce)            {               
               
                hijo1Ev = new Evaluation();
                hijo2Ev = new Evaluation();

                for (int j = 0; j < padre1Ev.Size(); j++)
                {
                    paramPadre1 = padre1Ev.GetValue(j);
                    paramPadre2 = padre2Ev.GetValue(j);

                    distance = Mathf.Abs(paramPadre1 - paramPadre2);

                    paramHijo1 = Mathf.Min(paramPadre2, paramPadre1) - alpha * distance;
                    paramHijo2 = Mathf.Max(paramPadre2, paramPadre1) + alpha * distance;
                    
                    hijo1Ev.AddValue(paramHijo1);
                    hijo2Ev.AddValue(paramHijo2);
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
        Evaluation selectedChildEv;
        for (int i = 0; i < hijosEvaluation.Count; i++)
        {
            random = Random.Range(0f, 1f);
            if (random < pmutacion)
            {                
                maxParam = hijosEvaluation[i].Size() - 1;
                randomParam = (int)(minParam + Random.value * (maxParam - minParam));
                randomValue = (float)(minValue + Random.value * (maxValue - minValue));
                selectedChildEv = hijosEvaluation[i];
                selectedChildEv.SetValue(randomParam, randomValue);
            }           
        }
        return hijosEvaluation;
    }


}
