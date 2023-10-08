﻿using Esprima.Ast;
using Key2Joy.Contracts;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Plugins;
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

        public CoreAction(string name)
            : base(name)
        {
        }

        public static AbstractAction MakeAction(MappingTypeFactory<AbstractAction> actionFactory)
        {
            var typeAttribute = actionFactory.Attribute;
            var action = actionFactory.CreateInstance(new object[]
            {
                typeAttribute.NameFormat ?? actionFactory.FullTypeName,
            });
            
            return action;
        }

        public AbstractAction MakeStartedAction(MappingTypeFactory<AbstractAction> actionFactory)
        {
            var typeAttribute = actionFactory.Attribute;
            var action = actionFactory.CreateInstance(new object[]
            {
                typeAttribute.NameFormat ?? actionFactory.FullTypeName,
            });
            action.SetStartData(listener, ref otherActions);
            return action;
        }

        public override string ToString()
        {
            return GetNameDisplay();
        }
    }
}