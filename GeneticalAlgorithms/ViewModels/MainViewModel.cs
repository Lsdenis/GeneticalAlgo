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
        private List<Item> _items;
        private IList<DataPoint> _itemsSeries;
        private string _itemValue;
        private string _maxItemValueFunction;
        private ICommand _nextStepButtonCommand;

        protected abstract int MinValue { get; }

        protected abstract int MaxValue { get; }

        public int SolutionAccuracy { get; set; }

        public List<Item> Items
        {
            get => _items;
            protected set
            {
                _items = value?.OrderBy(item => item.GetDoubleValue(MinValue, MaxValue)).ToList();

                if (value == null)
                {
                    return;
                }

                ItemsSeries = _items.Select(item => new DataPoint(item.GetDoubleValue(MinValue, MaxValue),
                    Function(item.GetDoubleValue(MinValue, MaxValue)))).ToList();

                var maxItem = _items.Aggregate((i, j) =>
                    Function(i.GetDoubleValue(MinValue, MaxValue)) > Function(j.GetDoubleValue(MinValue, MaxValue))
                        ? i
                        : j);

                var doubleValue = maxItem.GetDoubleValue(MinValue, MaxValue);
                ItemValue = doubleValue.ToString();
                MaxItemValueFunction = Function(doubleValue).ToString();
            }
        }

        public int PopulationNumber { get; set; }

        public double MutationPossibility { get; set; }

        public double CrossingoverPossibility { get; set; }

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

        protected abstract double Function(double inputValue);

        protected abstract void OnCalculateClicked();

        protected abstract void OnNextStepClicked();

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}