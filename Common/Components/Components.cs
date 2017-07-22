using System;
using System.Threading.Tasks;

namespace Common.Components
{
    public static class Components
    {
        public static async Task<bool> SwitchOnSafe(this IComponent component)
        {
            try
            {
                await component.SwitchOn();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static async Task<bool> SwitchOffSafe(this IComponent component)
        {
            try
            {
                await component.SwitchOff();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
