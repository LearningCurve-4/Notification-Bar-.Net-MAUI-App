namespace NotificationBar.Helpers.Converters;

public class ToggleMVConverter : IMultiValueConverter
{
	public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values[1] == null || values.Length != 2 || parameter == null) { return null; }

		string s = (string)parameter;
		string[] substrings = s.Split('_');
		if (substrings.Length != 3) { return null; }

		Shell.Current.Resources.TryGetValue(substrings[0], out object on);
		Shell.Current.Resources.TryGetValue(substrings[1], out object off);

		switch (substrings[2])
		{
			case "C":  //Toggle Color by Resource key
				return (bool)values[1] ? (Color)on : (Color)off;
			case "S":  //Toggle Image Source
				return (bool)values[1] ? (string)on : (string)off;
			case "T":  //Toggle Text
				return (bool)values[1] ? substrings[0] : substrings[1];
			default:
				break;
		}
		return null;
	}

	public object[]? ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		return null;
	}
}