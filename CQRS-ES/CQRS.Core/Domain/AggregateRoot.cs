using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace CQRS.Core.Domain
{
    public abstract class AggregateRoot
    {
        protected Guid _id;
        private readonly List<BaseEvent> _changes = new();

        public Guid Id
        {
            get { return _id; }
        }

        public int Version { get; set; } = -1;

        public IEnumerable<BaseEvent> GetUncommitedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommited()
        {
            _changes.Clear();
        }

        private void ApplyChange(BaseEvent @event, bool isNew)
        {
            /*aqui o this não se refere à classe AggregateRoot, já que não existe instância de classes abstratas, ela se refere ao 
            AggregateRoot concreto, isso se chama reflection */

            var method = this.GetType().GetMethod("Apply", new Type[] { @event.GetType() });

            if (null == method)
            {
                throw new ArgumentNullException(nameof(method), $"The apply method was not found in the aggregate for {@event.GetType().Name}.");
            }

            method.Invoke(this, new object[] { @event });

            if (isNew)
            {
                _changes.Add(@event);
            }
        }

        protected void RaiseEvent(BaseEvent @event)
        {
            ApplyChange(@event, true);
        }

        public void ReplayEvents(IEnumerable<BaseEvent> events)
        {
            foreach (var @event in events)
            {
                ApplyChange(@event, false);
            }
        }
    }
}