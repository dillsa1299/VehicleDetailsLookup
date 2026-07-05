// Applies theme to the document root before Blazor paints and when the user toggles light/dark.
// Storage key must match ThemePreferenceStorage.LocalStorageKey in the client project.

window.vdlookupApplyTheme = function (isDark) {
    const root = document.documentElement;
    window.__vdlookupThemeIsDark = isDark;
    root.classList.toggle('mud-theme-dark', isDark);
    root.style.removeProperty('background-color');
    root.style.removeProperty('color-scheme');
};

(function () {
    const storageKey = 'vdlookup-theme-mode';
    const darkValue = 'dark';

    function readIsDark() {
        try {
            return localStorage.getItem(storageKey) === darkValue;
        } catch {
            return false;
        }
    }

    window.vdlookupApplyTheme(readIsDark());
})();
