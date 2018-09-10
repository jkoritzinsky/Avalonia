using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using Avalonia.Metadata;
using Avalonia.Reactive;

namespace Avalonia.Data
{
    /// <summary>
    /// Binding that produces values from the earliest child binding in the list that has produced values.
    /// </summary>
    public class PriorityBinding : IBinding
    {
        /// <summary>
        /// Gets the collection of child bindings.
        /// </summary>
        [Content]
        public IList<IBinding> Bindings { get; set; } = new List<IBinding>();
        
        /// <summary>
        /// Gets or sets the value to use when the binding is unable to produce a value.
        /// </summary>
        public object FallbackValue { get; set; }

        /// <summary>
        /// Gets or sets the binding mode.
        /// </summary>
        public BindingMode Mode { get; set; } = BindingMode.OneWay;

        /// <summary>
        /// Gets or sets the binding priority.
        /// </summary>
        public BindingPriority Priority { get; set; }

        public InstancedBinding Initiate(IAvaloniaObject target, AvaloniaProperty targetProperty, object anchor = null, bool enableDataValidation = false)
        {
            var targetType = targetProperty?.PropertyType ?? typeof(object);
            var children = Bindings.Select(x => x.Initiate(target, null).Subject);

            var subject = new PrioritySubject(children.ToArray());
            var mode = Mode == BindingMode.Default ?
                targetProperty?.GetMetadata(target.GetType()).DefaultBindingMode : Mode;

            switch (mode)
            {
                case BindingMode.OneWay:
                    return InstancedBinding.OneWay(subject, Priority);
                case BindingMode.TwoWay:
                    return InstancedBinding.TwoWay(subject, Priority);
                case BindingMode.OneTime:
                    return InstancedBinding.OneTime(subject, Priority);
                case BindingMode.OneWayToSource:
                    return InstancedBinding.OneWayToSource(subject, Priority);
                default:
                    throw new NotSupportedException(
                        "Unknown BindingMode for PriorityBinding.");
            }
        }
    }
}
