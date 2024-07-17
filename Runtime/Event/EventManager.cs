using System;
using System.Collections.Generic;
using System.Linq;
using BlueSnake.Utils;

namespace BlueSnake.Event {
    public class EventManager : StandaloneSingleton<EventManager> {
        private readonly Dictionary<Type, List<EventSubscriber>> subscribers = new();

        public void Subscribe<T>(Type type, EventSubscriber<T> subscriber) where T : IEvent {
            if (!subscribers.ContainsKey(type)) subscribers.Add(type, new List<EventSubscriber>());
            subscribers[type].Add(subscriber);
        }

        public EventSubscriber<T> Subscribe<T>(Action<T> action) where T : IEvent {
            EventSubscriber<T> subscriber = new EventSubscriber<T>(action);
            Subscribe(typeof(T), subscriber);
            return subscriber;
        }

        public EventSubscriber<T> Subscribe<T>(string scope, Action<T> action) where T : IEvent {
            EventSubscriber<T> subscriber = new EventSubscriber<T>(scope, action);
            Subscribe(typeof(T), subscriber);
            return subscriber;
        }

        public void Unsubscribe<T>(EventSubscriber<T> subscriber) where T : IEvent {
            Type type = typeof(T);
            if (subscribers.TryGetValue(type, out var list)) {
                list.Remove(subscriber);
            }
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

        public void UnsubscribeAll() {
            subscribers.Clear();
        }

        public void Publish<T>(T callable) where T : IEvent {
            if (!subscribers.ContainsKey(callable.GetType())) return;
            foreach (var sub in subscribers[callable.GetType()].Cast<EventSubscriber<T>>()) {
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