using Key2Joy.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class BaseAction : ICloneable, IComparable<BaseAction>
    {
        [JsonProperty]
        internal string Name { get; set; }

        public string ImageResource { get; set; }

        public bool IsStarted;

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
                    typeAttribute.NameFormat ?? actionType.Name,
                    typeAttribute.Description
                });
        }

        internal BaseAction MakeStartedAction(Type actionType)
        {
            var action = MakeAction(actionType);
            action.IsStarted = IsStarted;
            action.listener = listener;
            action.otherActions = otherActions;
            return action;
        }

        internal virtual void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            IsStarted = true;
            this.listener = listener;
            this.otherActions = otherActions;
        }

        internal virtual void OnStopListening(TriggerListener listener)
        {
            IsStarted = false;
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

        public int CompareTo(BaseAction other)
        {
            return ToString().CompareTo(other.ToString());
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
