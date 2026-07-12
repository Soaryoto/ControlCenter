using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Soaryoto.Interfaces.Services;
using Soaryoto.Models;

namespace Soaryoto.ViewModels.Items;

public class ServiceItemViewModel
{
    private readonly IControllableService _service;
    public ServicesName Name => _service.Name;

    public ServicesState State => _service.State;
    public ICommand ToggleCommand { get; }

    public ServiceItemViewModel(IControllableService service)
    {
        _service = service;
        ToggleCommand = new RelayCommand(() => _service.Toggle());
    }
}