namespace Wolfgang.Extensions.String.Tests.Unit;

/// <summary>
/// Names the test collection of the classes that measure allocation or allocate a great deal. xunit
/// runs the classes of one collection one after another, so a garbage collection caused by one of
/// them cannot show up in a measurement another is taking.
/// </summary>
internal static class AllocationMeasurements
{
    public const string Name = "Allocation measurements";
}
