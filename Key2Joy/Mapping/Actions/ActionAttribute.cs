﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    public class ActionAttribute : MappingAttribute
    {
        /// <summary>
        /// When this action should be visibile in menu's.
        /// </summary>
        public ActionVisibility Visibility { get; set; } = ActionVisibility.Always;

        /// <summary>
        /// Gets all action types and their attribute annotations
        /// </summary>
        /// <param name="forTopLevel"></param>
        /// <returns></returns>
        public static Dictionary<Type, ActionAttribute> GetAllActions()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute(typeof(ActionAttribute), false) != null)
                .ToDictionary(t => t, t => t.GetCustomAttribute(typeof(ActionAttribute), false) as ActionAttribute);
        }
        
        /// <summary>
        /// Gets all action types and their attribute annotations depending on the specified visibility
        /// </summary>
        /// <param name="forTopLevel"></param>
        /// <returns></returns>
        public static SortedDictionary<ActionAttribute, Type> GetAllActions(bool forTopLevel)
        {
            return new SortedDictionary<ActionAttribute, Type>(
                Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                {
                    var actionAttribute = t.GetCustomAttributes(typeof(ActionAttribute), false).FirstOrDefault() as ActionAttribute;

                    if (actionAttribute == null
                    || actionAttribute.Visibility == ActionVisibility.Never)
                        return false;

                    if (forTopLevel)
                        return actionAttribute.Visibility == ActionVisibility.Always
                            || actionAttribute.Visibility == ActionVisibility.OnlyTopLevel;

                    return actionAttribute.Visibility == ActionVisibility.Always || actionAttribute.Visibility == ActionVisibility.UnlessTopLevel;
                })
                .ToDictionary(t => t.GetCustomAttribute(typeof(ActionAttribute), false) as ActionAttribute, t => t)
            );
        }
    }
}