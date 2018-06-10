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
            lce = new LifeCycleEvents("ASR", "FUSION", "speech-1", "acoustic", "command");
            mmic = new MmiCommunication("localhost", 9876, "User1", "ASR");
            //mmic = new MmiCommunication("localhost", 8000, "User1", "ASR"); // MmiCommunication(string IMhost, int portIM, string UserOD, string thisModalityName)

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
            onRecognized(new SpeechEventArg() { Text = e.Result.Text, Confidence = e.Result.Confidence, Final = true });
            /*foreach (var resultSemantic in e.Result.Semantics) 
                Console.WriteLine(resultSemantic.Key+":"+resultSemantic.Value.Value);*/

            string json = "{ \"recognized\": [";
            bool first = true;
            
            if (e.Result.Confidence <= 0.30)
                return;

            foreach (var resultSemantic in e.Result.Semantics)
            {
                if (!resultSemantic.Value.Value.ToString().Equals("")) {
                    json = AddJsonTag(json,resultSemantic.Key, resultSemantic.Value.Value.ToString(), first);
                    first = false;
                }
            }
            if (first)
                json = "{ ";


            if (e.Result.Confidence > 0.30 && e.Result.Confidence <= 0.45)
            {
                json = AddJsonTag(json, "confidence", "low confidence", false);
            }
            else if (e.Result.Confidence > 0.45 && e.Result.Confidence < 0.8)
            {
                json = AddJsonTag(json, "confidence", "explicit confirmation", false);
            }
            else if (e.Result.Confidence >= 0.8)
            {
                json = AddJsonTag(json, "confidence", "implicit confirmation", false);
            }
            json = AddJsonTag(json, "modality", "speech", false);
            json = json.Substring(0, json.Length - 2);
            json += "}";
            Console.WriteLine(json);
            //Console.WriteLine("--------"+e.Result.Semantics["action"].Value+"-------");
            var exNot = lce.ExtensionNotification(e.Result.Audio.StartTime + "", e.Result.Audio.StartTime.Add(e.Result.Audio.Duration) + "", e.Result.Confidence, json);
            mmic.Send(exNot);
        }

        private string AddJsonTag(string json, string resultKey, string resultValue, bool first) {
            switch (resultKey) {
                case "action":
                case "userName":
                case "channelName":
                case "guildName":
                case "reason":
                case "confirmation":
                    if (first)
                    {
                        json += "\"" +  resultKey + "\",\"" + resultValue + "\"], ";
                    }
                    else
                    {
                        json = json.Substring(0, json.Length - 3);
                        json += ", \"" + resultKey + "\",\"" + resultValue + "\"], ";
                    }
                    break;
                default:
                    json += "\"" + resultKey + "\"" + ":" + "\"" + resultValue + "\", ";
                    break;
            }
            return json;
        }

        public void stopListening()
        {
            sre.RecognizeAsyncStop();
        }

        public void startListening()
        {
            sre.RecognizeAsync(RecognizeMode.Multiple);
        }
    }

}
