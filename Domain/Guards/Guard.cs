namespace Domain.Guards;

// An entry point to a set of Guard Clauses defined as extension methods on IGuardClause.
public interface IGuard { }


public class Guard : IGuard
{
    // only way to create a guard class
    public static IGuard Against = new Guard();

    private Guard() { }
}
