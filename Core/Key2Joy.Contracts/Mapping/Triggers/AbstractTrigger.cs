using System;
using System.Text.Json.Serialization;

namespace Key2Joy.Contracts.Mapping
{
    public abstract class AbstractTrigger : AbstractMappingAspect
    {
        public event EventHandler<TriggerExecutingEventArgs> Executing;

        [JsonIgnore]
        public AbstractInputBag LastInputBag { get; protected set; }
        [JsonIgnore]
        public DateTime LastActivated { get; protected set; }
        [JsonIgnore]
        public bool ExecutedLastActivation { get; protected set; }

        public AbstractTrigger(string name)
            : base(name) { }

        /// <summary>
        /// Must return a singleton listener that will listen for triggers.
        /// 
        /// When the user starts their mappings, this listener will be given each relevant mapping to look for.
        /// </summary>
        /// <returns>Singleton trigger listener</returns>
        public abstract AbstractTriggerListener GetTriggerListener();

        /// <summary>
        /// Must return an input value unique in the profile. Like a Keys combination or an AxisDirection.
        /// Will be used to quickly lookup input triggers and their corresponding action
        /// </summary>
        /// <returns></returns>
        public abstract string GetUniqueKey();

        public virtual bool GetShouldExecute()
        {
            TriggerExecutingEventArgs eventArgs = new();

            Executing?.Invoke(this, eventArgs);

            return !eventArgs.Handled;
        }

        public virtual void DoActivate(AbstractInputBag inputBag, bool executed = false)
        {
            LastActivated = DateTime.Now;
            LastInputBag = inputBag;
            ExecutedLastActivation = executed;
        }
    }
}
