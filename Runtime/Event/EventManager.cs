using System;
using System.Collections.Generic;

namespace BlueSnake.Event {
    public class EventManager {
        private readonly Dictionary<Type, List<EventSubscriber>> subscribers = new();

        public void Subscribe<T>(Type type, EventSubscriber<T> subscriber) where T : IEvent {
            if (!subscribers.ContainsKey(type)) subscribers.Add(type, new List<EventSubscriber>());

            subscribers[type].Add(subscriber);
        }

        public void Subscribe<T>(Action<T> action) where T : IEvent {
            Subscribe(typeof(T), new EventSubscriber<T>(action));
        }

        public void Subscribe<T>(string scope, Action<T> action) where T : IEvent {
            Subscribe(typeof(T), new EventSubscriber<T>(scope, action));
        }

        public void UnsubscribeAll(string scope) {
            foreach (List<EventSubscriber> subscribers in this.subscribers.Values) {
                List<EventSubscriber> remove = new List<EventSubscriber>();
                foreach (EventSubscriber subscriber in subscribers)
                    if (subscriber.scope.Equals(scope))
                        remove.Add(subscriber);

                foreach (EventSubscriber subscriber in remove) subscribers.Remove(subscriber);
            }
        }

        public void Publish<T>(T callable) where T : IEvent {
            if (subscribers.ContainsKey(callable.GetType()))
                foreach (EventSubscriber eventSubscriber in subscribers[callable.GetType()]) {
                    EventSubscriber<T> sub = (EventSubscriber<T>)eventSubscriber;
                    sub.OnCall(callable);
                }
        }
    }

    public class EventSubscriber {
        public string scope;
    }

    public class EventSubscriber<T> : EventSubscriber where T : IEvent {
        private readonly Action<T> action;

        public EventSubscriber(Action<T> action) {
            scope = "Global";
            this.action = action;
        }

        public EventSubscriber(string scope, Action<T> action) {
            this.scope = scope;
            this.action = action;
        }

        public void OnCall(T objectEvent) {
            action(objectEvent);
        }
    }

    public interface IEvent { }

    public interface ICancelableEvent : IEvent {
        bool IsCancelled();
        void SetCancelled(bool value);
    }
}