namespace Domain.Guards;

// An entry point to a set of Guard Clauses defined as extension methods on IGuardClause.
public interface IEnsure { }


public class Ensure : IEnsure
{
    // only way to create a guard class
    public static IEnsure Against = new Ensure();

    private Ensure() { }
}
