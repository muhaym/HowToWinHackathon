using System;
using System.Collections.Generic;
using System.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace HowToWinHackathon
{
    public partial class PickPhoto : ContentPage
    {
        public PickPhoto()
        {
            InitializeComponent();
        }
        bool isbusy = false;
        const string MyEmotion = "Check my Emotion";
        const string TakePhoto = "Take a Photo";
        MediaFile file;
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            switch (ActionButton.Text)
            {
                case MyEmotion:
                    PhotoUpload();
                    break;
                case TakePhoto:
                    PickAPhoto();
                    break;
                default: return;
            }
        }

        async void PhotoUpload()
        {
            ActionButton.Text = TakePhoto;
            if (file == null)
            {
                await DisplayAlert("Error", "No Image Taken", "Ok");
                return;
            }
            isbusy = true;
            var result = await Services.Helpers.UploadPhoto(file);
            isbusy = false;
            if (result == null)
            {
                await DisplayAlert("Error", "Unable to send photo to the service", "Ok");
                return;
            }
            //Going to Display the result as alert, if multiple faces selecting the first major one from api
            var Face = result.FirstOrDefault();
            if (Face != null)
            {
                var emotion = "Anger: " + Face.Scores.Anger.ToString("0.0")
                        + "\n Contempt:" + Face.Scores.Contempt.ToString("0.0")
                        + "\n Disgust:" + Face.Scores.Disgust.ToString("0.0")
                        + "\n Fear:" + Face.Scores.Fear.ToString("0.0")
                        + "\n Happiness:" + Face.Scores.Happiness.ToString("0.0")
                        + "\n Neutral:" + Face.Scores.Neutral.ToString("0.0")
                        + "\n Sadness:" + Face.Scores.Sadness.ToString("0.0")
                        + "\n Surprise:" + Face.Scores.Surprise.ToString("0.0");
                await DisplayAlert("Results are out!", emotion, "OK");
            }
            else
            {
                await DisplayAlert("Oops", "No Face Detected!", "OK");
            }

        }

        async void PickAPhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });
            ActionButton.Text = MyEmotion;
            if (file == null)
                return;
            Guide.IsVisible = false;
            ActionButton.Text = MyEmotion;
            MyImage.Source = ImageSource.FromStream(() =>
                                        {
                                            var stream = file.GetStream();
                                            return stream;
                                        });

        }
    }
}
