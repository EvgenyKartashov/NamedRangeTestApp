namespace NamedRangeTestApp.Models;

public class ScenarioCorrelation
{
    public string ScenarioName { get; init; }
    public ModelCorrelation[] Models { get; init; }
}

public class ModelCorrelation
{
    public string Name { get; init; }
    public Correlation[] Correlations { get; init; }
}

public class Correlation
{
    public string ModelRange { get; init; }
    public string ScenarioRange { get; init; }
}
