using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GeneticalAlgorithms.Annotations;
using GeneticalAlgorithms.Core.Items;
using GeneticalAlgorithms.Custom;
using OxyPlot;

namespace GeneticalAlgorithms.ViewModels
{
    public abstract class MainViewModel : INotifyPropertyChanged
    {
        private ICommand _calculateButtonCommand;
        private IList<DataPoint> _functionSeries;
        private IList<DataPoint> _itemsSeries;
        private string _itemValue;
        private string _maxItemValueFunction;
        private ICommand _nextStepButtonCommand;

        protected abstract int MinValue { get; }

        protected abstract int MaxValue { get; }

        public int SolutionAccuracy { get; set; } = 3;

        public abstract List<Item> Items { get; set; }

        public int PopulationNumber { get; set; } = 10;

        public double MutationPossibility { get; set; } = 100;

        public double CrossingoverPossibility { get; set; } = 100;

        public int MaxSteps { get; set; } = 100;

        protected int NumberOfSteps;

        public ICommand CalculateButtonCommand =>
            _calculateButtonCommand ?? (_calculateButtonCommand = new Command(OnCalculateClicked, true));

        public ICommand NextStepButtonCommand =>
            _nextStepButtonCommand ?? (_nextStepButtonCommand = new Command(OnNextStepClicked, true));

        public IList<DataPoint> FunctionSeries
        {
            get => _functionSeries;
            set
            {
                _functionSeries = value;
                OnPropertyChanged();
            }
        }

        public IList<DataPoint> ItemsSeries
        {
            get => _itemsSeries;
            set
            {
                _itemsSeries = value;
                OnPropertyChanged();
            }
        }

        public string ItemValue
        {
            get => _itemValue;
            set
            {
                _itemValue = value;
                OnPropertyChanged();
            }
        }

        public string MaxItemValueFunction
        {
            get => _maxItemValueFunction;
            set
            {
                _maxItemValueFunction = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected abstract double Function(params double[] doubleParams);

        protected abstract void OnCalculateClicked();

        protected abstract void OnNextStepClicked();

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}