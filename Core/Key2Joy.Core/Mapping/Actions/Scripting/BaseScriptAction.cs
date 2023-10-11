using System.Collections.Generic;
using System.Text;
using Key2Joy.Contracts;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Plugins;

namespace Key2Joy.Mapping.Actions.Scripting;

public abstract class BaseScriptAction : CoreAction
{
    public static readonly object LockObject = new();

    /// <summary>
    /// TODO: Clean this up. This is just a quick hack to get enumerations into scripts. Should move to something the plugins can affect nicely.
    /// </summary>
    public static List<ExposedEnumeration> ExposedEnumerations = new();

    public string Script { get; set; }
    public bool IsScriptPath { get; set; }

    protected string cachedFile;

    public BaseScriptAction(string name)
        : base(name)
    { }

    protected virtual string GetExecutableScript()
    {
        if (this.IsScriptPath)
        {
            this.cachedFile ??= System.IO.File.ReadAllText(this.Script);

            return this.cachedFile;
        }

        return this.Script;
    }

    public abstract void RegisterScriptingEnum(ExposedEnumeration enumeration);

    public abstract void RegisterScriptingMethod(ExposedMethod exposedMethod, AbstractAction instance);

    public virtual void Print(params object[] args)
    {
        StringBuilder sb = new();
        for (var i = 0; i < args.Length; i++)
        {
            if (i > 0)
            {
                sb.Append("\t");
            }

            sb.Append(args[i]);
        }

        Output.WriteLine(sb);
    }

    public override string GetNameDisplay()
    {
        // Truncate the script to be no more than 50 characters
        var truncatedScript = this.Script.Length > 47 ? this.Script.Substring(0, 47) + "..." : this.Script;

        return this.Name.Replace("{0}", truncatedScript);
    }

    public override void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
        => base.OnStartListening(listener, ref otherActions);
}
