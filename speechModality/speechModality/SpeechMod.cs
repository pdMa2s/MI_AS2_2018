using System;
using mmisharp;
using Microsoft.Speech.Recognition;

namespace speechModality
{
    public class SpeechMod
    {
        private SpeechRecognitionEngine sre;
        private Grammar gr;
        public event EventHandler<SpeechEventArg> Recognized;
        protected virtual void onRecognized(SpeechEventArg msg)
        {
            EventHandler<SpeechEventArg> handler = Recognized;
            if (handler != null)
            {
                handler(this, msg);
            }
        }

        private LifeCycleEvents lce;
        private MmiCommunication mmic;

        public SpeechMod()
        {
            //init LifeCycleEvents..
            lce = new LifeCycleEvents("ASR", "FUSION","speech-1", "acoustic", "command"); // LifeCycleEvents(string source, string target, string id, string medium, string mode)
            //mmic = new MmiCommunication("localhost",9876,"User1", "ASR");  //PORT TO FUSION - uncomment this line to work with fusion later
            mmic = new MmiCommunication("localhost", 8000, "User1", "ASR"); // MmiCommunication(string IMhost, int portIM, string UserOD, string thisModalityName)

            mmic.Send(lce.NewContextRequest());

            //load pt recognizer
            sre = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("pt-PT"));
            gr = new Grammar(Environment.CurrentDirectory + "\\ptG.grxml", "rootRule");
            sre.LoadGrammar(gr);

            
            sre.SetInputToDefaultAudioDevice();
            sre.RecognizeAsync(RecognizeMode.Multiple);
            sre.SpeechRecognized += Sre_SpeechRecognized;
            sre.SpeechHypothesized += Sre_SpeechHypothesized;

        }

        private void Sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            onRecognized(new SpeechEventArg() { Text = e.Result.Text, Confidence = e.Result.Confidence, Final = false });
        }

        private void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            onRecognized(new SpeechEventArg(){Text = e.Result.Text, Confidence = e.Result.Confidence, Final = true});
            /*foreach (var resultSemantic in e.Result.Semantics) 
                Console.WriteLine(resultSemantic.Key+":"+resultSemantic.Value.Value);*/

            //SEND
            // IMPORTANT TO KEEP THE FORMAT {"recognized":["SHAPE","COLOR"]}

            string json = "";

            if (e.Result.Confidence < 0.30)
                return;

            json = "{ \"recognized\": {";
            foreach (var resultSemantic in e.Result.Semantics)
            {
                if (!resultSemantic.Value.Value.ToString().Equals(""))
                    json += "\"" + resultSemantic.Key + "\": \"" + resultSemantic.Value.Value + "\", ";
            }
            json = json.Substring(0, json.Length - 2);

            if (e.Result.Confidence > 0.31 && e.Result.Confidence < 0.45)
            {
                json += ", \"confidence\":\"low confidence\" } }";
            }
            else if (e.Result.Confidence > 0.46 && e.Result.Confidence < 0.79)
            {
                json += ", \"confidence\":\"explicit confimation\"} }";
            }
            else if (e.Result.Confidence > 0.8)
            {
                json += ", \"confidence\":\"implicit confimation\" } }";
            }
            Console.WriteLine(json);
            //Console.WriteLine("--------"+e.Result.Semantics["action"].Value+"-------");
            var exNot = lce.ExtensionNotification(e.Result.Audio.StartTime+"", e.Result.Audio.StartTime.Add(e.Result.Audio.Duration)+"",e.Result.Confidence, json);
            mmic.Send(exNot);
        }
    }
}
