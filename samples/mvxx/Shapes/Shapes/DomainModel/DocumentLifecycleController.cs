using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;
using Optional.Unsafe;

namespace Shapes.DomainModel
{
    class DocumentLifecycleController
    {
        public interface ILifecycleActionEvents
        {
            void OnEraseDocument();
            void OnFillDocumentData(string path);
            void OnExportDocumentData(string path);
        }

        public interface ISynchronizationEvents
        {
            bool IsDocumentSynced();
            void OnSyncDocument();
        }

        public interface IEvents
        {
            Common.ILifecycleDecisionEvents LifecycleDecisionEvents { get; }
            ILifecycleActionEvents LifecycleActionEvents { get; }
            ISynchronizationEvents SynchronizationEvents { get; }
        }

        private Option<string> _path = Option.None<string>();
        private readonly IEvents _events;

        public DocumentLifecycleController(IEvents events)
        {
            _events = events;
        }

        public bool New()
        {
            if (!_events.SynchronizationEvents.IsDocumentSynced())
            {
                var closingAction = _events.LifecycleDecisionEvents.RequestUnsavedClosing();
                if (closingAction == Common.ClosingAction.DontClose)
                {
                    return false;
                }

                if (closingAction == Common.ClosingAction.Save)
                {
                    if (!Save(false))
                    {
                        return false;
                    }
                }
            }

            _events.LifecycleActionEvents.OnEraseDocument();
            _events.SynchronizationEvents.OnSyncDocument();
            _path = Option.None<string>();
            return true;
        }

        public bool Open()
        {
            if (!New())
            {
                return false;
            }

            var path = _events.LifecycleDecisionEvents.RequestOpenPath();
            if (!path.HasValue)
            {
                return false;
            }

            _path = path;
            _events.LifecycleActionEvents.OnFillDocumentData(_path.ValueOrFailure());
            return true;
        }

        public bool Save(bool forcePathRequest)
        {
            if (!_path.HasValue || forcePathRequest)
            {
                var path = _events.LifecycleDecisionEvents.RequestSavePath();
                if (!path.HasValue)
                {
                    return false;
                }
                _path = path;
            }

            _events.LifecycleActionEvents.OnExportDocumentData(_path.ValueOrFailure());
            _events.SynchronizationEvents.OnSyncDocument();
            return true;
        }
    }
}
