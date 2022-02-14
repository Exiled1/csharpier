﻿using System;
using System.Runtime.InteropServices;
using System.Threading;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace CSharpier.VisualStudio
{
    [Guid(PackageGuidString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(CSharpierOptionsPage), "CSharpier", "General", 0, 0, true)]
    public sealed class CSharpierPackage : AsyncPackage
    {
        public const string PackageGuidString = "d348ba73-11dc-46be-8660-6d9819fc2c52";

        protected override async Task InitializeAsync(
            CancellationToken cancellationToken,
            IProgress<ServiceProgressData> progress
        )
        {
            await Logger.InitializeAsync(this);
            Logger.Instance.Info("Starting");

            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            await InfoBarService.InitializeAsync(this);
            await ReformatWithCSharpierOnSave.InitializeAsync(this);
            await ReformatWithCSharpier.InitializeAsync(this);
            await InstallerService.InitializeAsync(this);

            var dte = await this.GetServiceAsync(typeof(DTE)) as DTE;
            if (dte.ActiveDocument != null)
            {
                CSharpierProcessProvider
                    .GetInstance(this)
                    .FindAndWarmProcess(dte.ActiveDocument.FullName);
            }
        }
    }
}