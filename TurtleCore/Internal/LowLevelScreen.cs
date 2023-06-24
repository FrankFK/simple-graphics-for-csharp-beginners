﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Core.Internal
{
    /// <summary>
    /// An instance of this class represents the screen to which screen objects (lines, shapes, ...) are drawn
    /// </summary>
    /// <remarks>
    /// This class only is used internally.The users of the woopec library are using the Screen class.
    /// </remarks>
    internal class LowLevelScreen : ILowLevelScreen
    {
        private readonly IScreenObjectProducer _screenObjectProducer;
        private readonly IScreenResultConsumer _screenResultConsumer;

        public LowLevelScreen(IScreenObjectProducer screenObjectProducer, IScreenResultConsumer screenResultConsumer)
        {
            if (screenObjectProducer == null || screenResultConsumer == null)
            {
                // This situation occurs if Figure, Pen or Turtle are used unit tests without any preperation for that.
                var recommendedClass = nameof(Screen);
                var recommendedMethod = nameof(Screen.SwitchToUnitTestDefaultScreen);
                throw new ArgumentException($"Woopec: The arguments for LowLevelScreen are invalid. If you need a screen for unit tests, it is best to call method {recommendedMethod} of class {recommendedClass} at the beginning.");
            }
            _screenObjectProducer = screenObjectProducer ?? throw new ArgumentNullException("producer");
            _screenResultConsumer = screenResultConsumer ?? throw new ArgumentNullException(nameof(screenResultConsumer));
            LastIssuedAnimatonGroupID = ScreenObject.NoGroupId;
        }


        #region Methods of ILowLevelScreen
        ///<inheritdoc/>
        public int LastIssuedAnimatonGroupID { get; set; }

        ///<inheritdoc/>
        public int CreateLine()
        {
            return _screenObjectProducer.CreateLine();
        }

        ///<inheritdoc/>
        public void DrawLine(ScreenLine line)
        {
            UpdateLastIssuedAnimationGroupID(line);
            _screenObjectProducer.DrawLine(line);
        }

        ///<inheritdoc/>
        public int CreateFigure()
        {
            return _screenObjectProducer.CreateFigure();
        }

        ///<inheritdoc/>
        public void UpdateFigure(ScreenFigure figure)
        {
            UpdateLastIssuedAnimationGroupID(figure);
            _screenObjectProducer.UpdateFigure(figure);
        }

        ///<inheritdoc/>
        public async Task<string> TextInputAsync(string title, string prompt, Vec2D position)
        {
            var dialog = new ScreenDialog() { Title = title, Prompt = prompt, Position = position };
            // If we do not wait for another animation this dialog is shown immediately. In most cases the programmer expects
            // that all previously created animation are drawn before the dialog is shown
            // Therefore:
            dialog.WaitForCompletedAnimationsOfAnotherGroup = LastIssuedAnimatonGroupID;
            _screenObjectProducer.ShowDialog(dialog);
            var answer = await _screenResultConsumer.ReadTextResultAsync();
            return answer;
        }

        ///<inheritdoc/>
        public async Task<double?> NumberInputAsync(ScreenNumberDialog dialog)
        {
            dialog.WaitForCompletedAnimationsOfAnotherGroup = LastIssuedAnimatonGroupID;
            _screenObjectProducer.ShowNumberDialog(dialog);
            var answer = await _screenResultConsumer.ReadNumberResultAsync();
            return answer;
        }



        #endregion


        private void UpdateLastIssuedAnimationGroupID(ScreenObject screenObject)
        {
            if (screenObject.HasAnimations)
            {
                LastIssuedAnimatonGroupID = screenObject.GroupID;
            }
        }
    }
}
