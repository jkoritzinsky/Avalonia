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
            lock (_lock)
            {
                _subjects[_activeIndex].OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            lock (_lock)
            {
                _subjects[_activeIndex].OnError(error);
            }
        }

        public void OnNext(object value)
        {
            lock (_lock)
            {
                _subjects[_activeIndex].OnNext(value);
            }
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
            if (index < _activeIndex)
            {
                lock (_lock)
                {
                    if (index < _activeIndex && value != BindingOperations.DoNothing)
                    {
                        _activeIndex = index;
                        PublishNext(value);
                    }
                }
            }
        }
    }
}
