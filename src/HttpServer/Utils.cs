using System.Collections.Generic;
using System.Linq;
using HttpServer.Model.Entities;

namespace HttpServer
{
    public class Utils
    {
        public static ushort TranslateSemester(IEnumerable<KeyValuePair<string, string>> content)
        {
            ushort val = 0x00;

            for (int i = 0; i < 10; i++)
                if (content.Where(k => k.Key.Equals(i.ToString())).FirstOrDefault().Value == i.ToString())
                    val = (ushort)(val | (0x01 << i));

            return val;
        }

        public static string SemesterToText(ushort semester)
        {
            int aux = 0x01;
            string ret = "";
            for (int i = 0; i < 10; i++)
            {
                if ((semester & aux) == aux)
                    ret += (ret.Length != 0 ? " | " : "") + (i + 1) + "º";

                aux = aux << 1;
            }

            return ret;
        }

        public static IEnumerable<CurricularUnit> RetrievePrecedencesFromPayload(IEnumerable<KeyValuePair<string, string>> content)
        {
            
        }
    }
}
