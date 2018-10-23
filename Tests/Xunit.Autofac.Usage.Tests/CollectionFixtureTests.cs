using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Xunit.Autofac.Usage.Tests
{
    [Collection(nameof (FixtureCollection))]
    public class CollectionFixtureTests
    {
        private readonly Fixture _fixture;
        private readonly ComplexObject _complexObject;

        public CollectionFixtureTests(Fixture fixture, ComplexObject complexObject)
        {
            _fixture = fixture;
            _complexObject = complexObject;
        }

        [Fact]
        public void CheckReferenceEqualityOfCollectionFixtureObjects()
        {
            bool actual = object.ReferenceEquals(_fixture, _complexObject?.Fixture);

            Assert.NotNull(_fixture);
            Assert.NotNull(_complexObject);
            Assert.NotNull(_complexObject.Fixture);
            Assert.True(actual);
        }
    }

    public class Fixture
    {
    }

    public class ComplexObject
    {
        public Fixture Fixture { get; }

        public ComplexObject(Fixture fixture)
        {
            Fixture = fixture;
        }
    }

    [CollectionDefinition(nameof (FixtureCollection))]
    public class FixtureCollection : ICollectionFixture<Fixture>
    {
    }

    // ReSharper disable once UnusedMember.Global
    public class ServiceRegistration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<Fixture>()
                .AsSelf()
                .InstancePerTestCollection();

            builder
                .RegisterType<ComplexObject>()
                .AsSelf()
                .InstancePerTestCollection();
        }
    }
}
