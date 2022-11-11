using ApocalipseZ;
using System;

public interface IWeaponManager
{

    public event Action<IWeapon> OnActiveWeapon;

    void SetFpsPlayer(FpsPlayer player);
    void DesEquipWeapon();
    IWeapon GetActiveWeapon();


}