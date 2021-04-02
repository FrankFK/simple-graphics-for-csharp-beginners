﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TurtleCore.UnitTests
{
    [TestClass]
    public class Vec2DTest
    {

        [TestMethod]
        public void Vectors_With_Same_Values_Are_Equal()
        {
            // Arrange
            var vector1 = new Vec2D(1, 2);
            var vector2 = new Vec2D(1, 2);

            // Act
            bool areEqual = (vector1 == vector2);

            // Assert
            areEqual.Should().BeTrue();
        }


        [TestMethod]
        public void Add_TwoVectors_Works()
        {
            // Arrange
            var vector1 = new Vec2D(1, 2);
            var vector2 = new Vec2D(4, 5);

            // Act
            var result = vector1 + vector2;

            // Assert
            var expected = new Vec2D(5, 7);
            result.Should().Be(expected);
        }

        [TestMethod]
        public void Subtract_TwoVectors_Works()
        {
            // Arrange
            var vector1 = new Vec2D(1, 2);
            var vector2 = new Vec2D(4, 5);

            // Act
            var result = vector1 - vector2;

            // Assert
            var expected = new Vec2D(-3, -3);
            result.Should().Be(expected);
        }

        [TestMethod]
        public void Multiply_TwoVectors_Works()
        {
            // Arrange
            var vector1 = new Vec2D(2, 3);
            var vector2 = new Vec2D(4, 5);

            // Act
            var result = vector1 * vector2;

            // Assert
            var expected = new Vec2D(8, 15);
            result.Should().Be(expected);
        }

        [TestMethod]
        public void ScalarMultiply_Vector_Works()
        {
            // Arrange
            var vector = new Vec2D(2, 3);

            // Act
            var result = 4 * vector;

            // Assert
            var expected = new Vec2D(8, 12);
            result.Should().Be(expected);
        }

        [TestMethod]
        public void Negate_Vector_Works()
        {
            // Arrange
            var vector = new Vec2D(2, 3);

            // Act
            var result = -vector;

            // Assert
            var expected = new Vec2D(-2, -3);
            result.Should().Be(expected);
        }

        [TestMethod]
        public void Absolute_Of_Vector_Works()
        {
            // Arrange
            var vector = new Vec2D(3, 4);

            // Act
            var result = vector.AbsoluteValue;

            // Assert
            result.Should().Be(5);
        }

        [TestMethod]
        public void Rotate_Vector_Works()
        {
            // Arrange
            var vector = new Vec2D(3, 4);

            // Act
            var result = vector.Rotate(90);

            // Assert
            var expected = new Vec2D(-4, 3);
            result.XCor.Should().BeApproximately(expected.XCor, 0.001);
            result.YCor.Should().BeApproximately(expected.YCor, 0.001);
        }

    }
}
