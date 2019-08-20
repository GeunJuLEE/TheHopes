using System;

namespace readdb.Models
{
    public class MobilityData
    {
        public DateTime Time { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public double Ele { get; set; }

        public MobilityData()   //초기화
        {
        }

        public MobilityData(DateTime Time, double Latitude, double Longitude, double Ele)   //초기화2
        {   
            this.Time = Time;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.Ele = Ele;
        }
        public override string ToString()   //나중에 프린트 할까봐 Tostring() 오버라이딩 해놈
        {
            return @"Time is " + this.Time +
                    "\nLatitude is " + this.Latitude +
                    "\nLongitude is " + this.Longitude +
                    "\nEle is " + this.Ele + "\n\n";
        }
    }
}
