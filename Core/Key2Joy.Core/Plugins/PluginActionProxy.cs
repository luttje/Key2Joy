﻿using System;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Plugins;
using Key2Joy.Mapping.Actions;

namespace Key2Joy.Plugins;

public class PluginActionProxy : CoreAction, IGetRealObject<PluginAction>
{
    private readonly PluginActionInsulator source;

    public PluginActionProxy(string name, PluginActionInsulator source)
        : base(name) => this.source = source;

    public PluginAction GetRealObject() => this.source.PluginAction;

    public override MappingAspectOptions SaveOptions()
    {
        var options = base.SaveOptions();

        options = this.source.BuildSaveOptions(options);

        return options;
    }

    public override void LoadOptions(MappingAspectOptions options)
    {
        base.LoadOptions(options);

        this.source.LoadOptions(options);
    }

    public override string GetNameDisplay() => this.source.GetNameDisplay(this.Name);

    internal object InvokeScriptMethod(string methodName, object[] parameters)
    {
        try
        {
            return this.source.InvokeScriptMethod(methodName, parameters);
        }
        catch (Exception ex)
        {
            // TODO: Handle this better
            System.Windows.Forms.MessageBox.Show(ex.ToString());
            throw ex;
        }
    }
}
