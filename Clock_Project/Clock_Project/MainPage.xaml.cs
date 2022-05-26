using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Clock_Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();            
            SetTimer();
            DigitalClock();           
            RotateClock();
            WeatherParsing();
            NewsParsing();
            RotateBackground();
            
        }


        private async void DigitalClock()
        {
            while (true)
            {
                DigitalHour.Text = DateTime.Now.ToString("HH");
                DigitalMinute.Text = DateTime.Now.ToString("mm");
                DigitalSecond.Text = DateTime.Now.ToString("ss");
                await Task.Delay(50);
            }

        }

        private void SetTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Elapsed += TimerElapsedEvent;
            timer.Start();
        }

        private void TimerElapsedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            AnalogClock();
        }

        private void AnalogClock()
        {
            int sec = DateTime.Now.Second;
            int min = DateTime.Now.Minute;
            int hour = DateTime.Now.Hour;

            hour = hour % 12;

            sniddle.RotateTo((sec * 6) % 360, 0);
            mniddle.RotateTo((min * 6 + sec / 10) % 360, 0);
            hniddle.RotateTo((hour * 30 + min / 2 + sec / 120) % 360, 0);
        }



        public async void WeatherParsing()
        {
            HttpClient client = new HttpClient();

            while (true)
            {
                HttpResponseMessage weatherResponse = await client.GetAsync("https://www.kma.go.kr/wid/queryDFSRSS.jsp?zone=2818582000");
                

                if (!weatherResponse.IsSuccessStatusCode)
                {   //  오류 검사
                    return;
                }

                string res = await weatherResponse.Content.ReadAsStringAsync();
               
                XmlDocument document = new XmlDocument();
                document.LoadXml(res);

                XmlNode currentWeather = document.DocumentElement.SelectSingleNode("descendant::data");
                
                var weath = currentWeather.SelectSingleNode("wfKor");
                var temp = currentWeather.SelectSingleNode("temp");
                Weatherspoon.Text = weath.InnerText;
                Weatherspoong.Text = temp.InnerText;
                
                if (Weatherspoon.Text == "맑음")
                {
                    nowWeather.Source = "sunny.png";
                    await nowWeather.FadeTo(1, 5000);

                }
                else if (Weatherspoon.Text == "흐림")
                {
                    nowWeather.Source = "cloudy.png";
                    await nowWeather.FadeTo(1, 5000);
                }
                else if (Weatherspoon.Text == "비")
                {
                    nowWeather.Source = "rainny.png";
                    await nowWeather.FadeTo(1, 5000);
                }                
                await Task.Delay(10000);
                await nowWeather.FadeTo(0, 5000);
            }

        }

        public async void NewsParsing()
        {
            HttpClient client = new HttpClient();

            while (true)
            {
                HttpResponseMessage newsResponse = await client.GetAsync("https://www.yonhapnewstv.co.kr/category/news/headline/feed/");

                if (!newsResponse.IsSuccessStatusCode)
                {   //  오류 검사
                    return;
                }                

                string res = await newsResponse.Content.ReadAsStringAsync();
                              
                XmlDocument document = new XmlDocument();
                document.LoadXml(res);
                
                XmlNodeList articles = document.DocumentElement.SelectNodes("descendant::item");
                
                foreach (XmlNode article in articles)
                {
                    var headline = article.SelectSingleNode("title");
                    headLine.Text = headline.InnerText;
                    await headLine.FadeTo(1, 3000);
                    await Task.Delay(12000);
                    await headLine.FadeTo(0, 3000);
                    await Task.Delay(2000);
                }

            }

        }




        private async void RotateClock()
        {
            while (true)
            {
                await clock.RotateTo(720, 20000, Easing.CubicInOut);
                await Task.Delay(8000);
                await clock.RotateTo(0, 20000, Easing.CubicInOut);
                await Task.Delay(8000);
            }
        }

        
        private async void RotateBackground()
        {
            while (true)
            {
                sky.FadeTo(1, 3000);
                dawn.FadeTo(0, 300);
                moon.FadeTo(0, 300);
                underwater.FadeTo(0, 300);
                orora.FadeTo(0, 300);                           
                await Task.Delay(20000);
                sky.FadeTo(0, 3000);
                dawn.FadeTo(1, 3000);
                moon.FadeTo(0, 3000);
                underwater.FadeTo(0, 3000);
                orora.FadeTo(0, 3000);                
                await Task.Delay(20000);
                sky.FadeTo(0, 3000);
                dawn.FadeTo(0, 3000);
                moon.FadeTo(1, 3000);
                underwater.FadeTo(0, 3000);
                orora.FadeTo(0, 3000);                
                await Task.Delay(20000);
                sky.FadeTo(0, 3000);
                dawn.FadeTo(0, 3000);
                moon.FadeTo(0, 3000);
                underwater.FadeTo(1, 3000);
                orora.FadeTo(0, 3000);                
                await Task.Delay(20000);
                sky.FadeTo(0, 3000);
                dawn.FadeTo(0, 3000);
                moon.FadeTo(0, 3000);
                underwater.FadeTo(0, 3000);
                orora.FadeTo(1, 3000); 
                await Task.Delay(20000);                
            }
        }

    }
}
