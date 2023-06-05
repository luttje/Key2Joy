﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Mapping
{
    public abstract class AbstractTrigger : AbstractMappingAspect
    {
        public event EventHandler<TriggerExecutingEventArgs> Executing;

        public IInputBag LastInputBag { get; protected set; }
        public DateTime LastActivated { get; protected set; }
        public bool ExecutedLastActivation { get; protected set; }
        
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
            var eventArgs = new TriggerExecutingEventArgs();

            Executing?.Invoke(this, eventArgs);

            return !eventArgs.Handled;
        }

        public virtual void DoActivate(IInputBag inputBag, bool executed = false)
        {
            LastActivated = DateTime.Now;
            LastInputBag = inputBag;
            ExecutedLastActivation = executed;
        }
    }
}
