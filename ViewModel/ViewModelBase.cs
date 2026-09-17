using System;
using System.ComponentModel;

public class ViewModelBase : INotifyPropertyChanged
{

    public ViewModelBase() 
	{

	}

	public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


}
