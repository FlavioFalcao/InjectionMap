﻿using NUnit.Framework;

namespace InjectionMap.Test.Integration
{
    [TestFixture]
    public class ExtendMapTest : TestBase
    {
        [SetUp]
        public void Initialize()
        {
            Mapper.Clean<IExtendMap>();
        }

        [Test]
        public void ExtendMapWithArguments()
        {
            Mapper.Map<IExtendMap, ExtendMapMock>();

            var value = Resolver.ExtendMap<IExtendMap>().WithArgument("id", () => 1).WithArgument("name", () => "test1").Resolve();
            Assert.IsTrue(value.ID == 1);
            Assert.IsTrue(value.Name == "test1");

            value = Resolver.ExtendMap<IExtendMap>().WithArgument("id", 2).WithArgument("name", "test2").Resolve();
            Assert.IsTrue(value.ID == 2);
            Assert.IsTrue(value.Name == "test2");

            value = Resolver.ExtendMap<IExtendMap>().WithArgument(() => 3).WithArgument(() => "test3").Resolve();

            // property name is not provided. id should still be 2 and name test2
            Assert.IsFalse(value.ID == 3);
            Assert.IsFalse(value.Name == "test3");

            value = Resolver.ExtendMap<IExtendMap>().WithArgument("id", () => 3).WithArgument("name", () => "test3").Resolve();
            Assert.IsTrue(value.ID == 3);
            Assert.IsTrue(value.Name == "test3");

            value = Resolver.ExtendMap<IExtendMap>().WithArgument(4).WithArgument("test4").Resolve();

            // property name is not provided. id should still be 3 and name test3
            Assert.IsFalse(value.ID == 4);
            Assert.IsFalse(value.Name == "test4");
        }

        [Test]
        public void ExtendMapThatHasExistingArguments()
        {
            Mapper.Map<IExtendMap, ExtendMapMock>().WithArgument("test");

            var value = Resolver.ExtendMap<IExtendMap>().WithArgument("id", () => 1).Resolve();
            Assert.IsTrue(value.ID == 1);
            Assert.IsTrue(value.Name == "test");

            value = Resolver.ExtendMap<IExtendMap>().WithArgument("id", () => 1).WithArgument("name", () => "test1").Resolve();
            Assert.IsTrue(value.ID == 1);
            Assert.IsTrue(value.Name == "test1");

            value = Resolver.ExtendMap<IExtendMap>().WithArgument("id", 2).WithArgument("name", "test2").Resolve();
            Assert.IsTrue(value.ID == 2);
            Assert.IsTrue(value.Name == "test2");

            value = Resolver.ExtendMap<IExtendMap>().WithArgument(() => 3).WithArgument(() => "test3").Resolve();

            // property name is not provided. id should still be 2 and name test2
            Assert.IsFalse(value.ID == 3);
            Assert.IsFalse(value.Name == "test3");

            value = Resolver.ExtendMap<IExtendMap>().WithArgument(4).WithArgument("test4").Resolve();

            // property name is not provided. id should still be 2 and name test2
            Assert.IsFalse(value.ID == 4);
            Assert.IsFalse(value.Name == "test4");
        }

        [Test]
        public void ExtendMapWithOverrideExistingArguments()
        {
            Mapper.Map<IExtendMap, ExtendMapMock>().WithArgument("id", () => 1);

            var value = Resolver.ExtendMap<IExtendMap>().WithArgument("name", () => "test").WithArgument("id", () => 2).Resolve();
            Assert.IsTrue(value.ID == 2);
            Assert.IsTrue(value.Name == "test");
        }

        [Test]
        public void ExtendMapOnNonexistantMapWithConcreteType()
        {
            var value = Resolver.ExtendMap<ExtendMapNotMapped>().Resolve();
            Assert.IsTrue(value.ID == 1);
            Assert.IsTrue(value.Name == "test");
        }

        [Test]
        [ExpectedException(typeof(ResolverException))]
        public void ExtendMapOnNonexistantMapWithInterface()
        {
            var value = Resolver.ExtendMap<IExtendMap>().Resolve();
            Assert.IsTrue(value.ID == 1);
            Assert.IsTrue(value.Name == "test");
        }

        #region Mocks

        private interface IExtendMap
        {
            int ID { get; set; }

            string Name { get; set; }
        }

        private class ExtendMapMock : IExtendMap
        {
            public ExtendMapMock(int id, string name)
            {
                ID = id;
                Name = name;
            }

            public int ID { get; set; }

            public string Name { get; set; }
        }

        private class ExtendMapNotMapped : IExtendMap
        {
            public ExtendMapNotMapped()
            {
                ID = 1;
                Name = "test";
            }

            public int ID { get; set; }

            public string Name { get; set; }
        }

        #endregion
    }
}
