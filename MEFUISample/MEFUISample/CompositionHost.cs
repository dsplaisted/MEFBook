// -----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using System.IO;
using System.Threading;

namespace System.ComponentModel.Composition.Hosting
{
    public static class CompositionHost
    {
        // Field is internal only to assist in testing
        internal static CompositionContainer _container = null;
        private static object _lockObject = new object();

        /// <summary>
        ///     This method can be used to initialize the global container used by <see cref="CompositionInitializer.SatisfyImports"/>
        ///     in case where the default container doesn't provide enough flexibility. 
        ///     
        ///     If this method is needed it should be called exactly once and as early as possible in the application host. It will need
        ///     to be called before the first call to <see cref="CompositionInitializer.SatisfyImports"/>
        /// </summary>
        /// <param name="container">
        ///     <see cref="CompositionContainer"/> that should be used instead of the default global container.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="container"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Either <see cref="Initialize" /> has already been called or someone has already made use of the global 
        ///     container via <see cref="CompositionInitializer.SatisfyImports"/>. In either case you need to ensure that it 
        ///     is called only once and that it is called early in the application host startup code.
        /// </exception>
        public static void Initialize(CompositionContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            CompositionContainer globalContainer = null;
            bool alreadyCreated = TryGetOrCreateContainer(() => container, out globalContainer);

            if (alreadyCreated)
            {
				throw new InvalidOperationException("The container has already been initialized either by another call to Initialize or by someone causing the default container to be constructed. Ensure that Initialize is one of the first things that happens in the application host to ensure that it is ready for the first composition.");
            }
        }

        internal static bool TryGetOrCreateContainer(Func<CompositionContainer> createContainer, out CompositionContainer globalContainer)
        {
            bool alreadyCreated = true;
            if (_container == null)
            {
                var container = createContainer.Invoke();
                lock (_lockObject)
                {
                    if (_container == null)
                    {
                        Thread.MemoryBarrier();
                        _container = container;
                        alreadyCreated = false;
                    }
                }
            }
            globalContainer = _container;
            return alreadyCreated;
        }

        public static ComposablePartCatalog GetCatalog()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(@".\"));
            catalog.Catalogs.Add(new DirectoryCatalog(@".\", "*.exe"));
            var directory = @".\Extensions";
            if (Directory.Exists(directory))
                catalog.Catalogs.Add(new DirectoryCatalog(directory));
            return catalog;
        }

    }
}