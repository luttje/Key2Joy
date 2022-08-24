using Key2Joy.Properties;
using Newtonsoft.Json;
using System;
using System.Drawing;

namespace Key2Joy.Mapping
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class BaseTrigger: ICloneable, IComparable<BaseTrigger>
    {
        public event EventHandler<TriggerExecutingEventArgs> Executing;

        [JsonProperty]
        internal string Name { get; set; }

        // Must return an input value unique in the preset. Like a Keys combination or an AxisDirection.
        // Will be used to quickly lookup input triggers and their corresponding action
        internal abstract string GetUniqueKey();

        /// <summary>
        /// Must return a singleton listener that will listen for triggers.
        /// 
        /// When the user starts their mappings, this listener will be given each relevant mapping to look for.
        /// </summary>
        /// <returns>Singleton trigger listener</returns>
        internal abstract TriggerListener GetTriggerListener();

        public string ImageResource { get; set; }

        protected string description;

        public DateTime LastActivated { get; private set; }
        public IInputBag LastInputBag { get; private set; }
        public bool ExecutedLastActivation { get; private set; }

        internal BaseTrigger(string name, string description)
        {
            Name = name;
            this.description = description;
        }

        public virtual bool GetShouldExecute()
        {
            var eventArgs = new TriggerExecutingEventArgs();

            Executing?.Invoke(this, eventArgs);

            return !eventArgs.Handled;
        }

        public virtual Image GetImage()
        {
            return ImageResource != null
                ? (Bitmap)Resources.ResourceManager.GetObject(ImageResource)
                : null;
        }

        public virtual int CompareTo(BaseTrigger other)
        {
            return ToString().CompareTo(other.ToString());
        }

        public static bool operator ==(BaseTrigger a, BaseTrigger b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (ReferenceEquals(a, null)
                || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }
        public static bool operator !=(BaseTrigger a, BaseTrigger b) => !(a == b);

        internal void DoActivate(IInputBag inputBag, bool executed = false)
        {
            LastActivated = DateTime.Now;
            LastInputBag = inputBag;
            ExecutedLastActivation = executed;
        }

        public abstract object Clone();
    }
}
