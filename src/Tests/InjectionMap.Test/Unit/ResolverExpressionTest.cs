﻿using InjectionMap.Expressions;
using NUnit.Framework;
using System.Linq;

namespace InjectionMap.Test.Unit
{
    [TestFixture]
    public class ResolverExpressionTest
    {
        [SetUp]
        public void Initialize()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Clean<IResolverExpressionMock>();
            }
        }


        [Test]
        [Description("Creates a map that allready containes arguments. Overrides an argument by supplying the same name")]
        public void ResolverExpressionAddArgumentByNameThatExists()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Map<IResolverExpressionMock, ResolverExpression>().WithArgument("id", () => 1);
            }

            using (var resolver = new InjectionResolver())
            {
                var map = resolver.ExtendMap<IResolverExpressionMock>();
                var re = map.WithArgument("id", () => 2);
                Assert.IsNotNull(re);

                var ce = re as IComponentExpression;
                Assert.IsNotNull(ce);

                Assert.IsTrue(ce.Component.Arguments.Count == 1);
                Assert.IsTrue((int)ce.Component.Arguments.First().Callback.Compile().Invoke() == 2);
                Assert.AreSame(ce.Component.Arguments.First().Name, "id");
            }
        }

        [Test]
        [Description("Creates a map that allready containes arguments. Overrides an argument by supplying the same type argument")]
        public void ResolverExpressionAddArgumentByValueThatExists()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Map<IResolverExpressionMock, ResolverExpression>().WithArgument("id", () => 1);
            }

            using (var resolver = new InjectionResolver())
            {
                var map = resolver.ExtendMap<IResolverExpressionMock>();
                var re = map.WithArgument(() => 2);
                Assert.IsNotNull(re);

                var ce = re as IComponentExpression;
                Assert.IsNotNull(ce);

                Assert.IsTrue(ce.Component.Arguments.Count == 2);
                Assert.IsTrue((int)ce.Component.Arguments.First().Callback.Compile().Invoke() == 1);
                Assert.AreSame(ce.Component.Arguments.First().Name, "id");
                Assert.IsNull(ce.Component.Arguments.Last().Name);
            }
        }

        [Test]
        [Description("Creates a map that allready containes arguments. Overrides an argument by supplying the same type argument")]
        public void ResolverExpressionAddArgumentByValueThatExistsWithoutName()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Map<IResolverExpressionMock, ResolverExpression>().WithArgument(() => 1);
            }

            using (var resolver = new InjectionResolver())
            {
                var map = resolver.ExtendMap<IResolverExpressionMock>();
                var re = map.WithArgument(() => 2);
                Assert.IsNotNull(re);

                var ce = re as IComponentExpression;
                Assert.IsNotNull(ce);

                Assert.IsTrue(ce.Component.Arguments.Count == 2);
                Assert.IsTrue((int)ce.Component.Arguments.First().Callback.Compile().Invoke() == 1);
            }
        }



        [Test]
        [Description("IResolverExpression<T> WithArgument<TArg>(TArg value);")]
        public void ResolverExpressionWithArgumentValue()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Map<IResolverExpressionMock, ResolverExpression>();
            }

            using (var resolver = new InjectionResolver())
            {
                var map = resolver.ExtendMap<IResolverExpressionMock>();
                var re = map.WithArgument(1);
                Assert.IsNotNull(re);

                var ce = re as IComponentExpression;
                Assert.IsNotNull(ce);

                Assert.IsTrue((int)ce.Component.Arguments.First().Value == 1);
                Assert.IsNull(ce.Component.Arguments.First().Name);
                Assert.IsNull(ce.Component.Arguments.First().Callback);
            }
        }

        [Test]
        [Description("IResolverExpression<T> WithArgument<TArg>(string name, TArg value);")]
        public void ResolverExpressionWithArgumentNameAndValue()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Map<IResolverExpressionMock, ResolverExpression>();
            }

            using (var resolver = new InjectionResolver())
            {
                var map = resolver.ExtendMap<IResolverExpressionMock>();
                var re = map.WithArgument("id", 2);
                Assert.IsNotNull(re);

                var ce = re as IComponentExpression;
                Assert.IsNotNull(ce);

                Assert.IsTrue((int)ce.Component.Arguments.First().Value == 2);
                Assert.AreSame(ce.Component.Arguments.First().Name, "id");
                Assert.IsNull(ce.Component.Arguments.First().Callback);
            }
        }

        [Test]
        [Description("IResolverExpression<T> WithArgument<TArg>(Expression<Func<TArg>> predicate);")]
        public void ResolverExpressionWithArgumentValueExpression()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Map<IResolverExpressionMock, ResolverExpression>();
            }

            using (var resolver = new InjectionResolver())
            {
                var map = resolver.ExtendMap<IResolverExpressionMock>();
                var re = map.WithArgument(() => 3);
                Assert.IsNotNull(re);

                var ce = re as IComponentExpression;
                Assert.IsNotNull(ce);

                Assert.IsTrue((int)ce.Component.Arguments.First().Callback.Compile().Invoke() == 3);
                Assert.IsNull(ce.Component.Arguments.First().Name);
            }
        }

        [Test]
        [Description("IResolverExpression<T> WithArgument<TArg>(string name, Expression<Func<TArg>> predicate);")]
        public void ResolverExpressionWithArgumentNameAndValueExpression()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Map<IResolverExpressionMock, ResolverExpression>();
            }

            using (var resolver = new InjectionResolver())
            {
                var map = resolver.ExtendMap<IResolverExpressionMock>();
                var re = map.WithArgument("id", () => 4);
                Assert.IsNotNull(re);

                var ce = re as IComponentExpression;
                Assert.IsNotNull(ce);

                Assert.IsTrue((int)ce.Component.Arguments.First().Callback.Compile().Invoke() == 4);
                Assert.AreSame(ce.Component.Arguments.First().Name, "id");
            }
        }

        [Test]
        [Description("T Resolve();")]
        public void ResolverExpressionResolve()
        {
            using (var mapper = new InjectionMapper())
            {
                mapper.Map<IResolverExpressionMock, ResolverExpression>();
            }

            using (var resolver = new InjectionResolver())
            {
                var map = resolver.ExtendMap<IResolverExpressionMock>();
                var re = map.WithArgument("id", () => 5);
                var value = re.Resolve();

                Assert.IsTrue(value.ID == 5);
            }
        }


        internal interface IResolverExpressionMock
        {
            int ID { get; set; }
        }

        internal class ResolverExpression : IResolverExpressionMock
        {
            public ResolverExpression()
            {
            }
            public ResolverExpression(int id)
            {
                ID = id;
            }

            public int ID { get; set; }
        }
    }
}
