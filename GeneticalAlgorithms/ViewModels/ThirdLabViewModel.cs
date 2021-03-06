﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GeneticalAlgorithms.Annotations;
using GeneticalAlgorithms.Core;
using GeneticalAlgorithms.Core.Helpers;
using GeneticalAlgorithms.Core.Items;
using GeneticalAlgorithms.Custom;
using Microsoft.Win32;

namespace GeneticalAlgorithms.ViewModels
{
    public class ThirdLabViewModel : INotifyPropertyChanged
    {
        private readonly Action<List<TSPItem>, int[]> _drawPath;
        private const int SkipLines = 6;
        private string _bestFunction;
        private string _bestFunctionValue;
        private ICommand _calculateButtonCommand;
        private List<TSPItem> _items;
        private ICommand _nextStepButtonCommand;

        protected int NumberOfSteps = 0;
        private ICommand _startButtonCommand;

        public ThirdLabViewModel(Action<List<TSPItem>, int[]> drawPath)
        {
            _drawPath = drawPath;
        }

        public List<TSPItem> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        public List<int[]> Solutions { get; set; }

        public int PopulationNumber { get; set; }

        public int MutationPossibility { get; set; } = 50;

        public int CrossingoverPossibility { get; set; } = 100;

        public int MaxSteps { get; set; } = 100;

        public string BestFunction
        {
            get => _bestFunction;
            set
            {
                _bestFunction = value;
                OnPropertyChanged();
            }
        }

        public string BestFunctionValue
        {
            get => _bestFunctionValue;
            set
            {
                _bestFunctionValue = value;
                OnPropertyChanged();
            }
        }

        public ICommand CalculateButtonCommand =>
            _calculateButtonCommand ?? (_calculateButtonCommand = new Command(OnCalculateClicked, true));

        public ICommand StartButtonCommand =>
            _startButtonCommand ?? (_startButtonCommand = new Command(OnStartButtonClicked, true));

        private void OnStartButtonClicked()
        {
            for (int i = 0; i < MaxSteps; i++)
            {
                OnNextStepClicked();
            }
        }

        public ICommand NextStepButtonCommand =>
            _nextStepButtonCommand ?? (_nextStepButtonCommand = new Command(OnNextStepClicked, true));

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnCalculateClicked()
        {
            var openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();

            if (result.GetValueOrDefault() == false)
            {
                return;
            }

            var values = File.ReadAllLines(openFileDialog.FileName);
            values = values.Skip(SkipLines).ToArray();

            var tspItems = new List<TSPItem>();
            var index = 0;
            while (!values[index].Equals("EOF"))
            {
                var splitted = values[index].Split(' ');
                tspItems.Add(new TSPItem(int.Parse(splitted[1]), int.Parse(splitted[2])));
                index++;
            }

            Items = tspItems;

            Solutions = GeneratorHelper.GenerateTSPSolutions(Items.Count, PopulationNumber);
        }

        public void OnNextStepClicked()
        {
            NumberOfSteps++;
            var repoductionSolutions = ReproductionHelper.ReproduceTSP(Items, Solutions, PopulationNumber);
            Solutions.AddRange(CrossingoverHelper.MakeTSPCrossingover(repoductionSolutions));
            Solutions = Solutions.OrderBy(ints => ints.SolutionValue(Items)).Take(PopulationNumber).ToList();
//            MutationHelper.MutateTSP(Solutions, MutationPossibility);

            foreach (var solution in Solutions)
            {
                foreach (var i in solution)
                {
                    if (solution.Count(i1 => i1 == i && i1 != -1) > 1)
                    {
                        var a = i;
                    }
                }
            }

            var bestSolution = new int[0];
            double bestValue = 999999;
            foreach (var solution in Solutions)
            {
                var solutionValue = solution.SolutionValue(Items);
                if (solutionValue < bestValue)
                {
                    bestValue = solutionValue;
                    bestSolution = solution;
                }
            }

            BestFunctionValue = bestSolution.Aggregate(string.Empty, (s, i) => s += $"\t{i}");
            BestFunction = bestValue.ToString();

            _drawPath.Invoke(Items, bestSolution);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}