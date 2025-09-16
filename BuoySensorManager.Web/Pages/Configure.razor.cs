using BuoySensorManager.Services.Models;
using BuoySensorManager.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BuoySensorManager.Web.Pages
{
    [Route("configure")]
    public partial class Configure
    {
        [Inject]
        private IConfigurationService ConfigurationService { get; set; } = default!;

        private EditContext EditContext { get; set; } = default!;

        private ConfigRequest EditModel { get; set; } = default!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            EditModel = ConfigurationService.Get();
            EditContext = new EditContext(EditModel);
        }

        private void Save(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            if (EditContext.Validate() == false)
            {
                return;
            }

            ConfigurationService.Save(EditModel);
        }
    }
}
