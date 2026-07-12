using Soaryoto.Models;

namespace Soaryoto.Interfaces.Services;

public interface IControllableService
{
    ServicesName Name { get; }

    ServicesState State { get; }

    void Toggle();
}