using JokesApp.Models;
using JokesApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JokesApp.ViewModels
{
    public class MainPageViewModel:ViewModelBase
    {
        private readonly JokeService service;
        private Joke joke;
        private string setup;
        private string delivery;
        private string selectedCat;

        public bool IsVisible { get => joke is TwoPartJoke; }
        public string SetUp { get => setup; set { setup = value; OnPropertyChanged(); } }
        public string Delivery { get => delivery; set { delivery = value; OnPropertyChanged(); } }  
        public string SelectedCat { get => selectedCat; set { selectedCat = value; OnPropertyChanged(); if (selectedCat == null) SelectedCat = "Any"; } }
        public ObservableCollection<string> Cats { get; set; }

        public ICommand GetJokeCommand { get; private set; }

        public ICommand SubmitJokeCommand { get; private set; }

        public MainPageViewModel(JokeService service) 
        {
            joke = null;
            this.service=service;
            Cats = new();
            GetCatsAsync();
            GetJokeCommand = new Command(async () =>
            {
                joke = await service.GetRandomJoke();
                if (joke is OneLiner)
                {
                    SetUp =((OneLiner)joke).Joke;
                }
                if (joke is TwoPartJoke)
                {
                    SetUp=((TwoPartJoke)joke).Setup;
                    Delivery= ((TwoPartJoke)joke).Delivery;
                    
                }
              OnPropertyChanged(nameof(IsVisible));

            } );

            SubmitJokeCommand= new Command(async () => { await SubmitJoke(); });
        }
        private async void GetCatsAsync()
        {
            List<string> cats = await service.GetCatsAsync();
            foreach (string c in cats)
            {
                Cats.Add(c);
            }
        }

        private async Task SubmitJoke()
        {
           OneLiner j = this.joke as OneLiner; 
           MyJoke joke= new MyJoke() { Flags=j.Flags, Joke=j.Joke };
            if (await service.SubmitJokeAsync(joke))
               await AppShell.Current.DisplayAlert("Ha Ha Ha", "LOL", "Ok");
            else
                await AppShell.Current.DisplayAlert("DUH!", "SAD SO SAD!", "Ok");

        }
    }
}
