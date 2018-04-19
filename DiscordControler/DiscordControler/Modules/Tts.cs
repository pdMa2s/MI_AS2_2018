using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Synthesis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace DiscordControler.Modules
{
    class Tts
    {
        SpeechSynthesizer tts = null;
        private Coms _comModule;

        /*
         * Text to Speech
         */
        public Tts(Coms comModule)
        {

            this._comModule = comModule;
            Console.WriteLine("TTS constructor called");

            //create speech synthesizer
            tts = new SpeechSynthesizer();

            // show voices 
            // Initialize a new instance of the SpeechSynthesizer.
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                // Output information about all of the installed voices. 
                Console.WriteLine("Installed voices -");
                foreach (InstalledVoice voice in synth.GetInstalledVoices())
                {
                    VoiceInfo info = voice.VoiceInfo;
                    string AudioFormats = "";
                    foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                    {
                        AudioFormats += String.Format("{0}\n",
                        fmt.EncodingFormat.ToString());
                    }

                    Console.WriteLine(" Name:          " + info.Name);
                    Console.WriteLine(" Culture:       " + info.Culture);
                    Console.WriteLine(" Age:           " + info.Age);
                    Console.WriteLine(" Gender:        " + info.Gender);
                    Console.WriteLine(" Description:   " + info.Description);
                    Console.WriteLine(" ID:            " + info.Id);
                    Console.WriteLine(" Enabled:       " + voice.Enabled);
                    if (info.SupportedAudioFormats.Count != 0)
                    {
                        Console.WriteLine(" Audio formats: " + AudioFormats);
                    }
                    else
                    {
                        Console.WriteLine(" No supported audio formats found");
                    }

                    string AdditionalInfo = "";
                    foreach (string key in info.AdditionalInfo.Keys)
                    {
                        AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
                    }

                    Console.WriteLine(" Additional Info - " + AdditionalInfo);
                    Console.WriteLine();
                }
            }
            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();

            //set voice
            //tts.SelectVoiceByHints(VoiceGender.Male, VoiceAge.NotSet, 0, new System.Globalization.CultureInfo("pt-PT"));
            tts.SelectVoice("Microsoft Server Speech Text to Speech Voice (pt-PT, Nuno PTTS)");

            tts.SetOutputToDefaultAudioDevice();
            //set function to play audio after synthesis is complete
            tts.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(tts_SpeakCompleted);
        }

        /*
         * Speak
         * 
         * @param text - text to convert
         */
        public void Speak(string text)
        {
            _comModule.SendCommandToSpeech("ttsSpeaking");
            tts.SpeakAsync(text);
        }

        public void Speak(string text, int rate)
        {

            Console.WriteLine("Speak method called , version with samplerate parameter");

            tts.Rate = rate;
            // tts.SpeakAsync(text);

            Console.WriteLine("... calling  SpeakSsmlAsync()");

            tts.SpeakSsmlAsync(text);

            Console.WriteLine("done  SpeakSsmlAsync().\n");

        }

        /*
         * tts_SpeakCompleted
         */
        void tts_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            _comModule.SendCommandToSpeech("ttsNotSpeaking");
        }
    }   
}
