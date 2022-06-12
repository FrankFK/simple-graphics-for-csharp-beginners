﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Woopec.Core.Internal;

namespace Woopec.Core.UnitTests
{
    [TestClass]
    public class ScreenTest
    {

        private class TurtleScreenProducerMockup : IScreenObjectProducer
        {
            public List<ScreenLine> DrawnLines = new();

            public int CreateLine()
            {
                return 1;
            }
            public void DrawLine(ScreenLine line)
            {
                DrawnLines.Add(line);
            }

            public int CreateFigure()
            {
                throw new NotImplementedException();
            }

            public void UpdateFigure(ScreenFigure figure)
            {
                throw new NotImplementedException();
            }

            public void ShowDialog(ScreenDialog dialog)
            {
                throw new NotImplementedException();
            }

            public void ShowNumberDialog(ScreenNumberDialog dialog)
            {
                throw new NotImplementedException();
            }
        }

        private class TurtleScreenResultConsumerMockup : IScreenResultConsumer
        {
            public Task<double?> ReadNumberResultAsync()
            {
                throw new NotImplementedException();
            }

            public Task<string> ReadTextResultAsync()
            {
                return Task.FromResult("Don't care");
            }
        }

        private static TurtleScreenProducerMockup _producerMockup;
        private static TurtleScreenResultConsumerMockup _resultConsumerMockup;

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            _producerMockup = new TurtleScreenProducerMockup();
            _resultConsumerMockup = new TurtleScreenResultConsumerMockup();
            TurtleInputsAndOutputs.InitializeDefaultScreenObjectProducer(_producerMockup);
            TurtleInputsAndOutputs.InitializeDefaultScreenResultConsumer(_resultConsumerMockup);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            LowLevelScreen.ResetDefaultScreen();
        }

        [TestMethod]
        public void Screen_ThereArePrefefinedShapes()
        {
            Shapes.Turtle.Name.Should().NotBeNullOrWhiteSpace();
            Shapes.Arrow.Name.Should().NotBeNullOrWhiteSpace();
            Shapes.Circle.Name.Should().NotBeNullOrWhiteSpace();
            Shapes.Square.Name.Should().NotBeNullOrWhiteSpace();
            Shapes.Classic.Name.Should().NotBeNullOrWhiteSpace();
            Shapes.Triangle.Name.Should().NotBeNullOrWhiteSpace();
            Shapes.Bird.Name.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void Screen_AddPolygonShape()
        {
            var screen = LowLevelScreen.GetDefaultScreen();
            screen.AddShape("polygon", new Shape(new() { (-10, 0), (10, 0), (0, 10) }));
            screen.GetShapes().Should().Contain("polygon");
        }

        [TestMethod]
        public void Screen_AddCompoundShape()
        {
            var screen = LowLevelScreen.GetDefaultScreen();

            var shape = new Shape();
            shape.AddComponent(new() { (0, 0), (10, -5), (0, 10), (-10, -5) }, Colors.Blue, Colors.Yellow);
            shape.AddComponent(new() { (0, 0), (-10, 5), (0, -10), (10, 5) }, Colors.Yellow, Colors.Blue);

            screen.AddShape("compound shape", shape);
            screen.GetShapes().Should().Contain("compound shape");
        }
    }
}
