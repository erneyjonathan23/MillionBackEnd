namespace Op.Prueba.Application.Tests.Specifications;

using NUnit.Framework;
using OP.Prueba.Application.Specifications;
using Op.Prueba.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class OwnersByFilterSpecTests
{

    [Test]
    public void Constructor_WithName_ShouldFilterByName()
    {
        // Arrange
        var spec = new OwnersByFilterSpec("Alice", null);

        // Act
        var result = spec.Evaluate(GetOwners());

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Name, Is.EqualTo("Alice"));
    }

    [Test]
    public void Constructor_WithAddress_ShouldFilterByAddress()
    {
        // Arrange
        var spec = new OwnersByFilterSpec(null, "Street");

        // Act
        var result = spec.Evaluate(GetOwners());

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2)); 
    }

    [Test]
    public void Constructor_WithPaging_ShouldApplySkipAndTake()
    {
        // Arrange
        var spec = new OwnersByFilterSpec(null, null, pageNumber: 2, pageSize: 2);

        // Act
        var result = spec.Evaluate(GetOwners());

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.First().Name, Is.EqualTo("Charlie"));
    }

    [Test]
    public void Constructor_WithNullFilters_ShouldReturnAll()
    {
        // Arrange
        var spec = new OwnersByFilterSpec(null, null);

        // Act
        var result = spec.Evaluate(GetOwners());

        // Assert
        Assert.That(result.Count(), Is.EqualTo(4));
    }

    private List<Owner> GetOwners()
    {
        return new List<Owner>
        {
            new Owner { Name = "Alice", Address = "Street 123" },
            new Owner { Name = "Bob", Address = "Avenue 456" },
            new Owner { Name = "Charlie", Address = "Street 789" },
            new Owner { Name = "David", Address = "Boulevard 123" }
        };
    }
}
