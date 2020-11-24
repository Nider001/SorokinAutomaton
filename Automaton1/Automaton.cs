using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Automaton1
{
    class Pair<T1, T2>
    {
        public T1 Key { get; set; }
        public T2 Value { get; set; }

        public Pair(T1 key, T2 value)
        {
            Key = key;
            Value = value;
        }

        public override bool Equals(object obj)
        {
            return obj is Pair<T1, T2> pair &&
                   EqualityComparer<T1>.Default.Equals(Key, pair.Key) &&
                   EqualityComparer<T2>.Default.Equals(Value, pair.Value);
        }

        public override int GetHashCode()
        {
            var hashCode = 206514262;
            hashCode = hashCode * -1521134295 + EqualityComparer<T1>.Default.GetHashCode(Key);
            hashCode = hashCode * -1521134295 + EqualityComparer<T2>.Default.GetHashCode(Value);
            return hashCode;
        }
    }

    class AutomatonData
    {
        public bool UseRegex { get; set; }
        public KeyValuePair<Pair<string, string>, string>[] Transitions { get; set; }
        public HashSet<string> Start { get; set; }
        public HashSet<string> Finish { get; set; }

        public AutomatonData()
        {
            UseRegex = false;
            Transitions = new KeyValuePair<Pair<string, string>, string>[0];
            Start = new HashSet<string>();
            Finish = new HashSet<string>();
        }

        public AutomatonData(bool useRegex, Dictionary<Pair<string, string>, string> transitions, HashSet<string> start, HashSet<string> finish) : this(useRegex, transitions.ToArray(), start, finish)
        {

        }

        public AutomatonData(bool useRegex, KeyValuePair<Pair<string, string>, string>[] transitions, HashSet<string> start, HashSet<string> finish)
        {
            UseRegex = useRegex;
            Transitions = transitions ?? throw new ArgumentNullException(nameof(transitions));
            Start = start ?? throw new ArgumentNullException(nameof(start));
            Finish = finish ?? throw new ArgumentNullException(nameof(finish));
        }
    }

    class Automaton
    {
        bool useRegex;
        KeyValuePair<Pair<string, string>, string>[] transitions; //<<q1, '...'>, q2>
        HashSet<string> start;
        HashSet<string> finish;

        public Automaton(bool useRegex, KeyValuePair<Pair<string, string>, string>[] transitions, HashSet<string> start, HashSet<string> finish)
        {
            this.useRegex = useRegex;
            this.transitions = transitions ?? throw new ArgumentNullException(nameof(transitions));
            this.start = start ?? throw new ArgumentNullException(nameof(start));
            this.finish = finish ?? throw new ArgumentNullException(nameof(finish));
        }

        public Automaton(bool useRegex, Dictionary<Pair<string, string>, string> transitions, HashSet<string> start, HashSet<string> finish) : this(useRegex, transitions.ToArray(), start, finish)
        {

        }

        public Automaton(AutomatonData data) : this(data.UseRegex, data.Transitions, data.Start, data.Finish)
        {

        }

        private void MoveState(string symbol, ref HashSet<string> currentStates, ref Pair<bool, int> result, ref bool isInFinal)
        {
            try
            {
                isInFinal = false;

                bool countIncreasedThisTime = false;

                var tempStates = currentStates;
                currentStates = new HashSet<string>();

                bool passed = false;

                foreach (string cs in tempStates)
                {
                    var potentialTransitions = useRegex ? transitions.Where(x => x.Key.Key == cs && Regex.IsMatch(symbol, x.Key.Value)).ToArray() : transitions.Where(x => x.Key.Key == cs && symbol == x.Key.Value).ToArray();
                    if (!passed && potentialTransitions.Length != 0)
                    {
                        passed = true;
                    }

                    foreach (var temp in potentialTransitions)
                    {
                        if (finish.Contains(temp.Value))
                        {
                            isInFinal = true;
                        }

                        if (!currentStates.Contains(temp.Value))
                        {
                            currentStates.Add(temp.Value);

                            if (!countIncreasedThisTime && result.Key == true)
                            {
                                countIncreasedThisTime = true;
                                result.Value++;
                            }
                        }
                    }
                }

                if (!passed)
                {
                    throw new Exception();
                }
            }
            catch
            {
                result.Key = false;
            }
        }

        public Pair<bool, int> Max(string data, int skip)
        {
            if (skip < 0 || skip >= data.Length)
            {
                throw new ArgumentException(nameof(skip));
            }

            HashSet<string> currentStates = new HashSet<string>(start);
            Pair<bool, int> result = new Pair<bool, int>(true, 0);

            Pair<bool, int> frozenResult = new Pair<bool, int>(true, 0);

            char[] array = data.ToCharArray().Skip(skip).ToArray();

            bool arrivedToFinal = currentStates.Any(x => finish.Contains(x)) ? true : false;
            bool isInFinal = arrivedToFinal;

            foreach (char item in array)
            {
                MoveState(item.ToString(), ref currentStates, ref result, ref isInFinal);
                arrivedToFinal = arrivedToFinal || isInFinal;

                if (!result.Key)
                {
                    break;
                }
                else if (isInFinal)
                {
                    frozenResult.Key = result.Key;
                    frozenResult.Value = result.Value;
                }
            }

            if (!arrivedToFinal)
            {
                frozenResult.Key = false;
                frozenResult.Value = 0;
            }
            else if (!isInFinal)
            {
                frozenResult.Key = false;
            }

            return frozenResult;
        }
    }
}
