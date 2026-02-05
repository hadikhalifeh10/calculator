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
    
    // History tracking
    private readonly List<string> _history = new List<string>();
    private int _historyIndex = -1;

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
        
        // Save result to history
        _history.Add(DisplayText);
        _historyIndex = _history.Count; // Reset to end of history
        
        _isNewEntry = true;
    }

    private void Clear()
    {
        DisplayText = "0";
        _leftOperand = 0;
        _operation = null;
        _isNewEntry = true;
    }
    
    public void ShowPreviousAnswer()
    {
        if (_history.Count == 0) return;
        
        if (_historyIndex > 0)
        {
            _historyIndex--;
            DisplayText = _history[_historyIndex];
            _isNewEntry = true;
        }
    }
    
    public void ShowNextAnswer()
    {
        if (_history.Count == 0) return;
        
        if (_historyIndex < _history.Count - 1)
        {
            _historyIndex++;
            DisplayText = _history[_historyIndex];
            _isNewEntry = true;
        }
        else if (_historyIndex == _history.Count - 1)
        {
            // Go back to the current display
            _historyIndex = _history.Count;
            DisplayText = "0";
            _isNewEntry = true;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
