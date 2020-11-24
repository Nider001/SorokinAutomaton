using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automaton1
{
    static class ProgramTests
    {
        static private void Stub(string automatonPath, string input, int skip, Pair<bool, int> expectedResult, string logFilePath)
        {
            Automaton automaton = null;

            using (StreamReader sr = new StreamReader(automatonPath))
            {
                string t = sr.ReadToEnd();
                automaton = new Automaton(JsonConvert.DeserializeObject<AutomatonData>(t));
            }

            Pair<bool, int> result = automaton.Max(input, skip);

            using (StreamWriter sw = new StreamWriter(logFilePath, true))
            {
                sw.WriteLine();

                sw.WriteLine("TEST RESULT FOR " + automatonPath);
                sw.WriteLine("  INPUT: '" + input + "'");
                sw.WriteLine("  SKIP: " + skip + "");
                sw.WriteLine("  EXPECTED RESULT: <" + expectedResult.Key + ";" + expectedResult.Value + ">");
                sw.WriteLine("  ACTUAL RESULT: <" + result.Key + ";" + result.Value + ">");

                if (result.Equals(expectedResult))
                {
                    sw.WriteLine("CORRECT!");
                }
                else
                {
                    sw.WriteLine("INCORRECT!");
                }
            }
        }

        static public void TestSpecial()
        {
            using (StreamWriter sw = new StreamWriter("TESTLOG_SPECIAL.txt")) { }

            Stub("special.txt", ";", 0, new Pair<bool, int>(true, 1), "TESTLOG_SPECIAL.txt");
            Stub("special.txt", ";aaa", 0, new Pair<bool, int>(false, 1), "TESTLOG_SPECIAL.txt");
            Stub("special.txt", "{", 0, new Pair<bool, int>(true, 1), "TESTLOG_SPECIAL.txt");
            Stub("special.txt", "}aaa", 0, new Pair<bool, int>(false, 1), "TESTLOG_SPECIAL.txt");
        }

        static public void TestKeyWord()
        {
            using (StreamWriter sw = new StreamWriter("TESTLOG_KEYWORD.txt")) { }

            Stub("keyword.txt", "if", 0, new Pair<bool, int>(true, 2), "TESTLOG_KEYWORD.txt");
            Stub("keyword.txt", "while", 0, new Pair<bool, int>(true, 5), "TESTLOG_KEYWORD.txt");
            Stub("keyword.txt", "for", 0, new Pair<bool, int>(true, 3), "TESTLOG_KEYWORD.txt");
            Stub("keyword.txt", "bool", 0, new Pair<bool, int>(true, 4), "TESTLOG_KEYWORD.txt");
            Stub("keyword.txt", "else", 0, new Pair<bool, int>(true, 4), "TESTLOG_KEYWORD.txt");
            Stub("keyword.txt", "int", 0, new Pair<bool, int>(true, 3), "TESTLOG_KEYWORD.txt");
            Stub("keyword.txt", "double", 0, new Pair<bool, int>(true, 6), "TESTLOG_KEYWORD.txt");
            Stub("keyword.txt", "var", 0, new Pair<bool, int>(true, 3), "TESTLOG_KEYWORD.txt");
            Stub("keyword.txt", "ifs", 0, new Pair<bool, int>(false, 2), "TESTLOG_KEYWORD.txt");
            Stub("keyword.txt", "whil", 0, new Pair<bool, int>(false, 0), "TESTLOG_KEYWORD.txt");
            Stub("keyword.txt", "integer", 0, new Pair<bool, int>(false, 3), "TESTLOG_KEYWORD.txt");
        }

        static public void TestConstBool()
        {
            using (StreamWriter sw = new StreamWriter("TESTLOG_CONSTBOOL.txt")) { }

            Stub("const_bool.txt", "true", 0, new Pair<bool, int>(true, 4), "TESTLOG_CONSTBOOL.txt");
            Stub("const_bool.txt", "falsee", 0, new Pair<bool, int>(false, 5), "TESTLOG_CONSTBOOL.txt");
            Stub("const_bool.txt", "tru", 0, new Pair<bool, int>(false, 0), "TESTLOG_CONSTBOOL.txt");
            Stub("const_bool.txt", "fals", 0, new Pair<bool, int>(false, 0), "TESTLOG_CONSTBOOL.txt");
        }

        static public void TestConstDouble()
        {
            using (StreamWriter sw = new StreamWriter("TESTLOG_CONSTDOUBLE.txt")) { }

            Stub("const_double.txt", "0.0", 0, new Pair<bool, int>(true, 3), "TESTLOG_CONSTDOUBLE.txt");
            Stub("const_double.txt", "12.5", 0, new Pair<bool, int>(true, 4), "TESTLOG_CONSTDOUBLE.txt");
            Stub("const_double.txt", "11.5E2", 0, new Pair<bool, int>(true, 6), "TESTLOG_CONSTDOUBLE.txt");
            Stub("const_double.txt", "11.5e-2", 0, new Pair<bool, int>(true, 7), "TESTLOG_CONSTDOUBLE.txt");
            Stub("const_double.txt", "55.", 0, new Pair<bool, int>(false, 0), "TESTLOG_CONSTDOUBLE.txt");
            Stub("const_double.txt", ".39", 0, new Pair<bool, int>(false, 0), "TESTLOG_CONSTDOUBLE.txt");
            Stub("const_double.txt", "7.e+4", 0, new Pair<bool, int>(false, 0), "TESTLOG_CONSTDOUBLE.txt");
            Stub("const_double.txt", "10.2e+", 0, new Pair<bool, int>(false, 4), "TESTLOG_CONSTDOUBLE.txt");
        }

        static public void TestConstInt()
        {
            using (StreamWriter sw = new StreamWriter("TESTLOG_CONSTINT.txt")) { }

            Stub("const_int.txt", "0", 0, new Pair<bool, int>(true, 1), "TESTLOG_CONSTINT.txt");
            Stub("const_int.txt", "123", 0, new Pair<bool, int>(true, 3), "TESTLOG_CONSTINT.txt");
            Stub("const_int.txt", "123.5", 0, new Pair<bool, int>(false, 3), "TESTLOG_CONSTINT.txt");
            Stub("const_int.txt", "12aa", 0, new Pair<bool, int>(false, 2), "TESTLOG_CONSTINT.txt");
        }

        static public void TestId()
        {
            using (StreamWriter sw = new StreamWriter("TESTLOG_ID.txt")) { }

            Stub("id.txt", "_admin", 0, new Pair<bool, int>(true, 6), "TESTLOG_ID.txt");
            Stub("id.txt", "art_123", 0, new Pair<bool, int>(true, 7), "TESTLOG_ID.txt");
            Stub("id.txt", "0number", 0, new Pair<bool, int>(false, 0), "TESTLOG_ID.txt");
            Stub("id.txt", "admin+1", 0, new Pair<bool, int>(false, 5), "TESTLOG_ID.txt");
        }

        static public void TestAssign()
        {
            using (StreamWriter sw = new StreamWriter("TESTLOG_ASSIGN.txt")) { }

            Stub("assign.txt", "=", 0, new Pair<bool, int>(true, 1), "TESTLOG_ASSIGN.txt");
            Stub("assign.txt", "+=", 0, new Pair<bool, int>(true, 2), "TESTLOG_ASSIGN.txt");
            Stub("assign.txt", "-=", 0, new Pair<bool, int>(true, 2), "TESTLOG_ASSIGN.txt");
            Stub("assign.txt", "*=", 0, new Pair<bool, int>(true, 2), "TESTLOG_ASSIGN.txt");
            Stub("assign.txt", "/=", 0, new Pair<bool, int>(true, 2), "TESTLOG_ASSIGN.txt");
            Stub("assign.txt", "%=", 0, new Pair<bool, int>(true, 2), "TESTLOG_ASSIGN.txt");
            Stub("assign.txt", "=*", 0, new Pair<bool, int>(false, 1), "TESTLOG_ASSIGN.txt");
            Stub("assign.txt", "==", 0, new Pair<bool, int>(false, 1), "TESTLOG_ASSIGN.txt");
            Stub("assign.txt", "/=/", 0, new Pair<bool, int>(false, 2), "TESTLOG_ASSIGN.txt");
        }

        static public void TestOperator()
        {
            using (StreamWriter sw = new StreamWriter("TESTLOG_OPERATOR.txt")) { }

            Stub("oper.txt", "!", 0, new Pair<bool, int>(true, 1), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", "+", 0, new Pair<bool, int>(true, 1), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", "-", 0, new Pair<bool, int>(true, 1), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", "*", 0, new Pair<bool, int>(true, 1), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", "/", 0, new Pair<bool, int>(true, 1), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", "%", 0, new Pair<bool, int>(true, 1), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", ">", 0, new Pair<bool, int>(true, 1), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", "<", 0, new Pair<bool, int>(true, 1), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", "==", 0, new Pair<bool, int>(true, 2), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", "<=", 0, new Pair<bool, int>(true, 2), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", ">=", 0, new Pair<bool, int>(true, 2), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", "++", 0, new Pair<bool, int>(true, 2), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", "--", 0, new Pair<bool, int>(true, 2), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", "||", 0, new Pair<bool, int>(true, 2), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", "&&", 0, new Pair<bool, int>(true, 2), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", "-=", 0, new Pair<bool, int>(false, 1), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", "<>", 0, new Pair<bool, int>(false, 1), "TESTLOG_OPERATOR.txt");
            Stub("oper.txt", "&|", 0, new Pair<bool, int>(false, 0), "TESTLOG_OPERATOR.txt");
        }

        static public void TestBracket()
        {
            using (StreamWriter sw = new StreamWriter("TESTLOG_BRACKET.txt")) { }

            Stub("bracket.txt", "(", 0, new Pair<bool, int>(true, 1), "TESTLOG_BRACKET.txt");
            Stub("bracket.txt", ")", 0, new Pair<bool, int>(true, 1), "TESTLOG_BRACKET.txt");
            Stub("bracket.txt", "()", 0, new Pair<bool, int>(false, 1), "TESTLOG_BRACKET.txt");
            Stub("bracket.txt", "(1", 0, new Pair<bool, int>(false, 1), "TESTLOG_BRACKET.txt");
        }
    }
}
