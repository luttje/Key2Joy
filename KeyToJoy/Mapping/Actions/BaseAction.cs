using KeyToJoy.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class BaseAction
        : ICloneable
    {
        [JsonProperty]
        internal string Name { get; set; }

        public string ImageResource { get; set; }
        
        protected string description;
        protected TriggerListener listener;

        protected List<BaseAction> otherActions;

        public BaseAction(string name, string description)
        {
            Name = name;
            this.description = description;
        }

        internal abstract Task Execute(InputBag inputBag = null);
        
        public abstract object Clone();

        internal static BaseAction MakeAction(Type actionType, ActionAttribute typeAttribute = null)
        {
            if(typeAttribute == null)
                typeAttribute = actionType.GetCustomAttributes(typeof(ActionAttribute), true)[0] as ActionAttribute;
            
            return (BaseAction)Activator.CreateInstance(actionType, new object[]
                {
                    typeAttribute.NameFormat,
                    typeAttribute.Description
                });
        }

        /// <summary>
        /// Returns an action reference that remains during this session.
        /// 
        /// !!! I removed this because I was wrong to think this was immediately useful to me.
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        //internal BaseAction GetOtherActionByType(Type actionType)
        //{
        //    foreach (var action in otherActions)
        //    {
        //        if (action.GetType() == actionType)
        //            return action;
        //    }

        //    var attribute = actionType.GetCustomAttributes(typeof(ActionAttribute), true)[0] as ActionAttribute;
        //    var newAction = Create(actionType, attribute);
        //    otherActions.Add(newAction);
        //    return newAction;
        //}

        internal virtual void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            this.listener = listener;
            this.otherActions = otherActions;
        }

        internal virtual void OnStopListening(TriggerListener listener)
        {
            this.listener = null;
        }

        public virtual string GetNameDisplay()
        {
            return Name;
        }

        public virtual Image GetImage()
        {
            return ImageResource != null
                ? (Bitmap)Resources.ResourceManager.GetObject(ImageResource)
                : null;            
        }

        public override string ToString()
        {
            return GetNameDisplay();
        }

        public static bool operator ==(BaseAction a, BaseAction b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (ReferenceEquals(a, null)
                || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }
        public static bool operator !=(BaseAction a, BaseAction b) => !(a == b);
    }
}
