using Key2Joy.Contracts.Mapping.Actions;
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
        
        public abstract Task Execute(IInputBag inputBag = null);

        public virtual string GetNameDisplay() => Name;

        public AbstractAction(string name)
            : base(name) { }
        
        public virtual ActionOptions SaveOptions()
        {
            //return new ActionOptions()
            //{
            //    { nameof(Name), Name },
            //};
            var type = GetType();
            var properties = type.GetProperties();
            var options = new ActionOptions();

            foreach (var property in properties)
            {
                var value = property.GetValue(this);
                options.Add(property.Name, value);
            }

            return options;
        }

        public virtual void LoadOptions(ActionOptions options)
        {
            //Name = (string)options[nameof(Name)];
            var type = GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (!options.ContainsKey(property.Name))
                {
                    continue;
                }

                var value = options[property.Name];
                property.SetValue(this, value);
            }
        }

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
