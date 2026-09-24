// Copyright (C) 2026 Soaryoto
//
// This file is part of StreamingController.
//
// Licensed under the GNU General Public License v3.0 or later.
// See LICENSE for the full license text.
//
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.ObjectModel;
using Soaryoto.Interfaces.Services;
using System.Diagnostics;
using Soaryoto.Models;
using System.Linq;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Soaryoto.Services;

public sealed class ProcessManagerService : IProcessManagerService
{
    private bool _isLocked = false;
    private ObservableCollection<ManagedProcess> _processes { get; } = new ();
    private readonly ILoggingService _loggingService;
    private static readonly Regex AnsiRegex = new(@"\x1B\[[0-9;]*[A-Za-z]");

    private Process CreateProcess(ManagedProcess processStartInfo)
    {
        Process newProcess = new Process();

        newProcess.StartInfo.FileName = processStartInfo.Executable;
        newProcess.StartInfo.Arguments = processStartInfo.Arguments;
        newProcess.StartInfo.WorkingDirectory = processStartInfo.WorkingDirectory;

        newProcess.StartInfo.UseShellExecute = false;
        newProcess.StartInfo.CreateNoWindow = true;
        newProcess.StartInfo.RedirectStandardOutput = true;
        newProcess.StartInfo.RedirectStandardError = true;
        newProcess.EnableRaisingEvents = true;

        newProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;
        newProcess.StartInfo.StandardErrorEncoding = Encoding.UTF8;

        newProcess.OutputDataReceived += OutputReceived;
        newProcess.ErrorDataReceived += ErrorReceived;

        return newProcess;
    }
    private void OutputReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data != null)
        {
            string cleanText = AnsiRegex.Replace(e.Data, "");
            _loggingService.WriteLine(cleanText, LogLevel.Info);
        }
    }

    private void ErrorReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data != null)
        {
            string cleanText = AnsiRegex.Replace(e.Data, "");
            _loggingService.WriteLine(cleanText, LogLevel.Error);   
        }
    }

    public ProcessManagerService(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    public void Build()
    {
        _isLocked = true;
    }

    public void AddProcessInfo(ManagedProcess processStartInfo)
    {
        if (_isLocked)
            throw new InvalidOperationException($"Ошибка в регистрации процесса, контейнер был закрыт");

        ManagedProcess newManagedProcess = new ManagedProcess
        {
            ProcessTypeName = processStartInfo.ProcessTypeName,
            Name = processStartInfo.Name,
            Executable = processStartInfo.Executable,
            Arguments = processStartInfo.Arguments,
            WorkingDirectory = processStartInfo.WorkingDirectory
        };
        _processes.Add(newManagedProcess);
    }

    public void StartProcess(ProcessTypeName processTypeName)
    {
        ManagedProcess? managedProcess = _processes.FirstOrDefault( process => process.ProcessTypeName == processTypeName);

        if (managedProcess == null)
            throw new InvalidOperationException($"Ошибка, процесс или процессы были пусты");
        if (managedProcess.ProcessTypeName != processTypeName)
            throw new InvalidOperationException($"Небыл найден нужный тип процесса");

        if (managedProcess.Process == null || managedProcess.Process.HasExited)
        {
            managedProcess.Process = CreateProcess(managedProcess);    
        }

        managedProcess.Process.Start();
        
        managedProcess.Process.BeginOutputReadLine();
        managedProcess.Process.BeginErrorReadLine();
    }

    public void StopProcess(ProcessTypeName processTypeName)
    {
        ManagedProcess? managedProcess = _processes.FirstOrDefault(process => process.ProcessTypeName == processTypeName);
        
        if (managedProcess == null || managedProcess.Process == null)
            throw new InvalidOperationException($"Ошибка, процесс или процессы были пусты");
        if (managedProcess.ProcessTypeName != processTypeName)
            throw new InvalidOperationException($"Небыл найден нужный тип процесса");

        if (!managedProcess.Process.HasExited)
        {
            managedProcess.Process.Kill(true);
            managedProcess.Process.WaitForExit();
        }
    }
    public void StopAllProcess()
    {
        foreach(ManagedProcess managedProcess in _processes)
        {
            if (managedProcess.Process != null && !managedProcess.Process.HasExited)
            {
                managedProcess.Process.Kill(true);
                managedProcess.Process.WaitForExit();
            }
        }
    }

    public void RestartProcess(ProcessTypeName processTypeName)
    {
        ManagedProcess? managedProcess = _processes.FirstOrDefault( process => process.ProcessTypeName == processTypeName);
        
        if (managedProcess == null || managedProcess.Process == null)
            throw new InvalidOperationException($"Ошибка, процесс или процессы были пусты");
        if (managedProcess.ProcessTypeName != processTypeName)
            throw new InvalidOperationException($"Небыл найден нужный тип процесса");

        StopProcess(processTypeName);
        StartProcess(processTypeName);
        
    }

    public void IsRunning(ProcessTypeName processTypeName)
    {
        ManagedProcess? managedProcess = _processes.FirstOrDefault( process => process.ProcessTypeName == processTypeName);
        
        if (managedProcess == null || managedProcess.Process == null)
            throw new InvalidOperationException($"Ошибка, процесс или процессы были пусты");
        if (managedProcess.ProcessTypeName != processTypeName)
            throw new InvalidOperationException($"Небыл найден нужный тип процесса");
    }
}