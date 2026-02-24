using Blazored.LocalStorage;
using MudBlazor;

namespace GetteGarage.Client.Services
{
    public class AchievementService
    {
        private readonly ISnackbar _snackbar;
        private readonly ILocalStorageService _localStorage;
        private const string Key = "achievements_v1";

        public AchievementService(ISnackbar snackbar, ILocalStorageService localStorage)
        {
            _snackbar = snackbar;
            _localStorage = localStorage;
        }

        public async Task Unlock(string id, string title, string description)
        {
            var unlocked = await _localStorage.GetItemAsync<List<string>>(Key) ?? new List<string>();
            if (unlocked.Contains(id)) return;

            unlocked.Add(id);
            await _localStorage.SetItemAsync(Key, unlocked);

            // Use the Custom Component logic
            ShowCustomToast(title, description);
        }

        private void ShowCustomToast(string title, string desc)
        {
            // Pack the parameters to send to the component
            var parameters = new Dictionary<string, object>
            {
                { "Title", title },
                { "Description", desc }
            };

            // Render <RetroToast> inside the Snackbar
            _snackbar.Add<GetteGarage.Client.Components.Shared.RetroToast>(parameters, Severity.Normal, config =>
            {
                config.Icon = Icons.Material.Filled.EmojiEvents;
                config.IconColor = Color.Primary;
                config.ShowCloseIcon = false;
                config.VisibleStateDuration = 4000;
                
                // Make it look nice
                config.BackgroundBlurred = true;
                config.SnackbarVariant = Variant.Filled;
            });
        }
    }
}