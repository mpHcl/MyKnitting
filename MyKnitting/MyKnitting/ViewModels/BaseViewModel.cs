using MyKnitting.Models;
using MyKnitting.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace MyKnitting.ViewModels {
    public class BaseViewModel : INotifyPropertyChanged {
        public IDataStore<Project> ProjectsDataStore => DependencyService.Get<IDataStore<Project>>();
        public IDataStore<Needle> NeedlesDataStore => DependencyService.Get<IDataStore<Needle>>();
        public IDataStore<Yarn> YarnsDataStore => DependencyService.Get<IDataStore<Yarn>>();
        public IDataStore<YarnsForProject> YFPDataStore => DependencyService.Get<IDataStore<YarnsForProject>>();
        public IDataStore<NeedlesForProjects> NFPDataStore => DependencyService.Get<IDataStore<NeedlesForProjects>>();

        bool isBusy = false;
        public bool IsBusy {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null) {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
