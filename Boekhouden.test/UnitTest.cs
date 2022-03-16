using Boekhouden.UI;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NUnit.Framework;

namespace Boekhouden.test
{
    public class Tests
    {

        [TestCase]
        public void Test1()
        {
            // Arrange
            Inlezen_Json result = new Inlezen_Json
            {
                Sort = '+',
                Sum1 = 10,
                Sum2 = 10
            };
            // Act

            double actual = result.Calck();
            double expected = 20;
            // Assert
            Assert.AreEqual(expected, actual);

        }



        [Test]
        public void Test2()
        {
            // Arrange
            Inlezen_Json result = new Inlezen_Json
            {
                Sort = '-',
                Sum1 = 50,
                Sum2 = 10
            };
            // Act

            double actual = result.Calck();
            double expected = 40;
            // Assert
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void Test3(string inputJson, string outputJson)
        {

            // Arrange
            Inlezen_Json inlezen_Json= new Inlezen_Json();
            inlezen_Json.InLezen();
            // Act

            double actual = inputJson.Length;
            double expected = 40;
            // Assert
            Assert.AreEqual(expected, actual);

        }


    }
}