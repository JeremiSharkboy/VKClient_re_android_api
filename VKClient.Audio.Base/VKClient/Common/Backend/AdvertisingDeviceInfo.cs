using Microsoft.Phone.Info;
using Newtonsoft.Json;
using System;
using VKClient.Audio.Base;
using Windows.System.UserProfile;

namespace VKClient.Common.Backend
{
    public class AdvertisingDeviceInfo
    {
        private static readonly string ERROR_READING_ADV_ID = "-1";
        private static readonly string ADV_ID_IS_NOT_SUPPORTED = "-2";
        private static readonly string ADV_ID_USE_IS_FORBIDDEN = "-3";
        private PhoneAppInfo _phoneAppInfo;
        //{"app_version":"4.13.1","system_name":"Android","system_version":"6.0","manufacturer":"Fly","device_model":"FS407","ads_device_id":"d30bbfeb-0fd9-4e6c-96ef-7229b5424937","ads_tracking_disabled":1,"ads_android_id":"3da1047301b670fc"}
        public string app_version
        {
            get
            {
                return "4.13.1";//this._phoneAppInfo.AppVersion;
            }
        }

        public string system_name
        {
            get
            {
                return "Android";// "Windows Phone";
            }
        }

        public string system_version
        {
            get
            {
                return "6.0";// AppInfo.OSVersion;
            }
        }

        public string manufacturer
        {
            get
            {
                try
                {
                    return DeviceStatus.DeviceManufacturer;
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        public string device_model
        {
            get { return this._phoneAppInfo.Device; }
        }

        public string ads_device_id
        {
            get
            {
                try
                {
                    string advertisingId = AdvertisingManager.AdvertisingId;
                    if (!string.IsNullOrWhiteSpace(advertisingId))
                        return advertisingId;
                    return AdvertisingDeviceInfo.ADV_ID_USE_IS_FORBIDDEN;
                }
                catch (Exception)
                {
                    return AdvertisingDeviceInfo.ERROR_READING_ADV_ID;
                }
            }
        }

        public int ads_tracking_disabled
        {
            get
            {
                string adsDeviceId = this.ads_device_id;
                return adsDeviceId == AdvertisingDeviceInfo.ADV_ID_USE_IS_FORBIDDEN || adsDeviceId == AdvertisingDeviceInfo.ADV_ID_IS_NOT_SUPPORTED ? 1 : 0;
            }
        }

        public string ads_android_id
        {
            get { return ""; }
        }


        public AdvertisingDeviceInfo()
        {
            this._phoneAppInfo = AppInfo.GetPhoneAppInfo();
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
