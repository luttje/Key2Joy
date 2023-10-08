using System.Collections.Generic;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Contracts.Mapping.Actions
{
    public abstract class AbstractAction : AbstractMappingAspect
    {
        public bool IsStarted;

        protected AbstractTriggerListener listener;

        protected IList<AbstractAction> otherActions;

        public virtual async Task Execute(AbstractInputBag inputBag = null)
        { }

        public virtual string GetNameDisplay() => this.Name;

        public AbstractAction(string name)
            : base(name) { }

        public virtual void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
        {
            this.SetStartData(listener, ref otherActions);
        }

        public virtual void OnStopListening(AbstractTriggerListener listener)
        {
            this.IsStarted = false;

            this.listener = null;
        }

        public void SetStartData(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
        {
            this.IsStarted = true;
            this.listener = listener;
            this.otherActions = otherActions;
        }
    }
}
