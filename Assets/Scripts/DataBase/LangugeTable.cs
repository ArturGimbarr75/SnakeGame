using System.Collections.Generic;

using Assets.Scripts.Settings;


namespace Assets.Scripts.DataBase
{
    /// <summary>
    /// Класс для работы с таблицой языков
    /// </summary>
    public class LanguageTable : TableBase
    {
        private static Dictionary<string, string> English;
        private static Dictionary<string, string> Lithuanian;
        private static Dictionary<string, string> Russian;
        private const string NotFoundStr = "Text not found";

        public string GetElementText(string elName, UISettingsAttributes.Language language)
        {
            string sqlQuery;

            switch (language)
            {
                case UISettingsAttributes.Language.English:
                    if (English == null || !English.ContainsKey(elName))
                    {
                        sqlQuery = "select ename, english from Languages";
                        if (English == null || !English.ContainsKey(elName))
                            UpdateDictionaryInfo (ref English, sqlQuery);
                        if (English != null && English.ContainsKey(elName))
                            return English[elName];
                    }
                    else
                        return English[elName];
                    break;

                case UISettingsAttributes.Language.Lithuanian:
                    if (Lithuanian == null || !Lithuanian.ContainsKey(elName))
                    {
                        sqlQuery = "select ename, lithuanian from Languages";
                        if (Lithuanian == null || !Lithuanian.ContainsKey(elName))
                            UpdateDictionaryInfo(ref Lithuanian, sqlQuery);
                        if (Lithuanian != null && Lithuanian.ContainsKey(elName))
                            return Lithuanian[elName];
                    }
                    else
                        return Lithuanian[elName];
                    break;

                case UISettingsAttributes.Language.Russian:
                    if (Russian == null || !Russian.ContainsKey(elName))
                    {
                        sqlQuery = "select ename, russian from Languages";
                        if (Russian == null || !Russian.ContainsKey(elName))
                            UpdateDictionaryInfo(ref Russian, sqlQuery);
                        if (Russian != null && Russian.ContainsKey(elName))
                            return Russian[elName];
                    }
                    else
                        return Russian[elName];
                    break;
            }

            return NotFoundStr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textContainer"></param>
        /// <param name="sqlQuery"></param>
        private void UpdateDictionaryInfo (ref Dictionary<string, string> textContainer, string sqlQuery)
        {
            textContainer = new Dictionary<string, string>();

            dbconn.Open();
            dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = sqlQuery;
            reader = dbcmd.ExecuteReader();

            while (reader.Read())
            {
                string value = reader.GetString(1);
                string key = reader.GetString(0);
                textContainer.Add(key, value);
            }

            reader.Close();
            dbcmd.Dispose();
            dbconn.Close();
        }

    }

}