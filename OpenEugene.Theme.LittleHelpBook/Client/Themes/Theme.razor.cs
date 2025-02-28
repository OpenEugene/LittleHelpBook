using Microsoft.AspNetCore.Components;
using MudBlazor;
using Oqtane.Services;
using Oqtane.Shared;
using Oqtane.Themes;
using System.Threading.Tasks;

namespace OpenEugene.Theme.LittleHelpBook
{
    public partial class Theme : ThemeBase
    {
        [Inject] ISettingService SettingService { get; set; }

        public override string Name => "Theme";

        public override string Panes => PaneNames.Admin + ",Top Full Width,Top 100%,Left 50%,Right 50%,Left 33%,Center 33%,Right 33%,Left Outer 25%,Left Inner 25%,Right Inner 25%,Right Outer 25%,Left 25%,Center 50%,Right 25%,Left Sidebar 66%,Right Sidebar 33%,Left Sidebar 33%,Right Sidebar 66%,Bottom 100%,Bottom Full Width";

        private bool _login = true;
        private bool _register = true;
        private bool _isDarkMode;
        private MudThemeProvider _mudThemeProvider;

        private MudTheme _theme = new()
        {
            PaletteLight = new()
            {
                Primary = "#22AACB",
                Secondary = "#4EBC83",
                Tertiary = "#F48E5C",
                Success = "#4EBC83",
                Error = "#C4515F",
                Warning = "#FFBB3E",
                Background = "#ffffff",
                AppbarBackground = "#ffffff",
                AppbarText = "#000000",
                DrawerBackground = "#ffffff",
                DrawerText = "#000000",
                TextPrimary = "#000000",
                TextSecondary = "#000000"
            },
            PaletteDark = new()
            {
                Primary = "#22AACB",
                Secondary = "#4EBC83",
                Tertiary = "#F48E5C",
                Success = "#4EBC83",
                Error = "#C4515F",
                Warning = "#FFBB3E",
                Background = "#000000",
                AppbarBackground = "#000000",
                AppbarText = "#ffffff",
                DrawerBackground = "#000000",
                DrawerText = "#ffffff",
                TextPrimary = "#ffffff",
                TextSecondary = "#ffffff"
            }
        };

        protected override void OnParametersSet()
        {
            try
            {
                var settings = SettingService.MergeSettings(PageState.Site.Settings, PageState.Page.Settings);
                _login = bool.Parse(SettingService.GetSetting(settings, GetType().Namespace + ":Login", "true"));
                _register = bool.Parse(SettingService.GetSetting(settings, GetType().Namespace + ":Register", "true"));
            }
            catch
            {
                // error loading theme settings
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _isDarkMode = await _mudThemeProvider.GetSystemPreference();
                StateHasChanged();
            }
        }
    }
}
