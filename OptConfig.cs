using System;
using System.Collections.Generic;
using System.Dynamic;

namespace OptParse
{
    public class OptConfig<T> where T : class
    {
        internal dynamic OptionsDynamic { get; set; }
        public T Options { get; set; }
        public IList<string> Args { get; set; }

        public OptConfig()
        {
            OptionsDynamic = new ExpandoObject();
            Args = new List<string>();
        }


        internal void SetStatic()
        {
            var optInst = Activator.CreateInstance<T>();
            var opts = OptionsDynamic as IDictionary<string, object>;

            if (opts == null)
            {
                Options = null;
                return;
            }

            foreach (var opt in opts)
            {
                var propertyInfo = optInst.GetType().GetProperty(opt.Key);
                if (propertyInfo != null)
                    propertyInfo.SetValue(optInst, opt.Value, null);
            }
            Options = optInst;
        }
    }
}
