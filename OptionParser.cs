using System;
using System.Collections.Generic;

namespace OptParse
{
    public class OptionParser<T> : IOptionParser<T> where T : class
    {
        private readonly List<OptInternal> _options;

        public OptionParser()
        {
            _options = new List<OptInternal>();
        }

        public void AddOption(string shortName, string longName, string dest, string help, OptionAction action=OptionAction.Store, Type type = null, string metaVar = null, object defaultValue = null)
        {
            var opt = new OptInternal
                {
                    ShortName = GetShortName(shortName),
                    LongName = GetLongName(longName),
                    Dest = dest,
                    Help = help,
                    OptAction = action,
                    OptType = type,
                    MetaVar = metaVar,
                    DefaultValue = defaultValue
                };
            opt.Validate();
            _options.Add(opt);
        }

        public OptConfig<T> ParseArgs(string[] args)
        {
            var parser = new ArgParser<T>(_options, args);
            return parser.Parse();
        }

        private char GetShortName(string raw)
        {
            if (raw.StartsWith("-") && raw.Length == 2)
            {
                return raw[1];
            }
            throw new ArgumentException("Short name was not in valid format.", "raw");
        }

        private string GetLongName(string raw)
        {
            if (raw.StartsWith("--") && raw.Length > 2)
            {
                return raw.Substring(2);
            }
            throw new ArgumentException("Long name was not in valid format.", "raw");
        }
    }
}
