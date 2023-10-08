using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Plugins;
using Key2Joy.Mapping;
using System;

namespace Key2Joy.Plugins
{
    public class PluginActionProxy : CoreAction, IGetRealObject<PluginAction>
    {
        private readonly PluginActionInsulator source;

        public PluginActionProxy(string name, PluginActionInsulator source)
            : base(name)
        {
            this.source = source;
        }

        public PluginAction GetRealObject()
        {
            return source.GetPluginAction;
        }

        public override MappingAspectOptions SaveOptions()
        {
            var options = base.SaveOptions();

            options = source.BuildSaveOptions(options);

            return options;
        }

        public override void LoadOptions(MappingAspectOptions options)
        {
            base.LoadOptions(options);

            source.LoadOptions(options);
        }

        public override string GetNameDisplay()
        {
            return source.GetNameDisplay(Name);
        }

        internal object InvokeScriptMethod(string methodName, object[] parameters)
        {
            try
            {
                return source.InvokeScriptMethod(methodName, parameters);
            }
            catch (Exception ex)
            {
                // TODO: Handle this better
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                throw ex;
            }
        }
    }
}
