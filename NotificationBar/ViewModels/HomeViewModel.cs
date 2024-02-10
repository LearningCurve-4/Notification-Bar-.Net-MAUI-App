namespace NotificationBar.ViewModels;

public class HomeViewModel : BaseViewModel
{
	public HomeViewModel()
	{
		ReadThemeCommand.Execute(null);
	}

	string? appThemes;
	public string? AppThemes
	{
		get => appThemes;
		set
		{
			if (appThemes == value) { return; }
			appThemes = value;
			OnPropertyChanged();
			WriteThemeCommand.Execute(null);
		}
	}

	bool playNotifSound;
	public bool PlayNotifSound
	{
		get { return playNotifSound; }
		set
		{
			if (playNotifSound == value) { return; }
			playNotifSound = value;
			OnPropertyChanged();
			WriteThemeCommand.Execute(null);
		}
	}

	string? notifAudioFile;
	public string? NotifAudioFile
	{
		get { return notifAudioFile; }
		set
		{
			if (notifAudioFile == value) { return; }
			notifAudioFile = value;
			OnPropertyChanged();
			WriteThemeCommand.Execute(value ?? "notify");
		}
	}

	string? notifDisplayDuration;
	public string? NotifDisplayDuration
	{
		get { return notifDisplayDuration; }
		set
		{
			if (notifDisplayDuration == value) { return; }
			notifDisplayDuration = value;
			OnPropertyChanged();
			WriteThemeCommand.Execute(value ?? "2");
		}
	}

	public Command LoadThemeCommand => new Command<string>((str) =>
	{
		try
		{
			IsBusy = true;
			if (str != AppThemes) 
			{
				AppThemeService.LoadTheme(str);
				AppThemes = str;
				ShowNotificationCommand.Execute($"App Theme changed to {AppThemes}");
			}
		}
		finally
		{
			IsBusy = false;
		}
	}, (str) => IsNotBusy);

	public Command ReadThemeCommand => new(async () =>
	{
		try
		{
			IsBusy = true;
			string[] theme = await AppThemeService.ReadTheme();
			AppThemes = theme[0];
			PlayNotifSound = theme[1] == "0";
			NotifAudioFile = theme[2];
			NotifDisplayDuration = theme[3];
			AppThemeService.LoadTheme(AppThemes);
		}
		finally
		{
			IsBusy = false;
		}
	}, () => IsNotBusy);

	public Command WriteThemeCommand => new(async () =>
	{
		try
		{
			IsBusy = true;
			string b = PlayNotifSound ? "0" : "1" ?? "0";
			string theme = $"{AppThemes ?? "Default"},{b},{NotifAudioFile ?? "notify"},{NotifDisplayDuration ?? "2"}";
			AppThemeService.WriteTheme(theme);
			AppThemeService.LoadTheme(await AppThemeService.ReadValue(0));
		}
		finally
		{
			IsBusy = false;
		}
	}, () => IsNotBusy);

	public Command EnableDisableNotificationCommand => new(() =>
	{
		try
		{
			IsBusy = true;
			PlayNotifSound = !PlayNotifSound;
			WriteThemeCommand.Execute(null);
			if (PlayNotifSound)
			{
				ShowNotificationCommand.Execute("This is your notification bar");
			}
			else 
			{
				HideNotificationCommand.Execute(null);
			}
		}
		finally
		{
			IsBusy = false;
		}
	}, () => IsNotBusy);
}
