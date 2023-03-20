using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;
using Visualizer.Model;
using Visualizer.Responses;
using Visualizer.Util;

namespace Visualizer.ViewModel
{
    public class DailyPaneViewModel : ViewModelBase
    {
        public ObservableCollection<TagItemViewModel> DailyTags { get; set; }

        public DelegateCommand DeleteTagFromDailyListCommand { get; set; }
        public DelegateCommand PrintDailyListCommand { get; set; }

        private TagItemViewModel _selectedTag;

        private PrintUtility _printUtility;

        public DailyPaneViewModel()
        {
            this.DailyTags = new ObservableCollection<TagItemViewModel>();

            this.DeleteTagFromDailyListCommand = new DelegateCommand(DeleteTagFromDailyList);
            this.PrintDailyListCommand = new DelegateCommand(PrintDailyList);
        }

        private void PrintDailyList(object action)
        {
            var dateTime = DateTime.Now.ToString("D");
            StringBuilder fileBuilder = new StringBuilder();
            fileBuilder.AppendLine($"{dateTime} Daily List");

            foreach (var dailyTag in this.DailyTags)
            {
                var tagResult = dailyTag.TagResult;
                fileBuilder.AppendLine($"Tag ID: {tagResult.Id}, {tagResult.Name}");
            }

            string fileName = $"{dateTime}_DailyList.txt";
            File.WriteAllText(fileName, fileBuilder.ToString());

            this._printUtility = new PrintUtility(fileName);
            this._printUtility.Print();
        }

        private void DeleteTagFromDailyList(object action)
        {
            if (this.SelectedTag == null)
            {
                return;
            }

            this.DailyTags.Remove(this.SelectedTag);
            RefreshBindings();
        }

        public void AddTagToDailyList(TagResult tagResult)
        {
            // Add the tag result and sort the list by tag IDs
            List<TagItemViewModel> dailyTags = this.DailyTags.ToList();

            // If the tag already exists in the daily list, we don't want to re-add it
            var tagExists = dailyTags.Exists(tag => tag.TagResult.Id == tagResult.Id);
            if (tagExists == true)
            {
                return;
            }

            var dailyTag = new TagItemViewModel() { TagResult = tagResult};

            dailyTags.Add(dailyTag);
            dailyTags = dailyTags.OrderBy(tag => tag.TagResult.Name).ToList();

            this.DailyTags = new ObservableCollection<TagItemViewModel>(dailyTags);

            RefreshBindings();
        }

        private void RefreshBindings()
        {
            OnPropertyChanged(nameof(DailyTags));
            OnPropertyChanged(nameof(DailyListTagCount));
        }

        public string DailyListTagCount
        {
            get => $"Number of Tags: {this.DailyTags.Count}";
        }

        public TagItemViewModel SelectedTag
        {
            get =>
                this._selectedTag;
            set
            {
                this._selectedTag = value;
                OnPropertyChanged(() => SelectedTag);
            }
        }
    }
}
