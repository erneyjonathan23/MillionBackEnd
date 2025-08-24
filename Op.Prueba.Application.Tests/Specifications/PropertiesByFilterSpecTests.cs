namespace Op.Prueba.Application.Tests.Specifications;

using NUnit.Framework;
using Op.Prueba.Application.Specifications;
using Op.Prueba.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class PropertiesByFilterSpecTests
{

    [Test]
    public void Constructor_WithName_ShouldFilterByName()
    {
        // Arrange
        var spec = new PropertiesByFilterSpec("House A", null, null, null);

        // Act
        var result = spec.Evaluate(GetProperties());

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Name, Is.EqualTo("House A"));
    }

    [Test]
    public void Constructor_WithAddress_ShouldFilterByAddress()
    {
        // Arrange
        var spec = new PropertiesByFilterSpec(null, "Street", null, null);

        // Act
        var result = spec.Evaluate(GetProperties());

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));  
    }

    [Test]
    public void Constructor_WithPriceRange_ShouldFilterByMinAndMaxPrice()
    {
        // Arrange
        var spec = new PropertiesByFilterSpec(null, null, 150000, 350000);

        // Act
        var result = spec.Evaluate(GetProperties());

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2)); 
    }

    [Test]
    public void Constructor_WithPaging_ShouldApplySkipTakeAndOrder()
    {
        // Arrange
        var spec = new PropertiesByFilterSpec(null, null, null, null, pageNumber: 2, pageSize: 2);

        // Act
        var result = spec.Evaluate(GetProperties()).ToList();

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("House C"));  
        Assert.That(result[1].Name, Is.EqualTo("House D"));
    }

    [Test]
    public void Constructor_WithNoFilters_ShouldReturnAll()
    {
        // Arrange
        var spec = new PropertiesByFilterSpec(null, null, null, null);

        // Act
        var result = spec.Evaluate(GetProperties());

        // Assert
        Assert.That(result.Count(), Is.EqualTo(4));
    }
    private List<Property> GetProperties()
    {
        return new List<Property>
        {
            new Property { Name = "House A", Address = "Street 1", Price = 100000 },
            new Property { Name = "House B", Address = "Avenue 2", Price = 200000 },
            new Property { Name = "House C", Address = "Street 3", Price = 300000 },
            new Property { Name = "House D", Address = "Boulevard 4", Price = 400000 }
        };
    }
}

