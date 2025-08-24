namespace Op.Prueba.Application.Tests.Specifications;

using Op.Prueba.Application.Specifications;
using Op.Prueba.Domain.Entities;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class PropertyTracesByPropertySpecTests
{
    [Test]
    public void Constructor_WithPropertyId_ShouldFilterByName()
    {
        // Arrange
        var spec = new PropertyTracesByPropertySpec("Property-001");

        // Act
        var result = spec.Evaluate(GetPropertyTraces());

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Name, Is.EqualTo("Property-001"));
    }

    [Test]
    public void Constructor_WithPaging_ShouldApplySkipTakeAndOrder()
    {
        // Arrange
        var spec = new PropertyTracesByPropertySpec(null, pageNumber: 2, pageSize: 2);

        // Act
        var result = spec.Evaluate(GetPropertyTraces()).ToList();

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("Property-002"));
        Assert.That(result[1].Name, Is.EqualTo("Property-003"));
    }

    [Test]
    public void Constructor_WithNoFilters_ShouldReturnAll()
    {
        // Arrange
        var spec = new PropertyTracesByPropertySpec(null);

        // Act
        var result = spec.Evaluate(GetPropertyTraces());

        // Assert
        Assert.That(result.Count(), Is.EqualTo(4));
    }

    private List<PropertyTrace> GetPropertyTraces()
    {
        return new List<PropertyTrace>
        {
            new PropertyTrace { Name = "Property-001" },
            new PropertyTrace { Name = "Property-002" },
            new PropertyTrace { Name = "Property-003" },
            new PropertyTrace { Name = "Another-Property" }
        };
    }
}
