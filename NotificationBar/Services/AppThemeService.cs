namespace NotificationBar.Services;

public class AppThemeService
{
	public readonly static string appThemeFile = Path.Combine(FileSystem.AppDataDirectory, "apptheme.txt");
	public readonly static string defaultTheme = "Default," +  //App Theme
												 "0," +  //Play Notification Sound
											     "notify," +  //Notification Audio File
												 "2";  //Notifification Display Duration


	public AppThemeService()
	{
		if (!File.Exists(appThemeFile))
		{
			File.WriteAllText(appThemeFile, defaultTheme);
		}
	}

	public static Task<string[]> ReadTheme()
	{
		string contents = File.ReadAllText(appThemeFile);
		return Task.FromResult(contents.Split(','));
	}

	public static Task<string> ReadValue(int num)
	{
		string contents = File.ReadAllText(appThemeFile);
		string[] sa = contents.Split(',');
		return Task.FromResult(sa[num]);
	}

	public static void WriteTheme(string theme)
	{
		File.WriteAllText(appThemeFile, theme);
	}

	public static void LoadTheme(string? theme)
	{
		if (string.IsNullOrEmpty(theme)) { return; }
		Collection<ResourceDictionary> mergedDictionaries = (Collection<ResourceDictionary>)Shell.Current.Resources.MergedDictionaries;
		if (mergedDictionaries != null)
		{
			mergedDictionaries.Clear();
			mergedDictionaries.Add(new CommonStyle());

			switch (theme)
			{
				case "Black":
					mergedDictionaries.Add(new BlackTheme());
					break;
				case "White":
					mergedDictionaries.Add(new WhiteTheme());
					break;
				case "Orange":
					mergedDictionaries.Add(new OrangeTheme());
					break;
				case "Purple":
					mergedDictionaries.Add(new PurpleTheme());
					break;
				default:
					mergedDictionaries.Add(new DefaultTheme());
					break;
			}

			Shell.Current.Resources.TryGetValue(GlobalVariables.hdrResourceKey, out object hdrBgColor); Color statusBarColor = (Color)hdrBgColor;
			Shell.Current.Resources.TryGetValue(GlobalVariables.ftrResourceKey, out object ftrBgColor); Color navigationBarColor = (Color)ftrBgColor;
			double brightnessStatusBarColor = statusBarColor.Red * .3 + statusBarColor.Green * .59 + statusBarColor.Blue * .11;
			double brightnessNavigationBarColor = navigationBarColor.Red * .3 + navigationBarColor.Green * .59 + navigationBarColor.Blue * .11;
			PlatformsService.SetStatusNavigationBarsColor(statusBarColor, brightnessStatusBarColor > 0.5, navigationBarColor, brightnessNavigationBarColor > 0.5);
		}
	}
}
