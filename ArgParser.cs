using System.Collections.Generic;
using System.Linq;
using OptParse.Extensions;

namespace OptParse
{
    internal class ArgParser<T> where T : class
    {
        private readonly List<OptInternal> _options;
        private readonly string[] _args;
        private readonly List<int> _ordinalsUsed;
        private readonly OptConfig<T> _config;

        internal ArgParser(List<OptInternal> options, string[] args)
        {
            _options = options;
            _args = args;
            _ordinalsUsed = new List<int>();
            _config = new OptConfig<T>();
        }

        internal OptConfig<T> Parse()
        {
            Validate();
            for (int i = 0; i < _args.Length; i++ )
            {
                if (_ordinalsUsed.Contains(i))
                {
                    continue;
                }
                string arg = GetArg(i);
                if (arg.StartsWith("--"))
                {
                    ParseLongName(i);
                }
                else if (arg.StartsWith("-"))
                {
                    ParseShortName(i);
                }
                else
                {
                    _config.Args.Add(_args[i]);
                }
            }
            foreach (var opt in _options.Where(o => !o.IsSet))
            {
                var output = opt.GetDefault();
                AddOption(output);
            }
            _config.SetStatic();
            return _config;
        }

        private void Validate()
        {
            foreach (string arg in _args)
            {
                if (arg.Count(c => c == '=') > 1)
                {
                    throw new OptParseException("You may not have more than one '=' in an argument.");
                }
            }
        }

        private string GetArg(int ordinal)
        {
            return ordinal < _args.Length ? _args[ordinal] : null;
        }

        private string GetNextArg(int ordinal)
        {
            return GetArg(ordinal + 1);
        }

        private void ParseLongName(int ordinal)
        {
            string arg = GetArg(ordinal);
            string argStripped = arg.Remove(0, 2);
            string[] args = argStripped.Split('=');
            OptInternal opt = _options.FirstOrDefault(o => o.LongName == args[0]);
            if (opt == null)
            {
                throw FormatException("The application does not support argument with long name {0}.", arg);
            }
            var output = args.Length == 2 ? opt.Parse(args[1]) : opt.Parse();
            AddOption(output);
            MarkOrdinalUsed(ordinal);
        }

        private void ParseShortName(int ordinal)
        {
            string arg = GetArg(ordinal);
            string nextArg = GetNextArg(ordinal);
            string argStripped = arg.Remove(0, 1);
            char[] args = argStripped.ToCharArray();
            foreach (var a in args)
            {
                OptInternal opt = _options.FirstOrDefault(o => o.ShortName == a);
                if (opt == null)
                {
                    throw FormatException("The application does not support argument with short name -{0}.", a);
                }
                var output = nextArg.Exists() && !nextArg.IsArgument() && opt.OptAction == OptionAction.Store ? opt.Parse(nextArg) : opt.Parse();
                AddOption(output);
                MarkOrdinalUsed(ordinal);
                if(opt.OptAction == OptionAction.Store)
                    MarkOrdinalUsed(ordinal + 1);
            }
        }

        private void AddOption(KeyValuePair<string, object> kvp)
        {
            var options = _config.OptionsDynamic as IDictionary<string, object>;
            options.Add(kvp);
        }

        private void MarkOrdinalUsed(int ordinal)
        {
            if(!_ordinalsUsed.Contains(ordinal))
                _ordinalsUsed.Add(ordinal);
        }

        private OptParseException FormatException(string format, params object[] args)
        {
            var msg = string.Format(format, args);
            return new OptParseException(msg);
        }
    }
}
