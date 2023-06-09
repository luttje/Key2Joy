using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Mapping
{
    public abstract class AbstractAction : AbstractMappingAspect
    {
        public bool IsStarted;

        protected AbstractTriggerListener listener;

        protected IList<AbstractAction> otherActions;
        
        public virtual async Task Execute(AbstractInputBag inputBag = null)
        { }

        public virtual string GetNameDisplay() => Name;

        public AbstractAction(string name)
            : base(name) { }
        
        public virtual void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions) 
        {
            SetStartData(listener, ref otherActions);
        }

        public virtual void OnStopListening(AbstractTriggerListener listener) 
        {
            IsStarted = false;

            this.listener = null;
        }

        public void SetStartData(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
        {
            IsStarted = true;
            this.listener = listener;
            this.otherActions = otherActions;
        }
    }
}
