using Boekhouden.UI;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NUnit.Framework;

namespace Boekhouden.test
{
    public class Tests
    {

        [Test]
        public void Test1()
        {
            // Arrange
            Inlezen_Json sut = new Inlezen_Json
            {
                Gender = 'm',
                Height = 180
            };
            // Act

            double actual = sut.InLezen();
            double expected = 72.5;
            // Assert
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void Test2()
        {
            // Arrange
            Inlezen_Json sut = new Inlezen_Json
            {
                Gender = 'm',
                Height = 180
            };
            // Act

            double actual = sut.GetIdealBodyWeight();
            double expected = 72.5;
            // Assert
            Assert.AreEqual(expected, actual);

        }

    }
}