
using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace QUT
{
    /// <summary>
    /// A helper class that provides a robust mechanism for retrieving and querying command line parameters provided to the application
    /// </summary>
    public class CommandLineArguments
    {
        /// <summary>
        /// Dictionary used to hold command line argument values
        /// </summary>
        StringDictionary parameters;

        public CommandLineArguments(string[] Args, bool printArgs = false)
        {
            parameters = new StringDictionary();
            Regex Spliter = new Regex(@"^-{1,2}|^/|=|:",
                RegexOptions.IgnoreCase);// | RegexOptions.Compiled);

            Regex Remover = new Regex(@"^['""]?(.*?)['""]?$",
                RegexOptions.IgnoreCase);// | RegexOptions.Compiled);

            string Parameter = null;
            string[] Parts;

            // Valid parameters:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: 
            // -param1 value1 --param2 /param3:"Test-:-work" 
            //   /param4=happy -param5 '--=nice=--'
            foreach (string Txt in Args)
            {
                // Look for new parameters (-,/ or --) and a
                // possible enclosed value (=,:)
                Parts = Spliter.Split(Txt, 3);

                switch (Parts.Length)
                {
                    // Found a value (for the last parameter 
                    // found (space separator))
                    case 1:
                        if (Parameter != null)
                        {
                            if (!parameters.ContainsKey(Parameter))
                            {
                                Parts[0] =
                                    Remover.Replace(Parts[0], "$1");

                                parameters.Add(Parameter, Parts[0]);
                            }
                            Parameter = null;
                        }
                        // else Error: no parameter waiting for a value (skipped)
                        break;

                    // Found just a parameter
                    case 2:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (Parameter != null)
                        {
                            if (!parameters.ContainsKey(Parameter))
                                parameters.Add(Parameter, "true");
                        }
                        Parameter = Parts[1];
                        break;

                    // Parameter with enclosed value
                    case 3:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (Parameter != null)
                        {
                            if (!parameters.ContainsKey(Parameter))
                                parameters.Add(Parameter, "true");
                        }

                        Parameter = Parts[1];

                        // Remove possible enclosing characters (",')
                        if (!parameters.ContainsKey(Parameter))
                        {
                            Parts[2] = Remover.Replace(Parts[2], "$1");
                            parameters.Add(Parameter, Parts[2]);
                        }

                        Parameter = null;
                        break;
                }
            }
            // In case a parameter is still waiting
            if (Parameter != null)
            {
                if (!parameters.ContainsKey(Parameter))
                    parameters.Add(Parameter, "true");
            }

            // Print out arguments if requested
            if (printArgs && parameters.Count > 0)
            {
                Console.WriteLine("Program arguments:");
                Console.WriteLine("--------------------------");
                foreach (String key in parameters.Keys)
                    Console.WriteLine(key + " = " + parameters[key]);
                Console.WriteLine("--------------------------\n");
            }
        }

        // Retrieve a parameter value if it exists 
        // (overriding C# indexer property)
        public string this[string Param]
        {
            get
            {
                return (parameters[Param]);
            }
        }

        
        public static T CommandLineArgument<T>(string param)
        {
            string arg = null;
            QUT.CommandLineArguments cmdArgs = new QUT.CommandLineArguments(Environment.GetCommandLineArgs(), true);

            try
            {
                if (cmdArgs != null)
                    arg = cmdArgs[param];

                if (arg != null)
                    return (T)System.Convert.ChangeType(arg, typeof(T));
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogWarningFormat("WARNING: Parsing arguments failed, using default values ({0})\n", e.Message);

                return (T)System.Convert.ChangeType("0", typeof(T));
            }
            return (T)System.Convert.ChangeType("0", typeof(T));
        }

        public static bool HasArgument(string param)
        {
            if (CommandLineArgument<string>(param) == "true") return true;

            string[] args = Environment.GetCommandLineArgs();
            
            for (int i = 0; i < args.Length; ++i)
            {
                if (args[i] != null && args.Length > 0)
                {
                    if (args[i] == param) return true;
                }
            }
            return false;
        }
    }
}