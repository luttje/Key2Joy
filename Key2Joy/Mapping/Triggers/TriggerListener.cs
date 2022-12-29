using Key2Joy.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Mapping
{
    public abstract class TriggerListener
    {
        public event EventHandler<TriggerActivatingEventArgs> TriggerActivating;
        public event EventHandler<TriggerActivatedEventArgs> TriggerActivated;
        
        public virtual bool HasWndProcHandle { get; } = false;
        public IntPtr Handle { get; set; }

        public bool IsActive { get; private set; }

        protected List<TriggerListener> allListeners;

        public void StartListening(ref List<TriggerListener> allListeners)
        {
            if (IsActive)
                throw new Exception("Shouldn't StartListening to already active listener!");

            this.allListeners = allListeners;

            Start();
        }
        public void StopListening()
        {
            if (!IsActive)
                return;

            Stop();

            allListeners = null;
        }

        public abstract void AddMappedOption(MappedOption mappedOption);

        public abstract bool GetIsTriggered(BaseTrigger trigger);

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
        protected virtual bool DoExecuteTrigger(
            List<MappedOption> mappedOptions, 
            IInputBag inputBag,
            Func<BaseTrigger, bool> optionCandidateFilter = null)
        {
            var eventArgs = new TriggerActivatingEventArgs(
                this, 
                inputBag, 
                mappedOptions ?? new List<MappedOption>(), 
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
                    mappedOption.Action.Execute(inputBag);
                }
            }

            TriggerActivated?.Invoke(this, new TriggerActivatedEventArgs(
                this, 
                inputBag, 
                eventArgs.MappedOptionCandidates));

            return ConfigManager.Instance.OverrideDefaultTriggerBehaviour ? executedAny : false;
        }

        public virtual void WndProc(ref Message m)
        { }
    }
}
