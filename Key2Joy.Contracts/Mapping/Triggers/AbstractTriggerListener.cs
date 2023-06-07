using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Mapping
{
    public abstract class AbstractTriggerListener : MarshalByRefObject
    {
        public event EventHandler<TriggerActivatingEventArgs> TriggerActivating;
        public event EventHandler<TriggerActivatedEventArgs> TriggerActivated;

        public abstract void AddMappedOption(AbstractMappedOption mappedOption);

        public abstract bool GetIsTriggered(AbstractTrigger trigger);

        public abstract void StartListening(ref IList<AbstractTriggerListener> allListeners);
        public abstract void StopListening();

        /// <summary>
        /// Subclasses MUST call this to have their actions executed.
        /// 
        /// Even when they know no actions are listening, they should call this. This
        /// lets events provide other mapped options to be injected.
        /// </summary>
        /// <param name="mappedOptions"></param>
        /// <param name="inputBag"></param>
        /// <param name="optionCandidateFilter"></param>
        protected virtual bool DoExecuteTrigger(
            IList<AbstractMappedOption> mappedOptions,
            AbstractInputBag inputBag,
            Func<AbstractTrigger, bool> optionCandidateFilter = null)
        {
            var eventArgs = new TriggerActivatingEventArgs(
                this,
                inputBag,
                mappedOptions ?? new List<AbstractMappedOption>(),
                optionCandidateFilter);
            TriggerActivating?.Invoke(this, eventArgs);
            bool executedAny = false;

            foreach (var mappedOption in eventArgs.MappedOptionCandidates)
            {
                var shouldExecute = mappedOption.Trigger.GetShouldExecute();

                mappedOption.Trigger.DoActivate(inputBag, shouldExecute);

                if (shouldExecute)
                {
                    executedAny = true;
                    Task.Run(() => mappedOption.Action.Execute(inputBag));
                }
            }

            TriggerActivated?.Invoke(this, new TriggerActivatedEventArgs(
                this,
                inputBag,
                eventArgs.MappedOptionCandidates));

            return executedAny;
        }
    }
}
