// <copyright file="ContainerFactory.cs" company="Ensage">
//    Copyright (c) 2017 Ensage.
// </copyright>

namespace Ensage.SDK.Service
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Reflection;

    using global::Ensage.SDK.Service.Renderer.D2D;

    using log4net;

    using PlaySharp.Toolkit.Logging;

    public static class ContainerFactory
    {
        private static readonly ILog Log = AssemblyLogs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static AggregateCatalog catalog;

        public static AggregateCatalog Catalog
        {
            get
            {
                if (catalog == null)
                {
                    catalog = new AggregateCatalog();
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        Log.Debug($"Add Catalog {assembly.GetName().Name}");
                        catalog.Catalogs.Add(new AssemblyCatalog(assembly));
                    }
                }

                return catalog;
            }

            private set
            {
                catalog = value;
            }
        }

        public static ContextContainer<IEnsageServiceContext> CreateContainer(
            IEnsageServiceContext context,
            bool disableSilent = false)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Log.Debug($"Create Context({context}) Container");

            var flags = CompositionOptions.IsThreadSafe;
            if (disableSilent)
            {
                flags |= CompositionOptions.DisableSilentRejection;
            }

            var container = new CompositionContainer(Catalog, flags);

            container.ComposeExportedValue(context);

            switch (Drawing.RenderMode)
            {
                case RenderMode.Dx11:
                    container.ComposeExportedValue<ID2DContext>(new D2DContext());
                    container.ComposeExportedValue<ID2DRenderer>(new D2DRenderer());
                    break;

                case RenderMode.Dx9:
                case RenderMode.OpenGL:
                case RenderMode.Vulkan:
                    throw new NotSupportedException($"RenderMode({Drawing.RenderMode}) not supported.");
            }

            return new ContextContainer<IEnsageServiceContext>(context, container);
        }
    }
}