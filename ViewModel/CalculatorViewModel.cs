using System.ComponentModel;
using System.Windows.Input;

namespace calc.ViewModel;

public class CalculatorViewModel : INotifyPropertyChanged
{
    private readonly CalculatorModel _model = new CalculatorModel();

    private string _displayText = "0";
    public string DisplayText
    {
        get => _displayText;
        set { _displayText = value; OnPropertyChanged(nameof(DisplayText)); }
    }

    private double _leftOperand;
    private string _operation;
    private bool _isNewEntry = true;

    public ICommand NumberCommand { get; }
    public ICommand OperationCommand { get; }
    public ICommand EqualsCommand { get; }
    public ICommand ClearCommand { get; }

    public CalculatorViewModel()
    {
        NumberCommand = new RelayCommand(param => AppendNumber(param.ToString()));
        OperationCommand = new RelayCommand(param => SetOperation(param.ToString()));
        EqualsCommand = new RelayCommand(_ => Calculate());
        ClearCommand = new RelayCommand(_ => Clear());
    }

    private void AppendNumber(string number)
    {
        if (_isNewEntry)
        {
            DisplayText = number;
            _isNewEntry = false;
        }
        else
        {
            DisplayText += number;
        }
    }

    private void SetOperation(string op)
    {
        _leftOperand = double.Parse(DisplayText);
        _operation = op;
        _isNewEntry = true;
    }

    private void Calculate()
    {
        double rightOperand = double.Parse(DisplayText);
        double result = _model.Evaluate(_leftOperand, rightOperand, _operation);
        DisplayText = result.ToString();
        _isNewEntry = true;
    }

    private void Clear()
    {
        DisplayText = "0";
        _leftOperand = 0;
        _operation = null;
        _isNewEntry = true;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
