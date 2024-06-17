

namespace FileComparer
{
    public static class SessionManager
    {
        private static List<string> sessions = new List<string>();
       
        private static Dictionary<string, string> sessionData = new Dictionary<string, string>();
        public static List<string> SessionNames => sessions;
        public static void AddSession(string sessionName, string sessiondata)
        {
            sessions.Add(sessionName);
            sessionData[sessionName] = sessiondata;
        }
        public static string GetSessionData(string sessionName)
        {
            if (sessionData.ContainsKey(sessionName))
                return sessionData[sessionName];
            else
                throw new ArgumentException("Session name does not exist.");
        }
         
    }
}

