using PayerPortalAutomation.Application.Interfaces;
using PayerPortalAutomation.Automation.Portals;

namespace PayerPortalAutomation.Automation.Factories;

public class PortalAutomationFactory
{
    public IPortalAutomation Create(string payerId)
    {
        return new DemoPortalAutomation();
    }
}