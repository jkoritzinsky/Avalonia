using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia.Data;

namespace Avalonia.Reactive
{
    public class PrioritySubject : LightweightObservableBase<object>, ISubject<object>
    {
        private readonly IReadOnlyList<ISubject<object>> _subjects;
        private IDisposable subscriptions;
        private int _activeIndex;
        private object _lock;

        public PrioritySubject(IReadOnlyList<ISubject<object>> subjects)
        {
            _subjects = subjects;
            _activeIndex = _subjects.Count - 1;
        }

        public void OnCompleted()
        {
            int activeIndex;
            lock (_lock)
            {
                activeIndex = _activeIndex;
            }
            
            _subjects[activeIndex].OnCompleted();
        }

        public void OnError(Exception error)
        {
            int activeIndex;
            lock (_lock)
            {
                activeIndex = _activeIndex;
            }
            
            _subjects[activeIndex].OnError(error);
        }

        public void OnNext(object value)
        {
            int activeIndex;
            lock (_lock)
            {
                activeIndex = _activeIndex;
            }
            
            _subjects[activeIndex].OnNext(value);
        }

        protected override void Deinitialize()
        {
            subscriptions.Dispose();
        }

        protected override void Initialize()
        {
            subscriptions = new CompositeDisposable(
                _subjects
                    .Select((sub, i) =>
                        sub
                            .Where(o => o != AvaloniaProperty.UnsetValue)
                            .Subscribe(o => InnerSubjectOnNext(i, o))));
        }

        private void InnerSubjectOnNext(int index, object value)
        {
            var publishValue = false;
            if (index < _activeIndex)
            {
                lock (_lock)
                {
                    if (index < _activeIndex)
                    {
                        _activeIndex = index;
                        publishValue = true;
                    }
                }
            }
            
            if (publishValue)
            {
                PublishNext(value);
            }
        }
    }
}
