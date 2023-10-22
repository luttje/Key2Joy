using System.Collections.Generic;
using System.Text;
using Key2Joy.Contracts;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Contracts.Util;

namespace Key2Joy.Mapping.Actions.Scripting;

public abstract class BaseScriptAction : CoreAction
{
    public static readonly object LockObject = new();

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

    /// <summary>
    /// Called to register scripting methods on the environment.
    /// </summary>
    /// <param name="exposedMethod">The method to be exposed to scripting</param>
    /// <param name="scriptActionInstance">The action on which the exposed method resides</param>
    public abstract void RegisterScriptingMethod(ExposedMethod exposedMethod, AbstractAction scriptActionInstance);

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
        var truncatedScript = this.Script;

        return this.Name.Replace("{0}", truncatedScript);
    }

    public override void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
        => base.OnStartListening(listener, ref otherActions);
}
