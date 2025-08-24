namespace Op.Prueba.Application.Tests.Behaviours;

using FluentValidation;
using FluentValidation.Results;
using MediatR;
using NUnit.Framework;
using OP.Prueba.Application.Behaviours;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class ValidationBehaviorTests
{
    [Test]
    public async Task Handle_NoValidators_ShouldCallNext()
    {
        // Arrange
        var validators = Enumerable.Empty<IValidator<TestRequest>>();
        var behavior = new ValidationBehavior<TestRequest, string>(validators);

        var expectedResponse = "ok";
        RequestHandlerDelegate<string> next = () => Task.FromResult(expectedResponse);

        var request = new TestRequest { Name = "test" };

        // Act
        var response = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        Assert.AreEqual(expectedResponse, response);
    }

    [Test]
    public async Task Handle_ValidatorsWithNoErrors_ShouldCallNext()
    {
        // Arrange
        var spyValidator = new SpyValidator();
        var validators = new List<IValidator<TestRequest>> { spyValidator };

        var behavior = new ValidationBehavior<TestRequest, string>(validators);

        var expectedResponse = "ok";
        RequestHandlerDelegate<string> next = () => Task.FromResult(expectedResponse);

        var request = new TestRequest { Name = "valid" };

        // Act
        var response = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        Assert.AreEqual(expectedResponse, response);
        Assert.IsTrue(spyValidator.WasCalled);
    }

    private class SpyValidator : AbstractValidator<TestRequest>
    {
        public bool WasCalled { get; private set; }

        public SpyValidator()
        {
            RuleFor(x => x.Name).NotNull();
        }

        public override Task<ValidationResult> ValidateAsync(ValidationContext<TestRequest> context, CancellationToken cancellation = default)
        {
            WasCalled = true;
            return Task.FromResult(new ValidationResult());
        }
    }

    private class TestRequest : IRequest<string>
    {
        public string Name { get; set; } = string.Empty;
    }
}
