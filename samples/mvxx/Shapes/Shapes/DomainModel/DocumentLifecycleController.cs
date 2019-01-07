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
        public interface IDelegate
        {
            Option<string> RequestDocumentOpenPath();
            Common.ClosingAction RequestUnsavedDocumentClosing();
            Option<string> RequestDocumentSavePath();

            void OnEraseMemoryDocument();
            void OnOpenDocument(string path);
            void OnSaveDocument(string path);
        }

        private bool _modified = false;
        private Option<string> _path = Option.None<string>();
        private readonly IDelegate _delegate;

        public DocumentLifecycleController(IDelegate delegate_)
        {
            _delegate = delegate_;
        }

        public bool New()
        {
            if (IsModified())
            {
                var closingAction = _delegate.RequestUnsavedDocumentClosing();
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

            _delegate.OnEraseMemoryDocument();
            _modified = false;
            _path = Option.None<string>();
            return true;
        }

        public bool Open()
        {
            if (!New())
            {
                return false;
            }

            var path = _delegate.RequestDocumentOpenPath();
            if (!path.HasValue)
            {
                return false;
            }

            _path = path;
            _delegate.OnOpenDocument(_path.ValueOrFailure());
            return true;
        }

        public bool Save(bool forcePathRequest)
        {
            if (!_path.HasValue || forcePathRequest)
            {
                var path = _delegate.RequestDocumentSavePath();
                if (!path.HasValue)
                {
                    return false;
                }
                _path = path;
            }

            _delegate.OnSaveDocument(_path.ValueOrFailure());
            _modified = false;
            return true;
        }

        public void Modify()
        {
            _modified = true;
        }

        public bool IsModified()
        {
            return _modified;
        }
    }
}
