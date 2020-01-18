public enum SelectionFunction { Tournament };
public enum MutationFunction { Gaussian };
public enum CrossFunction { BLX };

public class GeneticEvolutionConfiguration 
{
    public int maxSteps;
    public int population;
   
    public SelectionFunction selectionFunction;
    public bool useMutation;
    
    public MutationFunction mutationFunction;
    public float mutationChance;
   
    public CrossFunction crossFunction;
    public float crossChance;

    public bool useElitismo;

   
    public GeneticEvolutionConfiguration()
    {

    }

    public GeneticEvolutionConfiguration(int maxSteps, int population, SelectionFunction selectionFunction,
        bool useMutation, MutationFunction mutationFunction, float mutationChance,
        CrossFunction crossFunction, float crossChance, bool useElitismo)
    {
        this.maxSteps = maxSteps;
        this.population = population;
        this.selectionFunction = selectionFunction;
        this.useMutation = useMutation;
        this.mutationFunction = mutationFunction;
        this.mutationChance = mutationChance;
        this.crossFunction = crossFunction;
        this.crossChance = crossChance;
        this.useElitismo = useElitismo;
    }
}
