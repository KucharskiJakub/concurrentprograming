using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Calculator.Annotations;
using Calculator.Commands;

namespace Calculator.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{

    private string _screenVal;
    private List<string> _availableOperations = new List<string> { "+", "-", "/", "*" };
    private DataTable _dataTable = new DataTable();
    private bool _isLastSignAnOperation;
    private bool _isLastSignAMinus;
    private bool _isLastSignAComma;

    public MainViewModel()
    {
        ScreenVal = "0";
        AddNumberCommand = new RelayCommands(AddNumber);
        AddOperationCommand = new RelayCommands(AddOperation, CanAddOperation);
        ClearScreenCommand = new RelayCommands(ClearScreen);
        GetResultCommand = new RelayCommands(GetResult, CanAddOperation);
        AddMinusCommand = new RelayCommands(AddMinus, CanAddMinus);
        AddCommaCommand = new RelayCommands(AddComma, CanAddComma);
        DeleteOperationCommand = new RelayCommands(DeleteOperation);
    }
    

    private void AddComma(object obj)
    {
        var comma = obj as string;

        if (ScreenVal == "0" && comma != ",")
            ScreenVal = string.Empty;

        else if (comma == "," && _availableOperations.Contains(ScreenVal.Substring(ScreenVal.Length - 1)))
            comma = "0,";

        ScreenVal += comma;
        _isLastSignAComma = true;
        _isLastSignAnOperation = false;
        _isLastSignAMinus = false;
    }

    private void AddMinus(object obj)
    {
        var operation = obj as string;

        ScreenVal += operation;

        _isLastSignAMinus = true;
        _isLastSignAnOperation = true;
        _isLastSignAComma = false;
    }


    private void AddNumber(object obj)
    {
        var number = obj as string;

        if (ScreenVal == "0" && number != ",")
            ScreenVal = string.Empty;
        
        ScreenVal += number;

        _isLastSignAnOperation = false;
        _isLastSignAMinus = false;
    }
    
    private void AddOperation(object obj)
    {
        var operation = obj as string;

        ScreenVal += operation;
        
        _isLastSignAnOperation = true;
        _isLastSignAMinus = false;
        _isLastSignAComma = false;
    }

    private void ClearScreen(object obj)
    {
        ScreenVal = "0";
        
        _isLastSignAnOperation = false;
        _isLastSignAMinus = false;
        _isLastSignAComma = false;
    }
    
    private void GetResult(object obj)
    {
        var result = Math.Round(Convert.ToDouble(_dataTable.Compute(ScreenVal.Replace(",", "."), "")), 2);

        ScreenVal = result.ToString();

        if (Convert.ToDouble(ScreenVal) % 1 == 0)
            _isLastSignAComma = false;
        else
        {
            _isLastSignAComma = true;
        }
    }
    
    private void DeleteOperation(object obj)
    {
        if (ScreenVal.Substring(ScreenVal.Length - 1) == ",")
            _isLastSignAComma = false;
        
        ScreenVal = ScreenVal.Remove(ScreenVal.Length-1, 1);
        if (string.IsNullOrEmpty(ScreenVal))
            ScreenVal = "0";
        else
        {
            if (ScreenVal.Substring(ScreenVal.Length - 1) == ",")
            {
                _isLastSignAComma = true;
                _isLastSignAnOperation = false;
                _isLastSignAMinus = false;
            }
            else if (ScreenVal.Substring(ScreenVal.Length - 1) == "-")
            {
                _isLastSignAnOperation = true;
                _isLastSignAMinus = true;
            }
            else if (_availableOperations.Contains(ScreenVal.Substring(ScreenVal.Length - 1)))
            {
                _isLastSignAMinus = false;
                _isLastSignAnOperation = true;
            }
            else
            {
                _isLastSignAnOperation = false;
                _isLastSignAMinus = false;
            }
        }
        
    }
    
    
    private bool CanAddOperation(object obj) => !_isLastSignAnOperation;
    
    private bool CanAddMinus(object obj) => !_isLastSignAMinus;
    
    private bool CanAddComma(object obj) => !_isLastSignAComma;


    public ICommand AddNumberCommand { get; set; }
    
    public ICommand AddOperationCommand { get; set; }
    
    public ICommand ClearScreenCommand { get; set; }
    
    public ICommand GetResultCommand { get; set; }
    
    public ICommand AddMinusCommand { get; set; }
    
    public ICommand AddCommaCommand { get; set; }
    
    public ICommand DeleteOperationCommand { get; set; }

    public string ScreenVal
    {
        get { return _screenVal;  }
        set
        {
            _screenVal = value;
            OnPropertyChanged();
        }
    }
        

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

