using System.Collections;
using System.Windows.Input;
using Model;

namespace ViewModel
{
    public class ViewModelWindow : ViewModelBase
    {
        private int _numberOfBalls;
        private readonly ModelBase _modelbase;
        private IList _balls;

        public ViewModelWindow() : this(ModelBase.CreateApi()) { }
        public ViewModelWindow(ModelBase modelbase)
        {
            _modelbase = modelbase;
            Start = new RelayCommand(() => StartCommand());
            Stop = new RelayCommand(() => StopCommand());
        }

        public ICommand Start { get; set; }
        
        public ICommand Stop { get; set; }

        public int NumberOfBalls
        { 
            get => _numberOfBalls;
            set
            {
                if (value.Equals(_numberOfBalls))
                    return;
                if (value < 0)
                    value = 0;
                else if (value > 1000)
                    value = 1000;
                _numberOfBalls = value;
                RaisePropertyChanged(nameof(NumberOfBalls));
            }
        }

        public void StartCommand()
        {
            Balls = _modelbase.Balls(_numberOfBalls);
            _modelbase.Animation(Balls);
        }
        
        public void StopCommand()
        {
            _modelbase.StopAnimation(_balls);
        }
        public IList Balls
        {
            get => _balls;
            set
            {
                if (value.Equals(_balls))
                    return;
                _balls = value;
                RaisePropertyChanged(nameof(Balls));
            }
        }
        
    }
}
