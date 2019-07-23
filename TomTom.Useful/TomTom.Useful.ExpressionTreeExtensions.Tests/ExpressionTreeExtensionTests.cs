using System;
using System.Linq.Expressions;
using Xunit;

namespace TomTom.Useful.ExpressionTreeExtensions.Tests
{
    public class ExpressionTreeExtensionTests
    {
        [Fact]
        public void ExtendParameter_should_extend_member_expression()
        {
            // arrange
            Expression<Func<SomeNestedClass, int>> sut =
                c => c.Id;
            var someClass = new SomeClass
            {
                Nested = new SomeNestedClass
                {
                    Id = new Random().Next()
                }
            };

            // act
            var result = sut.ReplaceParameter((SomeClass c) => c.Nested);

            // assert
            Assert.Equal(sut.Compile()(someClass.Nested), result.Compile()(someClass));
        }

        [Fact]
        public void ExtendParameter_should_extend_predicate()
        {
            // arrange
            Expression<Func<SomeNestedClass, bool>> sut =
                c => c.Id > 80;

            var someClass1 = new SomeClass
            {
                Nested = new SomeNestedClass
                {
                    Id = 80
                }
            };
            var someClass2 = new SomeClass
            {
                Nested = new SomeNestedClass
                {
                    Id = 81
                }
            };

            // act
            var result = sut.ReplaceParameter((SomeClass c) => c.Nested);

            // assert
            Assert.Equal(sut.Compile()(someClass1.Nested), result.Compile()(someClass1));
            Assert.Equal(sut.Compile()(someClass2.Nested), result.Compile()(someClass2));
        }

        [Fact]
        public void ExtendParameter_should_extend_complex_predicate()
        {
            // arrange
            Expression<Func<SomeNestedClass, bool>> sut =
                c => c.Id > 80 && c.Id < 88 && c.Name == "Soso";

            // act
            var result = sut.ReplaceParameter((SomeClass c) => c.Nested);

            // assert
            Assert.True(result.Compile()(new SomeClass
            {
                Nested = new SomeNestedClass
                {
                    Name = "Soso",
                    Id = 85
                }
            }));

            Assert.False(result.Compile()(new SomeClass
            {
                Nested = new SomeNestedClass
                {
                    Name = "Sosa",
                    Id = 85
                }
            }));
        }

        [Fact]
        public void ExtendParameter_should_extend_with_nested_parameter()
        {
            // arrange
            Expression<Func<SomeNestedNestedClass, bool>> sut =
                c => c.Id > 80 && c.Id < 82;

            // act
            var result = sut.ReplaceParameter((SomeClass c) => c.Nested.Nested2);

            // assert
            Assert.False(result.Compile()(new SomeClass
            {
                Nested = new SomeNestedClass
                {
                    Nested2 = new SomeNestedNestedClass
                    {
                        Id = 99
                    }
                }
            }));
            Assert.True(result.Compile()(new SomeClass
            {
                Nested = new SomeNestedClass
                {
                    Nested2 = new SomeNestedNestedClass
                    {
                        Id = 81
                    }
                }
            }));
        }

        private class SomeClass
        {
            public SomeNestedClass Nested { get; set; }

            public int Id { get; set; }
        }

        private class SomeNestedClass
        {
            public SomeNestedNestedClass Nested2 { get; set; }

            public int Id { get; set; }

            public string Name { get; set; }
        }

        private class SomeNestedNestedClass
        {
            public int Id { get; set; }
        }
    }
}
