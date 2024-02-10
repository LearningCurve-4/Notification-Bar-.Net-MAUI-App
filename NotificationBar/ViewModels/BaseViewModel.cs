namespace NotificationBar.ViewModels;

public class BaseViewModel : NotifyPropertyChanged
{
	bool isBusy = false;
	public bool IsBusy
	{
		get => isBusy;
		set
		{
			if (isBusy == value) { return; }
			isBusy = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(IsNotBusy));
		}
	}
	public bool IsNotBusy => !IsBusy;

	public static string CurrentPage { get; set; } = string.Empty;
	public Command GoToPageCommand => new Command<string>(async (page) =>
	{
		try
		{
			IsBusy = true;
			if (CurrentPage != page)
			{
				var pageType = Type.GetType(GlobalVariables.pageFolder + page);
				if (pageType != null)
				{
					await Shell.Current.GoToAsync(page, false);
					CurrentPage = page;
				}
				else
				{
					await Shell.Current.DisplayAlert("Error:", $"{page} - Not available", "OK");
				}
			}
		}
		finally
		{
			IsBusy = false;
		}
	}, (page) => IsNotBusy);

	public Command GoBackToPageCommand => new Command<string>(async (page) =>
	{
		try
		{
			IsBusy = true;
			await Shell.Current.GoToAsync(page, true);
			CurrentPage = string.Empty;
		}
		finally
		{
			IsBusy = false;
		}
	}, (page) => IsNotBusy);

	#region Notification
	CancellationTokenSource? cts;

	string notifText = string.Empty;
	public string NotifText
	{
		get => notifText;
		set
		{
			if (notifText == value) { return; }
			notifText = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(IsNotifVisible));
		}
	}
	public bool IsNotifVisible => !string.IsNullOrWhiteSpace(NotifText);

	public Command ShowNotificationCommand => new Command<string>(async (str) =>
	{
		try
		{
			IsBusy = true;
			cts = new CancellationTokenSource();

			if (!string.IsNullOrWhiteSpace(str))
			{
				string[] theme = await AppThemeService.ReadTheme();
				if (theme.Length == 4)
				{
					if (theme[1] == "0")
					{
						NotifText = str;
						PlatformsService.PlayAudioFile(theme[2] + ".wav");
					}
				}
				var b = int.TryParse(theme[3], out int n);
				await DelayTask(n, cts.Token);
			}
		}
		finally
		{
			NotifText = string.Empty;
			cts = null;
			IsBusy = false;
		}
	}, (str) => IsNotBusy);

	public async Task DelayTask(int ms, CancellationToken token = default)
	{
		try 
		{
			token.ThrowIfCancellationRequested();
			await Task.Delay(ms * 1000, token);  //wait for n seconds
		}  
		catch (TaskCanceledException) { }
		finally
		{
			HideNotificationCommand.Execute(null);
		}
	}

	public Command HideNotificationCommand => new(() =>
	{
		cts?.Cancel();
		//cts?.Dispose();
	});
	#endregion
}
