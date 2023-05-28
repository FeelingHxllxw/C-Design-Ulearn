using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Failures
{
    public class ReportMaker
    {
        /// <summary>
        /// </summary>
        /// <param name="day"></param>
        /// <param name="failureTypes">
        /// 0 for unexpected shutdown, 
        /// 1 for short non-responding, 
        /// 2 for hardware failures, 
        /// 3 for connection problems
        /// </param>
        /// <param name="deviceId"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes,
            int[] deviceId,
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            DateTime[] CollDate = new DateTime[times.GetLength(0)];
            for (int i = 0; i < times.GetLength(0); i++)
                CollDate[i] = new DateTime((int)times[i][2], (int)times[i][1], (int)times[i][0]);
            List<Device> newDevices = new List<Device>();
            for (int i = 0; i < devices.Count; i++)
                newDevices.Add(new Device(deviceId[i], (string)devices[i]["Name"], new Failure(failureTypes[i], CollDate[i])));
            DateTime completeDate = new DateTime(year, month, day);
            var result = FindDevicesFailedBeforeDate(completeDate, newDevices);
            return result;
        }

        public static List<string> FindDevicesFailedBeforeDate(DateTime finalDate, List<Device> devices)
        {
            List<string> name_list = new List<string>();
            for (int i = 0; i < devices.Count; i++)
                if (Failure.Earlier(finalDate, devices[i].Fail.FailureDate) && Failure.IsFailureSerious(devices[i].Fail.FailureType) == 1)
                    name_list.Add(devices[i].Name);
            return name_list;
        }
    }


    public class Failure
    {
        public FailureType FailureType;
        public DateTime FailureDate;
        public Failure(int failureType, DateTime failureDate)
        {
            FailureType = (FailureType)failureType;
            FailureDate = failureDate;
        }
        public static int IsFailureSerious(FailureType failureType)
        {
            if (failureType == FailureType.UnexpectedShutdown || failureType == FailureType.HardwareFailure)
                return 1;
            else
                return 0;
        }
        public static bool Earlier(DateTime finalDate, DateTime failureTime) => failureTime < finalDate;
    }

    public class Device
    {
        public int DeviceId;
        public string Name;
        public Failure Fail;
        public Device(int deviceId, string name, Failure failure)
        {
            DeviceId = deviceId;
            Name = name;
            Fail = failure;
        }
    }

    public enum FailureType
    {
        UnexpectedShutdown,
        ShornNonResponding,
        HardwareFailure,
        ConnectionProblem
    }

}
