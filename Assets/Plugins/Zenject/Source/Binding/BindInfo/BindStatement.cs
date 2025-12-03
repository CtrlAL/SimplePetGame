using System;
using System.Collections.Generic;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    [NoReflectionBaking]
    public class BindStatement : IDisposable
    {
        readonly List<IDisposable> _compositeDisposable;
        IBindingFinalizer _bindingFinalizer;

        public BindStatement()
        {
            _compositeDisposable = new List<IDisposable>();
            Reset();
        }

        public BindingInheritanceMethods BindingInheritanceMethod
        {
            get
            {
                AssertHasFinalizer();
                return _bindingFinalizer.BindingInheritanceMethod;
            }
        }

        public bool HasFinalizer
        {
            get { return _bindingFinalizer != null; }
        }

        public void SetFinalizer(IBindingFinalizer bindingFinalizer)
        {
            _bindingFinalizer = bindingFinalizer;
        }

        void AssertHasFinalizer()
        {
            if (_bindingFinalizer == null)
            {
                throw Assert.CreateException(
                    "Unfinished binding!  Some required information was left unspecified.");
            }
        }

        public void AddDisposable(IDisposable disposable)
        {
            _compositeDisposable.Add(disposable);
        }

        public BindInfo SpawnBindInfo()
        {
            var bindInfo = ZenPools.SpawnBindInfo();
            AddDisposable(bindInfo);
            return bindInfo;
        }

        public void FinalizeBinding(DiContainer container)
        {
            AssertHasFinalizer();
            _bindingFinalizer.FinalizeBinding(container);
        }

        public void Reset()
        {
            _bindingFinalizer = null;

            for (int i = 0; i < _compositeDisposable.Count; i++)
            {
                _compositeDisposable[i].Dispose();
            }

            _compositeDisposable.Clear();
        }

        public void Dispose()
        {
            ZenPools.DespawnStatement(this);
        }
    }
}
