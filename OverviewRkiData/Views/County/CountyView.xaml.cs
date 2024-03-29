﻿using Codexzier.Wpf.ApplicationFramework.Commands;
using Codexzier.Wpf.ApplicationFramework.Components.Ui.Anims;
using Codexzier.Wpf.ApplicationFramework.Components.Ui.EventBus;
using Codexzier.Wpf.ApplicationFramework.Components.UserSettings;
using OverviewRkiData.Components;
using OverviewRkiData.Components.Data;
using OverviewRkiData.Components.UserSettings;
using OverviewRkiData.Views.Data;
using OverviewRkiData.Views.RenderPicture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Codexzier.Wpf.ApplicationFramework.Controls.Diagram;
using System.Windows;

namespace OverviewRkiData.Views.County
{
    public partial class CountyView
    {
        private readonly CountyViewModel _viewModel;

        private readonly IList<(TextBlock, Storyboard)> _fadeIn = new List<(TextBlock, Storyboard)>();
        private readonly IList<(TextBlock, Storyboard)> _fadeOut = new List<(TextBlock, Storyboard)>();

        public CountyView()
        {
            this.InitializeComponent();

            this._viewModel = (CountyViewModel)this.DataContext;

            EventBusManager.Register<CountyView, BaseMessage>(this.CountyMessageEvent);
            this.InitializeAnimation();

            this._viewModel.CommandCreatePicture = new ButtonCommandCreatePicture(this._viewModel, this.RenderPicturePrint);
        }

        private void InitializeAnimation()
        {
            var setting = UserSettingsLoader<CustomSettingsFile>.GetInstance(SerializeHelper.Serialize, SerializeHelper.Deserialize).Load();
            this._viewModel.ShowAnimationOn = setting.DiagramAnimationOn;
            this._viewModel.ShowBarsFromRightToLeft = setting.DiagramAnimationRightToLeft;
            this._viewModel.ScaledDiagram = setting.LastScaledDiagram;


            this._fadeOut.Add((this.tbDescription_Name, new Storyboard()));
            this._fadeOut.Add((this.tbValue_Name, new Storyboard()));

            this._fadeOut.Add((this.tbDescription_Deaths, new Storyboard()));
            this._fadeOut.Add((this.tbValue_Deaths, new Storyboard()));

            this._fadeOut.Add((this.tbDescription_WeekIncidence, new Storyboard()));
            this._fadeOut.Add((this.tbValue_WeekIncidence, new Storyboard()));

            for (int index = 0; index < this._fadeOut.Count; index++)
            {
                Animations.SetFade(this._fadeOut[index].Item1, this._fadeOut[index].Item2, 1d, 0d, 0, 0);
            }

            this._fadeIn.Add((this.tbDescription_Name, new Storyboard()));
            this._fadeIn.Add((this.tbValue_Name, new Storyboard()));

            this._fadeIn.Add((this.tbDescription_Deaths, new Storyboard()));
            this._fadeIn.Add((this.tbValue_Deaths, new Storyboard()));

            this._fadeIn.Add((this.tbDescription_WeekIncidence, new Storyboard()));
            this._fadeIn.Add((this.tbValue_WeekIncidence, new Storyboard()));

            for (int index = 0; index < this._fadeIn.Count; index++)
            {
                Animations.SetFade(this._fadeIn[index].Item1, this._fadeIn[index].Item2, 0d, 1d, (index * 100) + 10, 1000);
            }
        }

        private async void CountyMessageEvent(IMessageContainer obj)
        {
            foreach (var item in this._fadeOut)
            {
                item.Item2.Begin();
            }

            if (obj.Content is DistrictItem districtItem)
            {
                foreach (var item in this._fadeIn)
                {
                    item.Item2.Begin();
                }

                this._viewModel.DistrictData = districtItem;
                var setting = UserSettingsLoader<CustomSettingsFile>.GetInstance(SerializeHelper.Serialize, SerializeHelper.Deserialize).Load();
                var getLastDays = 0;

                await Task.Run(() =>
                {
                    if (setting.OnlyShowLast200Values)
                    {
                        getLastDays = 100;
                    }
                    
                    var result = HelperExtension.GetCountyResults(
                        districtItem.Name, 
                        setting.FillMissingDataWithDummyValues,
                        getLastDays);

                    var enumerable = result as Landkreis[] ?? result.ToArray();

                    var lastDay = 1;
                    var countyResults = enumerable.Select(s =>
                     {
                         var toolTip = $"{s.Date:d} | {s.WeekIncidence:N1} | {s.Deaths}";

                         var temp = new DiagramLevelItem
                         {
                             Value = s.WeekIncidence,
                             ToolTipText = toolTip,
                             SetHighlightMark = s.Date.Day == 1 || s.Date.Day < lastDay,
                             SetColor = s.Name.Equals("Dummy") ? 2 : 0
                         };

                         lastDay = s.Date.Day;

                         return temp;
                     });

                    // if (setting.OnlyShowLast200Values)
                    // {
                    //     countyResults = countyResults.TakeLast(100);
                    // }


                    this._viewModel.CountyResults = countyResults.ToList();

                    var today = DateTime.Today;
                    var oneDayTrend = this.GetIncidenceTrendByOneDay(enumerable, today);
                    this._viewModel.Trend = $"{oneDayTrend:N1} ({this.TrendInOneWord(oneDayTrend)})";
                    var weekTrend = this.GetIncidenceTrendByWeek(enumerable, today);
                    this._viewModel.WeekTrend = $"{weekTrend:N1} ({this.TrendInOneWord(weekTrend)})";

                    var deathResult = enumerable.Select(s =>
                    {
                        var toolTip = $"{s.Date:d} | {s.Deaths:N1} | {s.WeekIncidence:N1}";
                        return new DiagramLevelItem { Value = s.Deaths, ToolTipText = toolTip };
                    });

                    if (setting.OnlyShowLast200Values)
                    {
                        deathResult = deathResult.TakeLast(100);
                    }

                    // TODO: funktioinert nur, wenn vor to list ausgeführt wurde
                    // Vielleicht funktioniert das direkt so verarbeitet wird?
                    //var lastDeathRateValue = 0d;
                    //foreach (var item in deathResult)
                    //{
                    //    var lastDayTemp = item.Value;
                    //    item.Value -= lastDeathRateValue;
                    //    lastDeathRateValue = lastDayTemp;
                    //}

                    this._viewModel.CountyDeathResults = deathResult.ToList();

                    // TODO Obsolete, wird nicht verwendet
                    this.Dispatcher.Invoke(delegate
                    {
                        this.RenderPicturePrint.DataContext = new RenderPicturePrintViewModel
                        {
                            CountyResults = this._viewModel.CountyResults,
                            DistrictData = this._viewModel.DistrictData
                        };
                    });


                });
            }
        }

        private string TrendInOneWord(double oneDayTrend)
        {
            return oneDayTrend != 0.0 ? oneDayTrend > 0 ? "Up" : "Down" : "Hold";
        }

        private double GetIncidenceTrendByOneDay(Landkreis[] enumerable, DateTime today)
        {
            var todayValue = this.GetIncidenceByDay(enumerable, today);
            var lastDayValue = this.GetIncidenceByDay(enumerable, today.AddDays(-1));

            return todayValue - lastDayValue;
        }

        private double GetIncidenceByDay(Landkreis[] enumerable, DateTime today)
        {
            var lastDayIncidence = enumerable.FirstOrDefault(w =>
                w.Date.Day == today.Day &&
                w.Date.Month == today.Month &&
                w.Date.Year == today.Year);

            var lastDayValue = 0d;

            if (lastDayIncidence != null)
            {
                lastDayValue = lastDayIncidence.WeekIncidence;
            }

            return lastDayValue;
        }

        private double GetIncidenceTrendByWeek(Landkreis[] enumerable, DateTime today)
        {
            var listResults = new List<double>();
            var last = this.GetIncidenceByDay(enumerable, today);
            for (var i = 1; i < 7; i++)
            {
                var fromLastValue = this.GetIncidenceByDay(enumerable, today.AddDays(-i));

                var trend = last - fromLastValue;
                last = fromLastValue;

                listResults.Add(trend);
            }

            return listResults.Average();
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var instance = UserSettingsLoader<CustomSettingsFile>.GetInstance(SerializeHelper.Serialize, SerializeHelper.Deserialize);
            var setting = instance.Load();
            setting.LastScaledDiagram = e.NewValue;
            instance.Save(setting);
        }
    }
}
