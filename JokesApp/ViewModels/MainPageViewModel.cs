using JokesApp.Models;
using JokesApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public bool IsVisible { get => joke is TwoPartJoke; }
        public string SetUp { get => setup; set { setup = value; OnPropertyChanged(); } }
        public string Delivery { get => delivery; set { delivery = value; OnPropertyChanged(); } }  
        

        public ICommand GetJokeCommand { get; private set; }

        public MainPageViewModel(JokeService service) 
        {
            joke = null;
            this.service=service;
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
        }
    }
}
