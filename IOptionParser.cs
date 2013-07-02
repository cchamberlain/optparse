using System;

namespace OptParse
{
    public interface IOptionParser<T> where T : class
    {
        void AddOption(string shortName, string longName, string dest, string help,
                       OptionAction action = OptionAction.Store, Type type = null, string metaVar = null,
                       object defaultValue = null);
        OptConfig<T> ParseArgs(string[] args);
    }
}