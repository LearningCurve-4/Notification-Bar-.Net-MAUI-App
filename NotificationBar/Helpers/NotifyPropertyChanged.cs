﻿namespace NotificationBar.Helpers;

public class NotifyPropertyChanged : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
