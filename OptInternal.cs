using System;
using System.Collections.Generic;

namespace OptParse
{
    internal class OptInternal
    {
        public char ShortName { get; set; }
        public string LongName { get; set; }
        public string Dest { get; set; }
        public string Help { get; set; }
        public OptionAction OptAction { get; set; }
        public Type OptType { get; set; }
        public string MetaVar { get; set; }
        public object DefaultValue { get; set; }

        internal bool IsSet { get; set; }

        internal OptInternal()
        {
            IsSet = false;
        }

        internal void Validate()
        {
            if (string.IsNullOrEmpty(Dest))
            {
                throw new OptParseException("Dest cannot be null.");
            }
            if (string.IsNullOrEmpty(Help))
            {
                throw new OptParseException("Help cannot be null.");
            }
        }

        internal KeyValuePair<string, object> Parse(string value = null)
        {
            var output = new KeyValuePair<string, object>(Dest, GetValue(value));
            IsSet = true;
            return output;
        }

        internal KeyValuePair<string, object> GetDefault()
        {
            return new KeyValuePair<string, object>(Dest, DefaultValue);
        }

        private object GetValue(object input = null)
        {
            switch (OptAction)
            {
                case OptionAction.Store:
                    return input ?? DefaultValue;
                case OptionAction.StoreTrue:
                    return true;
                case OptionAction.StoreFalse:
                    return false;
                default:
                    return input ?? DefaultValue;
            }
        }
    }
}
