using System;
using System.Collections.Generic;
using Plugin.Media;

using Xamarin.Forms;

namespace HowToWinHackathon
{
    public partial class PickPhoto : ContentPage
    {
        public PickPhoto()
        {
            InitializeComponent();
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;
            Guide.IsVisible = false;

            await DisplayAlert("File Location", file.Path, "OK");

            MyImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });


        }
    }
}
