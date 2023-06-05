using Esprima.Ast;
using Key2Joy.Contracts;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Plugins;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    public abstract class CoreAction : AbstractAction
    {
        public string ImageResource { get; set; }

        protected string description;

        public CoreAction(string name, string description)
        {
            Name = name;
            this.description = description;
        }

        public static AbstractAction MakeAction(MappingTypeFactory<AbstractAction> actionFactory)
        {
            //if (typeAttribute == null)
            //    typeAttribute = actionType.GetCustomAttributes(typeof(ActionAttribute), true)[0] as ActionAttribute;

            //return (CoreAction)Activator.CreateInstance(actionType, new object[]
            //    {
            //        typeAttribute.NameFormat ?? actionType.Name,
            //        typeAttribute.Description
            //    });

            var action = actionFactory.CreateInstance();
            var typeAttribute = actionFactory.Attribute;
            action.Name = typeAttribute.NameFormat ?? action.GetType().Name;
            // TODO: description? Can we refactor, because it makes stuff needlessly hard....
            
            return action;
        }

        public AbstractAction MakeStartedAction(MappingTypeFactory<AbstractAction> actionFactory)
        {
            var action = actionFactory.CreateInstance();
            action.SetStartData(listener, ref otherActions);
            return action;
        }

        public virtual string GetNameDisplay()
        {
            return Name;
        }

        public override string ToString()
        {
            return GetNameDisplay();
        }
    }
}
