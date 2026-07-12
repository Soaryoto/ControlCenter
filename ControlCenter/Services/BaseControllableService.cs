using Soaryoto.Models;

namespace Soaryoto.Services;

public class BaseControllableService
{
    public ServicesName Name { get; protected set; } = ServicesName.BaseModel;
    public ServicesState State { get; protected set; } = ServicesState.Stopped;
}