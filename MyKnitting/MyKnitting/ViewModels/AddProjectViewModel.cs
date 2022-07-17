using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Xamarin.Essentials;
using Xamarin.Forms;

using MyKnitting.Models;
using MyKnitting.Services;
using MyKnitting.Views;

namespace MyKnitting.ViewModels {
    public class AddProjectViewModel : BaseViewModel {
        private string name = "nazwa";
        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }

        public Command AddPhoto { get; }
        string projectPhoto = "";

        Tools.IImageResize resizer => DependencyService.Get<Tools.IImageResize>();
        private async void AddPhotoFunc() {
            try {
                var result = await FilePicker.PickAsync(PickOptions.Images);
                var new_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), result.FileName);
                ////File.Copy(result.FullPath, new_path, true);
                ////File.WriteAllBytes(new_path, Droid.Resize(File.ReadAllBytes(result.FullPath), 800, 600).Result);
                //SkiaSharp.SKBitmap originalImage = SkiaSharp.SKBitmap.Decode(File.ReadAllBytes(new_path));
                //var resizedImage = originalImage.Resize(new SkiaSharp.SKImageInfo(800, 600), SkiaSharp.SKFilterQuality.Medium);


                File.WriteAllBytes(new_path, resizer.Resize(File.ReadAllBytes(result.FullPath), 1600, 900));


                projectPhoto = new_path;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        public Command AddPattern { get; }
        string projectPattern = "";
        private async void AddPatternFunc() {
            try {
                var result = await FilePicker.PickAsync();
                var new_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), result.FileName);
                File.Copy(result.FullPath, new_path, true);
                projectPattern = result.FileName;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        public Command SaveProject { get; }
        private async void SaveProjectFunc() {
            Project project = new Project { Id = 0, Name = name, Pattern = projectPattern, Photo = projectPhoto, Done = "false" };
            await ProjectsDataStore.AddItemAsync(project);
            await Shell.Current.DisplayAlert("Włóczka i druty", "Włóczkę i druty do projektu można dodać przy edycji projektu!!", "Ok.");
            Shell.Current.SendBackButtonPressed();
        }

        public AddProjectViewModel() {
            AddPhoto = new Command(AddPhotoFunc);
            AddPattern = new Command(AddPatternFunc);
            SaveProject = new Command(SaveProjectFunc);
        }



    }
}
