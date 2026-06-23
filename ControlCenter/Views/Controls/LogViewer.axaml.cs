using System;
using System.Collections.Specialized;
using System.IO;
using Avalonia.Controls;
using Avalonia.Threading;
using Soaryoto.ViewModels.Controls;

namespace Soaryoto.Views.Controls;

public partial class LogViewer : UserControl
{
    private LogViewerViewModel? _viewModel;
    public LogViewer()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (_viewModel != null)
        {
            ((INotifyCollectionChanged)_viewModel.Logs)
                .CollectionChanged += Logs_CollectionChanged;
        }

        _viewModel = DataContext as LogViewerViewModel;

        if (_viewModel != null)
        {
            ((INotifyCollectionChanged)_viewModel.Logs)
                .CollectionChanged += Logs_CollectionChanged;
        }
    }
    private void Logs_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Dispatcher.UIThread.Post(() =>
        {
            File.AppendAllText("debug.txt", $"Items: {LogListBox.ItemCount}\n");
            if (LogListBox.ItemCount > 0)
            {
                File.AppendAllText("debug.txt", $"Items: {LogListBox.ItemCount}\n");
                LogListBox.ScrollIntoView(LogListBox.ItemCount - 1);
            }
        });
    }
}