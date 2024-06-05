using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Bubla
{
    public class RobotSpeak
    {
        public void Speak(string text, int volume = 30, int rate = 1)
        {
            new Thread(() => {
                SpeechSynthesizer talker = new SpeechSynthesizer();
                talker.Volume = volume;
                talker.Rate = rate;
                talker.Speak(text);
            }).Start();
        }
    }
}
