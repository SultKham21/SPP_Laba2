using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Faker.UnitTests.TestClasses;
using Faker.ValueGenerators.CustomGenerators;

namespace Faker.UnitTests
{
    [TestClass]
    public class FakerUnitTest
    {
        private Faker faker;

        [TestInitialize]
        public void Setup()
        {
            faker = new Faker();
        }

        [TestMethod]
        public void NullableFieldsTest()
        {
            NullableFieldsNoConstructor noConstructorObject = faker.Create<NullableFieldsNoConstructor>();
            Assert.AreNotEqual(null, noConstructorObject.dateTimeField);
            Assert.AreNotEqual(null, noConstructorObject.stringField);
            Assert.AreNotEqual(null, noConstructorObject.objectField);

            NullableFieldsWithConstructor constructorObject = faker.Create<NullableFieldsWithConstructor>();
            Assert.AreNotEqual(null, constructorObject.stringField);
            Assert.AreEqual(default(DateTime), constructorObject.dateTimeField);
            Assert.AreEqual(default(object), constructorObject.objectField);
        }

        [TestMethod]
        public void NullablePropertiesTest()
        {
            NullablePropertiesNoConstructor noConstructorObject = faker.Create<NullablePropertiesNoConstructor>();
            Assert.AreNotEqual(null, noConstructorObject.ObjectProperty);
            Assert.AreNotEqual(1, noConstructorObject.StringProperty);
            Assert.AreNotEqual(null, noConstructorObject.DateTimeProperty);

            NullablePropertiesWithConstructor constructorObject = faker.Create<NullablePropertiesWithConstructor>();
            Assert.AreNotEqual(null, constructorObject.DateTimeProperty);
            Assert.AreEqual(default(string), constructorObject.StringProperty);
            Assert.AreEqual(default(object), constructorObject.ObjectProperty);
        }

        [TestMethod]
        public void SelfRecursionTest()
        {
            SelfRecursiveClassNoConstructor noConstructor = faker.Create<SelfRecursiveClassNoConstructor>();
            Assert.AreEqual(null, noConstructor.innerObject);
            SelfRecursiveClassWithConstructor selfRecursive = faker.Create<SelfRecursiveClassWithConstructor>();
            Assert.AreEqual(null, selfRecursive.innerObject);
        }

        [TestMethod]
        public void IndirectRecursiongTest()
        {
            IndirectRecursiveClass1 indirectRecursiveObject = faker.Create<IndirectRecursiveClass1>();
            Assert.AreEqual(null, indirectRecursiveObject.InnerObject.InnerObject);
        }



        [TestMethod]
        public void CustomGenerationTest()
        {
            IFakerConfig config = new FakerConfig();
            config.Add<CustomGenerationPropertyClass, int, IntNonRandomGenerator>(cl => cl.SomeValue);
            config.Add<CustomGenerationConstructorClass, int, IntNonRandomGenerator>(cl => cl.SomeValue2);
            Assert.ThrowsException<ArgumentException>(() => config.Add<CustomGenerationPropertyClass, string, IntNonRandomGenerator>(err => err.SomeString));

            faker = new Faker(config);

            CustomGenerationPropertyClass propertyClass = faker.Create<CustomGenerationPropertyClass>();
            Assert.AreEqual(IntNonRandomGenerator.DefaultGeneratedValue, propertyClass.SomeValue);
            Assert.AreEqual(IntNonRandomGenerator.DefaultGeneratedValue, propertyClass.someValue);

            CustomGenerationConstructorClass constructorClass = faker.Create<CustomGenerationConstructorClass>();
            Assert.AreEqual(IntNonRandomGenerator.DefaultGeneratedValue, constructorClass.SomeValue2);
            Assert.AreNotEqual(IntNonRandomGenerator.DefaultGeneratedValue, constructorClass.SomeValue);
            Assert.AreNotEqual(IntNonRandomGenerator.DefaultGeneratedValue, constructorClass.someValue);
        }
    }
}
