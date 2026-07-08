using ECOMSYSTEM.Shared;
namespace ECOMSYSTEM.Web
{
    public class MyService : IMyInterface
    {
        public string getSecurityKey(string keyType)
        {
            return ($"myKery is {keyType}");
        }
    }
}
