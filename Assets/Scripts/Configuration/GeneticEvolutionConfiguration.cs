public enum SelectionFunction { Tournament };
public enum MutationFunction { Normal };
public enum CrossFunction { BLX };

public class GeneticEvolutionConfiguration 
{
    public int maxSteps;
    public int population;
   
    public SelectionFunction selectionFunction;
    public int tournamentSize;

    public bool useMutation;    
    public MutationFunction mutationFunction;
    public float mutationChance;
   
    public CrossFunction crossFunction;
    public float crossChance;
    public float alphaBLX;

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

    public static GeneticEvolutionConfiguration GetDefaultConfig()
    {
        GeneticEvolutionConfiguration config = new GeneticEvolutionConfiguration();
        config.maxSteps = 10000;
        config.population = 10;
        config.selectionFunction = SelectionFunction.Tournament;
        config.mutationFunction = MutationFunction.Normal;
        config.useMutation = false;
        config.mutationChance = 0.05f;
        config.useElitismo = true;
        config.crossChance = 0.95f;
        config.crossFunction = CrossFunction.BLX;
        config.tournamentSize = 4;
        config.alphaBLX = 0.5f;

        return config;
    }
}
