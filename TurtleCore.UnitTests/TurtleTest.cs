﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TurtleCore.UnitTests
{

    [TestClass]
    public class TurtleTest
    {
        private class TurtleScreenProducerMockup : IScreenObjectProducer
        {
            public List<ScreenLine> DrawnLines = new();
            public List<ScreenFigure> FigureUpdates = new();

            private int _figureCounter = 0;

            public int CreateLine()
            {
                return 1;
            }

            public void DrawLine(ScreenLine line)
            {
                DrawnLines.Add(line);
            }
            public int CreateFigure(ShapeBase shape)
            {
                _figureCounter++;
                return _figureCounter - 1;
            }

            public void UpdateFigure(ScreenFigure figure)
            {
                FigureUpdates.Add(figure);
            }


        }

        private static TurtleScreenProducerMockup _producerMockup;

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            Debug.WriteLine("Init start");
            _producerMockup = new TurtleScreenProducerMockup();
            TurtleOutputs.InitializeDefaultScreenObjectProducer(_producerMockup);
            Debug.WriteLine("Init end");
        }

        [TestCleanup]
        public void TestsCleanup()
        {
            Screen.ResetDefaultScreen();
        }


        [TestMethod]
        public void Turtle_InitialValues()
        {
            // Arrange

            // Act
            var turtle = CreateSut();

            // Assert
            var expected = new Vec2D(0, 0);
            turtle.Position.Should().Be(expected);
            turtle.Heading.Should().Be(0);

            // The turtle is immediately visible after creation!
            turtle.IsVisible.Should().BeTrue();
        }


        [TestMethod]
        public void Turtle_Forward_Works()
        {
            // Arrange
            var turtle = CreateSut();

            // Act
            turtle.Forward(25);

            // Assert
            var expected = new Vec2D(25, 0);
            turtle.Position.Should().Be(expected);

            // Act
            turtle.Forward(25);

            // Assert
            var expected2 = new Vec2D(50, 0);
            turtle.Position.Should().Be(expected2);
        }

        [TestMethod]
        public void Turtle_Backward_Works()
        {
            // Arrange
            var turtle = CreateSut();

            // Act
            turtle.Backward(25);

            // Assert
            var expected = new Vec2D(-25, 0);
            turtle.Position.Should().Be(expected);

            // Act
            turtle.Backward(25);

            // Assert
            var expected2 = new Vec2D(-50, 0);
            turtle.Position.Should().Be(expected2);
        }

        [TestMethod]
        public void Turtle_Left_Works()
        {
            // Arrange
            var turtle = CreateSut();

            // Act
            turtle.Left(90);

            // Assert
            turtle.Heading.Should().Be(90);

            turtle.Forward(100);
            var expextedPostion = new Vec2D(0, 100);
            turtle.Position.IsApproximatelyEqualTo(expextedPostion, 0.001).Should().BeTrue();
        }

        [TestMethod]
        public void Turtle_LeftWithMoreThan360Degrees_Works()
        {
            // Arrange
            var turtle = CreateSut();

            // Act
            turtle.Left(450);

            // Assert
            turtle.Heading.Should().Be(90);
        }

        [TestMethod]
        public void Turtle_Right_Works()
        {
            // Arrange
            var turtle = CreateSut();

            // Act
            turtle.Right(90);

            // Assert
            turtle.Heading.Should().Be(270);

            turtle.Forward(100);
            var expextedPostion = new Vec2D(0, -100);
            turtle.Position.IsApproximatelyEqualTo(expextedPostion, 0.001).Should().BeTrue();
        }

        [TestMethod]
        public void Turtle_InitiallyPenIsDown()
        {
            _producerMockup.DrawnLines.Clear();

            // Arrange
            var turtle = CreateSut();

            // Act
            turtle.Forward(100);

            // Assert
            _producerMockup.DrawnLines.Count.Should().Be(1);
        }

        [TestMethod]
        public void Turtle_WhenPenIsUpNoLineIsDrawn()
        {
            _producerMockup.DrawnLines.Clear();

            // Arrange
            var turtle = CreateSut();

            // Act
            turtle.PenUp();
            turtle.Forward(100);

            // Assert
            _producerMockup.DrawnLines.Count.Should().Be(0);
        }

        [TestMethod]
        public void Turtle_MoveAfterPenDownIsDrawn()
        {
            _producerMockup.DrawnLines.Clear();

            // Arrange
            var turtle = CreateSut();

            // Act
            turtle.PenUp();
            turtle.Forward(100);
            turtle.Left(90);
            turtle.PenDown();
            turtle.Forward(50);

            // Assert
            _producerMockup.DrawnLines.Count.Should().Be(1);
            var line = _producerMockup.DrawnLines[0];
            line.Point1.IsApproximatelyEqualTo(new Vec2D(100, 0), 0).Should().BeTrue();
            line.Point2.IsApproximatelyEqualTo(new Vec2D(100, 50), 0).Should().BeTrue();
        }

        [TestMethod]
        public void Turtle_PositionChangeWithPendownIsDrawn()
        {
            _producerMockup.DrawnLines.Clear();

            // Arrange
            var turtle = CreateSut();

            // Act
            turtle.PenDown();
            turtle.SetPosition(new Vec2D(0, 100));

            // Assert
            _producerMockup.DrawnLines.Count.Should().Be(1);
        }

        [TestMethod]
        public void Turtle_InitiallyATurtleIsShown()
        {
            _producerMockup.FigureUpdates.Clear();

            // Act
            var turtle = CreateSut();

            // Assert
            _producerMockup.FigureUpdates.Count.Should().Be(1);
            _producerMockup.FigureUpdates[0].IsVisible.Should().BeTrue();
        }


        private static Turtle CreateSut()
        {
            return new Turtle();
        }
    }
}
