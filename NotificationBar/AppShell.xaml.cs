namespace NotificationBar;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		PlatformHandler.InitializeCustomHandler();
		InitializeRouting();
		InitializeAppFiles();
	}

	public void InitializeRouting()
	{
		Routing.RegisterRoute(nameof(AboutAppPage), typeof(AboutAppPage));
	}

	public static void InitializeAppFiles()
	{
		if (!File.Exists(AppThemeService.appThemeFile))
		{
			File.WriteAllText(AppThemeService.appThemeFile, AppThemeService.defaultTheme);
		}
	}
}
