﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TurtleCore;

namespace TurtleWpf
{
    internal class WpfScreenObjectWriter : IScreenObjectWriter
    {

        private readonly Canvas _canvas;

        private readonly CanvasLines _canvasLines;
        private readonly CanvasPathes _canvasPathes;

        public WpfScreenObjectWriter(Canvas canvas)
        {
            _canvas = canvas;
            _canvasLines = new CanvasLines(canvas.Width, canvas.Height);
            _canvasPathes = new CanvasPathes(canvas.Width, canvas.Height);
        }

        public void UpdateWithAnimation(ScreenObject screenObject)
        {
            if (!screenObject.HasAnimations)
                // The screenObject only has waited for other animations to be finished. But it has no animations itself.
                // We can handle it with a normal Update:
                Update(screenObject);
            else
            {
                CanvasChildrenChange result;
                if (screenObject is ScreenLine)
                    result = _canvasLines.UpdateWithAnimation(screenObject as ScreenLine, OnAnimationIsFinished);
                else if (screenObject is ScreenFigure)
                    result = _canvasPathes.UpdateWithAnimation(screenObject as ScreenFigure, OnAnimationIsFinished);
                else
                    throw new ArgumentOutOfRangeException(nameof(screenObject), "Parameter has wrong type");

                UpdateCanvasChildren(result);
            }
        }

        public event AnimationIsFinished OnAnimationIsFinished;

        public void Update(ScreenObject screenObject)
        {
            CanvasChildrenChange result;
            if (screenObject is ScreenFigureCreate)
                result = _canvasPathes.CreatePath(screenObject as ScreenFigureCreate);
            else if (screenObject is ScreenFigure)
                result = _canvasPathes.Update(screenObject as ScreenFigure);
            else
                throw new ArgumentOutOfRangeException(nameof(screenObject), "Parameter has wrong type");

            UpdateCanvasChildren(result);
        }


        private void UpdateCanvasChildren(CanvasChildrenChange change)
        {
            switch (change.Operation)
            {
                case Operation.Add:
                    if (!_canvas.Children.Contains(change.Element))
                    {
                        _canvas.Children.Add(change.Element);
                    }
                    break;
                case Operation.Remove:
                    _canvas.Children.Remove(change.Element);
                    break;
                case Operation.Nothing:
                    break;
                default:
                    break;
            }
        }

    }
}
