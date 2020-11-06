using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace Automaton1
{
    class Program
    {
        static Pair<bool, int> Execute(Automaton automaton, string str, int skip)
        {
            return automaton.Max(str, skip);
        }

        static void Main(string[] args)
        {
            Automaton special = null;
            Automaton keyWord = null;
            Automaton constBool = null;
            Automaton constDouble = null;
            Automaton constInt = null;
            Automaton id = null;
            Automaton assign = null;
            Automaton oper = null;
            Automaton bracket = null;

            using (StreamReader sr = new StreamReader("special.txt"))
            {
                string t = sr.ReadToEnd();
                special = new Automaton(JsonConvert.DeserializeObject<AutomatonData>(t));
            }

            using (StreamReader sr = new StreamReader("keyword.txt"))
            {
                string t = sr.ReadToEnd();
                keyWord = new Automaton(JsonConvert.DeserializeObject<AutomatonData>(t));
            }

            using (StreamReader sr = new StreamReader("const_bool.txt"))
            {
                string t = sr.ReadToEnd();
                constBool = new Automaton(JsonConvert.DeserializeObject<AutomatonData>(t));
            }

            using (StreamReader sr = new StreamReader("const_double.txt"))
            {
                string t = sr.ReadToEnd();
                constDouble = new Automaton(JsonConvert.DeserializeObject<AutomatonData>(t));
            }

            using (StreamReader sr = new StreamReader("const_int.txt"))
            {
                string t = sr.ReadToEnd();
                constInt = new Automaton(JsonConvert.DeserializeObject<AutomatonData>(t));
            }

            using (StreamReader sr = new StreamReader("id.txt"))
            {
                string t = sr.ReadToEnd();
                id = new Automaton(JsonConvert.DeserializeObject<AutomatonData>(t));
            }

            using (StreamReader sr = new StreamReader("assign.txt"))
            {
                string t = sr.ReadToEnd();
                assign = new Automaton(JsonConvert.DeserializeObject<AutomatonData>(t));
            }

            using (StreamReader sr = new StreamReader("oper.txt"))
            {
                string t = sr.ReadToEnd();
                oper = new Automaton(JsonConvert.DeserializeObject<AutomatonData>(t));
            }

            using (StreamReader sr = new StreamReader("bracket.txt"))
            {
                string t = sr.ReadToEnd();
                bracket = new Automaton(JsonConvert.DeserializeObject<AutomatonData>(t));
            }


            //=================================================================================

            string code = "";
            Pair<bool, int> state = new Pair<bool, int>(false, 0);
            int iter = 0;

            List<Pair<string, string>> tokens = new List<Pair<string, string>>();

            using (StreamReader sr = new StreamReader("CODE.txt"))
            {
                code = Regex.Replace(sr.ReadToEnd(), @"\s", "");
            }

            Dictionary<Automaton, string> automatons = new Dictionary<Automaton, string>();
            automatons.Add(special, "special");
            automatons.Add(keyWord, "keyWord");
            automatons.Add(constBool, "constBool");
            automatons.Add(constDouble, "constDouble");
            automatons.Add(constInt, "constInt");
            automatons.Add(id, "id");
            automatons.Add(assign, "assign");
            automatons.Add(oper, "oper");
            automatons.Add(bracket, "bracket");

            while (iter < code.Length)
            {

                foreach (var automaton in automatons)
                {
                    state = Execute(automaton.Key, code, iter);

                    if (state.Value != 0)
                    {
                        tokens.Add(new Pair<string, string>(automaton.Value, code.Substring(iter, state.Value)));
                        iter += state.Value;
                        break;
                    }
                    else if (automaton.Value == automatons.Values.Last())
                    {
                        throw new ArgumentException("Error: invalid code at '" + code[iter] + "...'");
                    }
                }

                string json = JsonConvert.SerializeObject(tokens, Formatting.Indented);

                using (StreamWriter sr = new StreamWriter("RESULT.txt"))
                {
                    sr.Write(json);
                }
            }
        }
    }
}
