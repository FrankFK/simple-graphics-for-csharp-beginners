﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Core.Internal
{
    internal class ScreenResultConsumer : IScreenResultConsumer
    {
        private readonly IScreenResultChannel _screenResultChannel;

        public ScreenResultConsumer(IScreenResultChannel screenResultChannel)
        {
            _screenResultChannel = screenResultChannel;
        }

        public async Task<string> ReadTextResultAsync()
        {
            ScreenResultText screenResultText = null;
            try
            {
                var screenResult = await _screenResultChannel.ReadAsync();
                screenResultText = screenResult as ScreenResultText;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ScreenResultConsumer ReadTextResultAsync: Exception " + ex.Message);
                throw;
            }
            return screenResultText.Text;
        }

        public async Task<double?> ReadNumberResultAsync()
        {
            ScreenResultNumber screenResultNumber = null;
            try
            {
                var screenResult = await _screenResultChannel.ReadAsync();
                screenResultNumber = screenResult as ScreenResultNumber;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ScreenResultConsumer ReadNumberResultAsync: Exception " + ex.Message);
                throw;
            }
            return screenResultNumber.Value;
        }
    }
}
