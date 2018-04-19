using System;
using mmisharp;
using Microsoft.Speech.Recognition;
using System.Globalization;
using Microsoft.Speech.Recognition.SrgsGrammar;
using System.Xml;
using System.IO;

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
            lce = new LifeCycleEvents("ASR", "FUSION", "speech-1", "acoustic", "command"); // LifeCycleEvents(string source, string target, string id, string medium, string mode)
            //mmic = new MmiCommunication("localhost",9876,"User1", "ASR");  //PORT TO FUSION - uncomment this line to work with fusion later
            mmic = new MmiCommunication("localhost", 8000, "User1", "ASR"); // MmiCommunication(string IMhost, int portIM, string UserOD, string thisModalityName)

            mmic.Send(lce.NewContextRequest());

            //load pt recognizer
            
            sre = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("pt-PT"));
           
            Stream xmlStream =  ModifyXMLGrammar();

            gr = new Grammar(xmlStream, "rootRule");

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

            string json = "";

            if (e.Result.Confidence <= 0.30)
                return;

            json = "{ \"recognized\": {";
            foreach (var resultSemantic in e.Result.Semantics)
            {
                if (!resultSemantic.Value.Value.ToString().Equals(""))
                    json += "\"" + resultSemantic.Key + "\": \"" + resultSemantic.Value.Value + "\", ";
            }
            json = json.Substring(0, json.Length - 2);

            if (e.Result.Confidence > 0.30 && e.Result.Confidence <= 0.45)
            {
                json += ", \"confidence\":\"low confidence\" } }";
            }
            else if (e.Result.Confidence > 0.45 && e.Result.Confidence < 0.8)
            {
                json += ", \"confidence\":\"explicit confirmation\" } }";
            }
            else if (e.Result.Confidence >= 0.8)
            {
                json += ", \"confidence\":\"implicit confirmation\" } }";
            }
            Console.WriteLine(json);
            //Console.WriteLine("--------"+e.Result.Semantics["action"].Value+"-------");
            var exNot = lce.ExtensionNotification(e.Result.Audio.StartTime + "", e.Result.Audio.StartTime.Add(e.Result.Audio.Duration) + "", e.Result.Confidence, json);
            mmic.Send(exNot);
        }

        public void stopListening()
        {
            sre.RecognizeAsyncStop();
        }

        public void startListening()
        {
            sre.RecognizeAsync(RecognizeMode.Multiple);
        }

        private Stream ModifyXMLGrammar()
        {
            XmlDocument xmlGrammar = new XmlDocument();
            xmlGrammar.Load(Environment.CurrentDirectory + "\\ptG.grxml");
           
            //Display all the book titles.
            XmlNodeList elemList = xmlGrammar.GetElementsByTagName("rule");
            for (int i = 0; i < elemList.Count; i++)
            {
                XmlAttributeCollection attrs = elemList[i].Attributes;
                foreach (XmlAttribute at in attrs) {
                    if (at.Name == "id" && at.Value == "guildNameRule") {
                        XmlNode node =xmlGrammar.CreateNode("element", "item", "http://www.w3.org/2001/06/grammar");
                        node.InnerXml = "Tópicos de Apicultura <tag>out=\"Tópicos de Apicultura\";</tag>";
                       
                        elemList[i].ChildNodes[1].AppendChild(node);
                        
                    }
                }
               
            }
            MemoryStream xmlStream = new MemoryStream();
            xmlGrammar.Save(xmlStream);
            xmlStream.Flush();
            xmlStream.Position = 0;
            return xmlStream;

        }
    }

}
