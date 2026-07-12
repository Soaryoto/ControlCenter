using System.Collections.ObjectModel;
using System.Linq;
using Soaryoto.Infrastructure;
using Soaryoto.ViewModels.Items;

namespace Soaryoto.ViewModels.Controls;

public partial class ServiceSwitcherViewModel : ViewModelBase
{
    private readonly ManagerControllableService _managerControllableService;
    public ObservableCollection<ServiceItemViewModel> ServicesItems { get; }

    public ServiceSwitcherViewModel(ManagerControllableService managerControllableService)
    {
        _managerControllableService = managerControllableService;
        ServicesItems = new ObservableCollection<ServiceItemViewModel>(_managerControllableService.Services.Select(s => new ServiceItemViewModel(s)));
    }
}
