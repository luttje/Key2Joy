using Key2Joy.Config;
using Key2Joy.Contracts.Mapping;
using System;
using System.Collections.Generic;

namespace Key2Joy.Mapping
{
    public abstract class CoreTriggerListener : AbstractTriggerListener
    {
        protected bool IsActive { get; private set; }

        protected IList<AbstractTriggerListener> allListeners;

        public override void StartListening(ref IList<AbstractTriggerListener> allListeners)
        {
            if (IsActive)
                throw new Exception("Shouldn't StartListening to already active listener!");

            this.allListeners = allListeners;

            Start();
        }

        public override void StopListening()
        {
            if (!IsActive)
                return;

            Stop();

            allListeners = null;
        }

        protected virtual void Start()
        {
            IsActive = true;
        }

        protected virtual void Stop()
        {
            IsActive = false;
        }

        /// <summary>
        /// Subclasses MUST call this to have their actions executed.
        /// 
        /// Even when they know no actions are listening, they should call this. This
        /// lets events provide other mapped options to be injected.
        /// </summary>
        /// <param name="mappedOptions"></param>
        /// <param name="inputBag"></param>
        /// <param name="optionCandidateFilter"></param>
        protected override bool DoExecuteTrigger(
            IList<AbstractMappedOption> mappedOptions,
            AbstractInputBag inputBag,
            Func<AbstractTrigger, bool> optionCandidateFilter = null)
        {
            var executedAny = base.DoExecuteTrigger(mappedOptions, inputBag, optionCandidateFilter);

            return ConfigManager.Config.OverrideDefaultTriggerBehaviour ? executedAny : false;
        }
    }
}
