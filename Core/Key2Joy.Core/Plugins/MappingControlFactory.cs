﻿using Key2Joy.Contracts.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Plugins
{
    /// <summary>
    /// Creates instances of the Control, simply using Activator.CreateInstance
    /// </summary>
    public abstract class MappingControlFactory
    {
        public string ForTypeFullName { get; private set; }
        public string ImageResourceName { get; private set; }

        public MappingControlFactory(string forTypeFullName, string imageResourceName)
        {
            ForTypeFullName = forTypeFullName;
            ImageResourceName = imageResourceName;
        }

        public virtual T CreateInstance<T>() where T : Control
        {
            return (T)Activator.CreateInstance(ToType());
        }

        public abstract Type ToType();
        public virtual string GetTypeName()
        {
            return ToType().FullName;
        }
    }

    /// <summary>
    /// Creates instances of the Control, simply using Activator.CreateInstance
    /// </summary>
    public class TypeMappingControlFactory : MappingControlFactory
    {
        private Type controlType;

        public TypeMappingControlFactory(string forTypeFullName, string imageResourceName, Type controlType)
            : base(forTypeFullName, imageResourceName)
        {
            this.controlType = controlType;
        }

        public override Type ToType()
        {
            return controlType;
        }
    }

    /// <summary>
    /// Creates the Control by commanding the PluginHostProxy to create it.
    /// </summary>
    public class PluginMappingControlFactory : MappingControlFactory
    {
        private PluginHostProxy pluginHost;
        private string controlTypeName;

        internal PluginMappingControlFactory(string forTypeFullName, string imageResourceName, PluginHostProxy pluginHost, string controlTypeName)
            : base(forTypeFullName, imageResourceName)
        {
            this.pluginHost = pluginHost;
            this.controlTypeName = controlTypeName;
        }

        public override T CreateInstance<T>()
        {
            return (T)pluginHost.CreateControl(controlTypeName);
        }

        /// <summary>
        /// Since we can't get the Type in the other appdomain, we return the host/contract class it derives from instead.
        /// </summary>
        /// <returns></returns>
        public override Type ToType()
        {
            throw new InvalidOperationException("Cannot get Type in other appdomain.");
        }

        public override string GetTypeName()
        {
            return controlTypeName;
        }
    }
}